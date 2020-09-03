using eduProjectModel.Domain;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class UsersRepository
    {
        private readonly IConfiguration configuration;
        public UsersRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<User> GetAsync(int id)
        {
            string commandText = @"SELECT user_id, user_account_type_id, first_name, last_name, phone_number, phone_format,
	                                            student.study_year,
	                                            faculty.name, faculty.address, study_program.name, study_program.cycle, study_program.duration_years,
	                                            study_program_specialization.name,
                                                academic_rank.name,
                                                study_field.name
                                      FROM user
                                      INNER JOIN account using(user_id)
                                      LEFT OUTER JOIN student using(user_id)
                                      LEFT OUTER JOIN faculty_member using (user_id)
                                      LEFT OUTER JOIN study_program using(study_program_id)
                                      LEFT OUTER JOIN study_program_specialization using(study_program_specialization_id)
                                      LEFT OUTER JOIN faculty ON faculty_member.faculty_id = faculty.faculty_id  OR study_program.faculty_id = faculty.faculty_id
                                      LEFT OUTER JOIN academic_rank using(academic_rank_id)
                                      LEFT OUTER JOIN study_field using(study_field_id)
                                      WHERE user.user_id = @id;";
            MySqlCommand command = new MySqlCommand
            {
                CommandText = commandText
            };
            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@id",
                Value = id
            });
            User user = null;
            using (var connection = new MySqlConnection(configuration["ConnectionStrings:eduProjectDb"]))
            {
                await connection.OpenAsync();
                command.Connection = connection;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user = GetUserFromRow(reader);
                    }
                }
                await connection.CloseAsync();
            }
            return user;
        }
        private User GetUserFromRow(MySqlDataReader reader)
        {
            User user = new User();
            UserAccountType accountType = (UserAccountType)Enum.ToObject(typeof(UserAccountType), reader.GetInt32(1));
            if (accountType is UserAccountType.Student)
            {
                Student student = new Student();
                student.StudyYear = reader.GetInt32(6);
                student.StudyProgram = new StudyProgram
                {
                    Name = reader.GetString(9),
                    Cycle = reader.GetByte(10),
                    DurationYears = reader.GetByte(11)
                };
                student.StudyProgramSpecialization = new StudyProgramSpecialization
                {
                    Name = reader.GetString(12)
                };
                user = student;
            }
            else if (accountType is UserAccountType.FacultyMember)
            {
                FacultyMember facultyMember = new FacultyMember();
                facultyMember.Faculty = new Faculty
                {
                    Name = reader.GetString(7),
                    Address = reader.GetString(8)
                };
                facultyMember.AcademicRank = (AcademicRank)Enum.ToObject(typeof(AcademicRank), reader.GetInt32(13));
                facultyMember.StudyField = new StudyField
                {
                    Name = reader.GetString(14)
                };
                user = facultyMember;
            }
            user.UserId = reader.GetInt32(0);
            user.FirstName = reader.GetString(2);
            user.LastName = reader.GetString(3);
            user.PhoneNumber = !reader.IsDBNull(4) ? reader.GetString(4) : null;
            user.PhoneFormat = !reader.IsDBNull(4) ? reader.GetString(5) : null;
            return user;
        }
    }
}
