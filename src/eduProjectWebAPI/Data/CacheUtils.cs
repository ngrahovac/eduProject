using eduProjectModel.Domain;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.Extensions.Caching.Memory;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public static class CacheUtils
    {
        public static async Task<Faculty> CacheFaculty(int id, DbConnectionStringBase dbConnectionString, IMemoryCache cache)
        {
            Faculty faculty = null;
            StudyProgram studyProgram = null;
            StudyProgramSpecialization studyProgramSpecialization = null;

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT faculty.faculty_id, faculty.name, address, 
                                           study_program.study_program_id, cycle, duration_years, study_program.name,
                                           study_program_specialization_id, study_program_specialization.name
                                    FROM faculty 
                                    LEFT OUTER JOIN study_program USING(faculty_id) 
                                    LEFT OUTER JOIN study_program_specialization USING(study_program_id)
                                    WHERE faculty_id = @id"
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
                            int facultyId = reader.GetInt32(0);
                            int programId = reader.GetInt32(3);
                            int? specializationId = !(reader.IsDBNull(7)) ? (int?)reader.GetInt32(7) : null;

                            string key;

                            if (!cache.TryGetValue($"{CacheKeyTemplate.Faculty}{id}", out faculty))
                            {
                                // faculty does not exists in cache
                                // add faculty to cache
                                faculty = GetFacultyFromRow(reader);
                                key = $"{CacheKeyTemplate.Faculty}{facultyId}";
                                cache.CreateEntry(key);
                                cache.Set(key, faculty);

                                // add study program to cache as separate entry
                                studyProgram = GetStudyProgramFromRow(reader);
                                faculty.AddStudyProgram(studyProgram);
                                key = $"{CacheKeyTemplate.Program}{programId}";
                                cache.CreateEntry(key);
                                cache.Set(key, studyProgram);

                                // add study program to cache as separate entry, if exists
                                if (specializationId != null)
                                {
                                    studyProgramSpecialization = GetStudyProgramSpecializationFromRow(reader);
                                    studyProgram.AddSpecialization(studyProgramSpecialization);
                                    key = $"{CacheKeyTemplate.Specialization}{specializationId}";
                                    cache.CreateEntry(key);
                                    cache.Set(key, studyProgramSpecialization);
                                }
                            }
                            else
                            {
                                // faculty exists in cache (e.g. added in previous row); checking for program and specialization
                                if (!cache.TryGetValue($"{CacheKeyTemplate.Program}{programId}", out studyProgram))
                                {
                                    studyProgram = GetStudyProgramFromRow(reader);
                                    faculty.AddStudyProgram(studyProgram);
                                    key = $"{CacheKeyTemplate.Program}{programId}";
                                    cache.CreateEntry(key);
                                    cache.Set(key, studyProgram);

                                    if (specializationId != null)
                                    {
                                        studyProgramSpecialization = GetStudyProgramSpecializationFromRow(reader);
                                        studyProgram.AddSpecialization(studyProgramSpecialization);
                                        key = $"{CacheKeyTemplate.Specialization}{specializationId}";
                                        cache.CreateEntry(key);
                                        cache.Set(key, studyProgramSpecialization);
                                    }
                                }
                                else
                                {
                                    // study program is already added, so this row must define another specialization
                                    studyProgramSpecialization = GetStudyProgramSpecializationFromRow(reader);
                                    studyProgram.AddSpecialization(studyProgramSpecialization);
                                    key = $"{CacheKeyTemplate.Specialization}{specializationId}";
                                    cache.CreateEntry(key);
                                    cache.Set(key, studyProgramSpecialization);
                                }
                            }
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return faculty;
        }

        public static async Task<StudyProgram> CacheStudyProgram(int id, DbConnectionStringBase dbConnectionString, IMemoryCache cache)
        {
            int facultyId = 0;
            Faculty faculty = null;
            string studyProgramName = null;

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT faculty.faculty_id, study_program.name
                                    FROM faculty 
                                    LEFT OUTER JOIN study_program USING(faculty_id) 
                                    LEFT OUTER JOIN study_program_specialization USING(study_program_id)
                                    WHERE study_program.study_program_id = @id"
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
                        await reader.ReadAsync();
                        facultyId = reader.GetInt32(0);
                        studyProgramName = reader.GetString(1);
                    }
                }

                await connection.CloseAsync();
            }

            faculty = await CacheFaculty(facultyId, dbConnectionString, cache);

            // FIX: assumes study program names are unique
            return faculty.StudyPrograms.Where(sp => sp.Name == studyProgramName).First();
        }

        public static async Task<StudyProgramSpecialization> CacheProgramSpecialization(int id, DbConnectionStringBase dbConnectionString, IMemoryCache cache)
        {
            int facultyId = 0;
            Faculty faculty = null;
            StudyProgramSpecialization programSpecialization = null;
            string studyProgramSpecializationName = null;

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT faculty.faculty_id, study_program_specialization.name
                                    FROM faculty 
                                    LEFT OUTER JOIN study_program USING(faculty_id) 
                                    LEFT OUTER JOIN study_program_specialization USING(study_program_id)
                                    WHERE study_program_specialization.study_program_specialiation_id = @id"
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
                        await reader.ReadAsync();
                        facultyId = reader.GetInt32(0);
                        studyProgramSpecializationName = reader.GetString(1);
                    }
                }

                await connection.CloseAsync();
            }

            faculty = await CacheFaculty(facultyId, dbConnectionString, cache);

            // FIX: assumes study program specialization names are unique
            foreach (var p in faculty.StudyPrograms)
            {
                foreach (var s in p.StudyProgramSpecializations)
                {
                    if (s.Name == studyProgramSpecializationName)
                    {
                        programSpecialization = s;
                        break;
                    }
                }
            }

            return programSpecialization;
        }

        private static Faculty GetFacultyFromRow(MySqlDataReader reader)
        {
            return new Faculty
            {
                Name = reader.GetString(1),
                Address = reader.GetString(2)
            };
        }

        private static StudyProgram GetStudyProgramFromRow(MySqlDataReader reader)
        {
            return new StudyProgram
            {
                Cycle = (byte)reader.GetInt32(4),
                DurationYears = (byte)reader.GetInt32(5),
                Name = reader.GetString(6)
            };
        }

        private static StudyProgramSpecialization GetStudyProgramSpecializationFromRow(MySqlDataReader reader)
        {
            return new StudyProgramSpecialization
            {
                Name = reader.GetString(8)
            };
        }

        public static async Task<StudyField> CacheStudyField(int id, DbConnectionStringBase dbConnectionString, IMemoryCache cache)
        {
            StudyField studyField = null;

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT name, description 
                                    FROM study_field   
                                    WHERE study_field_id = @id "
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
                        await reader.ReadAsync();

                        studyField = new StudyField
                        {
                            Name = reader.GetString(0),
                            Description = !reader.IsDBNull(1) ? reader.GetString(1) : null
                        };

                        var key = $"{CacheKeyTemplate.Field}{id}";
                        cache.CreateEntry(key);
                        cache.Set(key, studyField);
                    }
                }

                await connection.CloseAsync();
            }

            return studyField;
        }

        public static async Task<Tag> CacheTags(int id, DbConnectionStringBase dbConnectionString, IMemoryCache cache)
        {
            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                Tag tag = null;

                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT tag_id, name, description 
                                    FROM tag
                                    WHERE tag_id = @id"
                };

                command.Parameters.Clear();
                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = id
                });

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();

                        tag = new Tag
                        {
                            Name = reader.GetString(1),
                            Description = !reader.IsDBNull(2) ? reader.GetString(1) : null
                        };

                        string key = $"{CacheKeyTemplate.Tag}{reader.GetInt32(0)}";
                        cache.CreateEntry(key);
                        cache.Set(key, tag);
                    }
                }

                await connection.CloseAsync();

                return tag;
            }
        }
    }
}
