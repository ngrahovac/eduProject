using eduProjectModel.Domain;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class FacultiesRepository : IFacultiesRepository
    {
        private readonly DbConnectionParameters dbConnectionString;

        public FacultiesRepository(DbConnectionParameters dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public async Task<Faculty> GetAsync(int id)
        {
            Faculty faculty = null;

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT faculty.faculty_id, faculty.name, address,
                                           study_program.study_program_id, study_program.name, cycle, duration_years,
                                           study_program_specialization_id, study_program_specialization.name
                                    FROM faculty
                                    LEFT OUTER JOIN study_program USING (faculty_id)
                                    LEFT OUTER JOIN study_program_specialization USING (study_program_id)
                                    WHERE faculty.faculty_id = @id"
                };

                command.Parameters.Clear();
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
                        while (await reader.ReadAsync())
                        {
                            if (faculty == null)
                            {
                                // first row
                                faculty = new Faculty
                                {
                                    FacultyId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Address = reader.GetString(2)
                                };
                            }

                            // faculty already created
                            // checking if study program exists or is new

                            int programId = reader.GetInt32(3);
                            var programs = faculty.StudyPrograms.Where(sp => sp.ProgramId == programId);

                            if (programs.Count() == 0)
                            {
                                // add new program and specialization if exists
                                StudyProgram program = new StudyProgram
                                {
                                    ProgramId = reader.GetInt32(3),
                                    Name = reader.GetString(4),
                                    Cycle = reader.GetByte(5),
                                    DurationYears = reader.GetByte(6)
                                };

                                if (!reader.IsDBNull(7))
                                {
                                    program.AddSpecialization(new StudyProgramSpecialization
                                    {
                                        SpecializationId = reader.GetInt32(7),
                                        Name = reader.GetString(8)
                                    });
                                }

                                faculty.AddStudyProgram(program);
                            }
                            else
                            {
                                // program already created
                                // add specialization

                                StudyProgram program = programs.First();
                                program.AddSpecialization(new StudyProgramSpecialization
                                {
                                    SpecializationId = reader.GetInt32(7),
                                    Name = reader.GetString(8)
                                });
                            }
                        }
                    }
                }

                await connection.CloseAsync();
            }

            // add faculty, its programs and specializations to cache
            if (faculty != null)
            {
                /*
                var key = $"{CacheKeyTemplate.Faculty}{faculty.FacultyId}";
                cache.CreateEntry(key);
                cache.Set(key, faculty);*/

                foreach (var p in faculty.StudyPrograms)
                {/*
                    key = $"{CacheKeyTemplate.Program}{p.ProgramId}";
                    cache.CreateEntry(key);
                    cache.Set(key, p);*/

                    foreach (var s in p.StudyProgramSpecializations)
                    {/*
                        key = $"{CacheKeyTemplate.Specialization}{s.SpecializationId}";
                        cache.CreateEntry(key);
                        cache.Set(key, s);*/
                    }
                }
            }

            return faculty;
        }

        // TODO: refactor
        public async Task<ICollection<Faculty>> GetAllAsync()
        {
            List<Faculty> faculties = new List<Faculty>();
            List<int> facultyIds = new List<int>();

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT faculty_id
                                    FROM faculty"
                };

                command.Parameters.Clear();

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            facultyIds.Add(reader.GetInt32(0));
                        }
                    }
                }

                await connection.CloseAsync();
            }

            foreach (int id in facultyIds)
                faculties.Add(await GetAsync(id));

            return faculties;
        }

        public async Task AddAsync(Faculty faculty)
        {
            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                var command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                await AddFaculty(command, faculty);

                await connection.CloseAsync();
            }
        }

        private async Task AddFaculty(MySqlCommand command, Faculty faculty)
        {
            command.CommandText = @"INSERT INTO faculty
                                    (name, address)
                                    VALUES
                                    (@name, @address)";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = faculty.Name
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@address",
                DbType = DbType.String,
                Value = faculty.Address
            });

            await command.ExecuteNonQueryAsync();
            faculty.FacultyId = (int)command.LastInsertedId;

            await AddStudyPrograms(command, faculty);
        }

        private async Task AddStudyPrograms(MySqlCommand command, Faculty faculty)
        {
            command.CommandText = @"INSERT INTO study_program
                                    (name, cycle, duration_years, faculty_id)
                                    VALUES
                                    (@name, @cycle, @years, @facultyId)";

            foreach (var program in faculty.StudyPrograms)
            {
                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@name",
                    DbType = DbType.String,
                    Value = program.Name
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@cycle",
                    DbType = DbType.Byte,
                    Value = program.Cycle
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@years",
                    DbType = DbType.Byte,
                    Value = program.DurationYears
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@facultyId",
                    DbType = DbType.Int32,
                    Value = faculty.FacultyId
                });

                await command.ExecuteNonQueryAsync();
                program.ProgramId = (int)command.LastInsertedId;
            }

            foreach (var program in faculty.StudyPrograms)
            {
                await AddStudyProgramSpecializations(command, program);
            }
        }

        private async Task AddStudyProgramSpecializations(MySqlCommand command, StudyProgram program)
        {
            command.CommandText = @"INSERT INTO study_program_specialization
                                    (name, study_program_id)
                                    VALUES
                                    (@name, @programId)";

            foreach (var specialization in program.StudyProgramSpecializations)
            {
                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@name",
                    DbType = DbType.String,
                    Value = specialization.Name
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@programId",
                    DbType = DbType.Int32,
                    Value = program.ProgramId
                });

                await command.ExecuteNonQueryAsync();
                specialization.SpecializationId = (int)command.LastInsertedId;
            }
        }
    }
}
