using eduProjectModel.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IMemoryCache cache;
        private readonly DbConnectionStringBase dbConnectionString;

        public UsersRepository(DbConnectionStringBase dbConnectionString, IMemoryCache cache)
        {
            this.cache = cache;
            this.dbConnectionString = dbConnectionString;
        }
        public async Task<User> GetAsync(int id)
        {
            User user = null;

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT user_id, user_account_type_id, first_name, last_name, phone_number, phone_format,
	                                       study_year, study_program_id, study_program_specialization_id
	                                       faculty_id, study_field_id, academic_rank_id
                                    FROM user
                                    LEFT OUTER JOIN student using(user_id)
                                    LEFT OUTER JOIN faculty_member using (user_id)
                                    WHERE user.user_id = @id;"
                };

                command.Parameters.Add(new MySqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "@id",
                    Value = id
                });

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        user = await GetUserFromRow(reader);
                    }
                }

                await connection.CloseAsync();
            }

            return user;
        }

        private async Task<User> GetUserFromRow(MySqlDataReader reader)
        {
            User user = new User();

            UserAccountType accountType = (UserAccountType)Enum.ToObject(typeof(UserAccountType), reader.GetInt32(1));

            if (accountType is UserAccountType.Student)
            {
                Student student = new Student();

                student.StudyYear = reader.GetInt32(6);
                int studyProgramId = reader.GetInt32(7);
                int? studyProgramSpecializationId = !reader.IsDBNull(8) ? (int?)reader.GetInt32(8) : null;

                StudyProgram studyProgram;
                StudyProgramSpecialization studyProgramSpecialization;

                if (cache.TryGetValue($"{CacheKeyTemplate.Program}{studyProgramId}", out studyProgram))
                {
                    student.StudyProgram = studyProgram;
                }
                else
                {
                    student.StudyProgram = await CacheUtils.CacheStudyProgram(studyProgramId, dbConnectionString, cache);
                }

                if (cache.TryGetValue($"{CacheKeyTemplate.Specialization}{studyProgramSpecializationId}", out studyProgramSpecialization))
                {
                    student.StudyProgramSpecialization = studyProgramSpecialization;
                }
                else
                {
                    student.StudyProgramSpecialization = await CacheUtils.CacheProgramSpecialization((int)studyProgramSpecializationId, dbConnectionString, cache);
                }

                user = student;
            }

            else if (accountType is UserAccountType.FacultyMember)
            {
                FacultyMember facultyMember = new FacultyMember();
                facultyMember.AcademicRank = (AcademicRank)Enum.ToObject(typeof(AcademicRank), reader.GetInt32(11));

                int facultyId = reader.GetInt32(9);
                int studyFieldId = reader.GetInt32(10);

                Faculty faculty;
                StudyField studyField;

                if (cache.TryGetValue($"{CacheKeyTemplate.Faculty}{reader.GetInt32(9)}", out faculty))
                {
                    facultyMember.Faculty = faculty;
                }
                else
                {
                    facultyMember.Faculty = await CacheUtils.CacheFaculty(facultyId, dbConnectionString, cache);
                }

                if (cache.TryGetValue($"{CacheKeyTemplate.Field}{reader.GetInt32(10)}", out studyField))
                {
                    facultyMember.StudyField = studyField;
                }
                else
                {
                    facultyMember.StudyField = await CacheUtils.CacheStudyField(studyFieldId, dbConnectionString, cache);
                }

                user = facultyMember;
            }

            user.UserId = reader.GetInt32(0);
            user.FirstName = reader.GetString(2);
            user.LastName = reader.GetString(3);
            user.PhoneNumber = !reader.IsDBNull(4) ? reader.GetString(4) : null;
            user.PhoneFormat = !reader.IsDBNull(5) ? reader.GetString(5) : null;

            return user;
        }
    }
}
