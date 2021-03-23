using eduProjectModel.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DbConnectionParameters dbConnectionParameters;

        public UsersRepository(DbConnectionParameters dbConnectionParameters)
        {
            this.dbConnectionParameters = dbConnectionParameters;
        }

        public async Task<User> GetAsync(int id)
        {
            User user = null;

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                user = await ReadBasicUserInfo(command, id);

                if (user != null)
                {
                    await ReadAuthoredProjectIds(command, id, user);

                    await ReadProjectCollaborationIds(command, id, user);
                }

                await connection.CloseAsync();
            }

            return user;
        }

        private async Task ReadProjectCollaborationIds(MySqlCommand command, int id, User user)
        {
            command.CommandText = @"SELECT project_id 
                                    FROM project_collaborator
                                    WHERE user_id = @userId";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@userId",
                Value = user.UserId
            });

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        user.ProjectCollaborationIds.Add(reader.GetInt32(0));
                    }
                }
            }
        }

        private async Task ReadAuthoredProjectIds(MySqlCommand command, int id, User user)
        {
            command.CommandText = @"SELECT project_id 
                                    FROM project
                                    WHERE user_id = @userId";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@userId",
                Value = user.UserId
            });

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        user.AuthoredProjectIds.Add(reader.GetInt32(0));
                    }
                }
            }
        }

        private async Task<User> ReadBasicUserInfo(MySqlCommand command, int id)
        {
            User user = null;

            command.CommandText = @"SELECT user_id, user_account_type_id, first_name, last_name, phone_number, phone_format,
	                                       student.faculty_id, study_year, study_program_id, study_program_specialization_id,
	                                       faculty_member.faculty_id, study_field_id, academic_rank_id
                                    FROM user
                                    INNER JOIN account USING (user_id)
                                    LEFT OUTER JOIN student using(user_id)
                                    LEFT OUTER JOIN faculty_member using(user_id)
                                    WHERE user.user_id = @id;";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@id",
                Value = id
            });

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    await reader.ReadAsync();
                    user = GetUserFromRow(reader);
                }
            }

            return user;
        }

        private User GetUserFromRow(MySqlDataReader reader)
        {
            UserAccountType accountType = (UserAccountType)Enum.ToObject(typeof(UserAccountType), reader.GetInt32(1));
            if (accountType is UserAccountType.Student)
            {
                Student student = new Student
                {
                    UserId = reader.GetInt32(0),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    PhoneNumber = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                    PhoneFormat = !reader.IsDBNull(5) ? reader.GetString(5) : null,
                    FacultyId = reader.GetInt32(6),
                    StudyYear = reader.GetInt32(7),
                    StudyProgramId = reader.GetInt32(8),
                    StudyProgramSpecializationId = !reader.IsDBNull(9) ? (int?)reader.GetInt32(9) : null
                };

                return student;
            }

            else if (accountType is UserAccountType.FacultyMember)
            {
                FacultyMember facultyMember = new FacultyMember
                {
                    UserId = reader.GetInt32(0),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    PhoneNumber = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                    PhoneFormat = !reader.IsDBNull(5) ? reader.GetString(5) : null,
                    FacultyId = reader.GetInt32(10),
                    StudyField = StudyField.fields[reader.GetInt32(11)],
                    AcademicRank = (AcademicRank)Enum.ToObject(typeof(AcademicRank), reader.GetInt32(12))
                };

                return facultyMember;
            }

            return null;
        }

        public async Task AddAsync(User user)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                await AddUserData(command, user);

                await connection.CloseAsync();
            }
        }

        private async Task AddUserData(MySqlCommand command, User user)
        {
            command.CommandText = @"INSERT INTO user
                                    (first_name, last_name)
                                    VALUES
                                    (@firstName, @lastName)";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.String,
                ParameterName = "@fistName",
                Value = user.FirstName
            });

            await command.ExecuteNonQueryAsync();

            if (user is Student s)
            {
                await AddStudentData(command, s);
            }
            else if (user is FacultyMember fm)
            {
                await AddFacultyMemberData(command, fm);
            }

            command.CommandText = @"INSERT INTO user_settings
                                    (user_id)
                                    VALUES
                                    (@userId)";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@userId",
                Value = user.UserId
            });

            await command.ExecuteNonQueryAsync();
        }

        private async Task AddStudentData(MySqlCommand command, Student s)
        {
            command.CommandText = @"INSERT INTO student
                                    (user_id, study_program_id, study_program_specialization_id, study_year)
                                    VALUES
                                    (@userId, @programId, @specializationId, @studyYear)";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@userId",
                Value = s.UserId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@programId",
                Value = s.StudyProgramId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@specialiationId",
                Value = s.StudyProgramSpecializationId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@studyYear",
                Value = s.StudyYear
            });

            await command.ExecuteNonQueryAsync();
        }

        private async Task AddFacultyMemberData(MySqlCommand command, FacultyMember fm)
        {
            command.CommandText = @"INSERT INTO faculty_member
                                    (user_id, faculty_id, study_field_id, academic_rank_id)
                                    VALUES
                                    (@userId, @facultyId, @fieldId, @rankId)";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@userId",
                Value = fm.UserId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@facultyId",
                Value = fm.FacultyId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@fieldId",
                Value = StudyField.fields.Where(f => f.Value.Name == fm.StudyField.Name).First().Key
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@rankId",
                Value = (int)fm.AcademicRank
            });

            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(User user)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                await UpdateUserData(command, user);

                await connection.CloseAsync();
            }
        }

        private async Task UpdateUserData(MySqlCommand command, User user)
        {
            command.CommandText = @"UPDATE user
                                    SET 
                                    first_name = @firstName,
                                    last_name = @lastName
                                    WHERE user_id = @userId";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.String,
                ParameterName = "@fistName",
                Value = user.FirstName
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@userId",
                Value = user.UserId
            });

            await command.ExecuteNonQueryAsync();

            if (user is Student s)
            {
                await UpdateStudentData(command, s);
            }
            else if (user is FacultyMember fm)
            {
                await UpdateFacultyMemberData(command, fm);
            }
        }

        private async Task UpdateStudentData(MySqlCommand command, Student s)
        {
            command.CommandText = @"UPDATE student
                                    SET
                                    study_program_id = @programId,
                                    study_program_specialization_id = @specializationId,
                                    study_year = @studyYear
                                    WHERE user_id = @userId";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@userId",
                Value = s.UserId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@programId",
                Value = s.StudyProgramId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@specialiationId",
                Value = s.StudyProgramSpecializationId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@studyYear",
                Value = s.StudyYear
            });

            await command.ExecuteNonQueryAsync();
        }

        private async Task UpdateFacultyMemberData(MySqlCommand command, FacultyMember fm)
        {
            command.CommandText = @"UPDATE faculty_member
                                    SET
                                    faculty_id = @facultyId,
                                    study_field_id = @fieldId,
                                    academic_rank_id = @rankId  
                                    WHERE user_id = @userId";


            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@userId",
                Value = fm.UserId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@facultyId",
                Value = fm.FacultyId
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@fieldId",
                Value = StudyField.fields.Where(f => f.Value.Name == fm.StudyField.Name).First().Key
            });

            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@rankId",
                Value = (int)fm.AcademicRank
            });

            await command.ExecuteNonQueryAsync();
        }
    }
}
