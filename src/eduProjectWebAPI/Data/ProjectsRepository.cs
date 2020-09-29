﻿using eduProjectModel.Domain;
using Microsoft.Extensions.Caching.Memory;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;


namespace eduProjectWebAPI.Data
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly IMemoryCache cache;
        private readonly DbConnectionStringBase dbConnectionString;

        public ProjectsRepository(DbConnectionStringBase dbConnectionString, IMemoryCache cache)
        {
            this.dbConnectionString = dbConnectionString;
            this.cache = cache;
        }

        public async Task<Project> GetAsync(int id)
        {
            Project project = null;

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                // read project attributes from table `project`
                project = await ReadBasicProjectInfo(command, id);

                if (project != null)
                {
                    // read collaborator profiles from  table `collaborator_profiles`
                    await ReadCollaboratorProfilesInfo(command, id, project);

                    // read tag ids from table `project_tag`
                    await ReadTagsInfo(command, id, project);

                    // read collaborator ids from table `project_collaborator`
                    await ReadCollaboratorIds(command, id, project);
                }

                await connection.CloseAsync();
            }

            return project;
        }

        private async Task<Project> ReadBasicProjectInfo(MySqlCommand command, int id)
        {
            Project project = new Project();

            command.CommandText = @"SELECT project_id, title, start_date, end_date, 
                                           project.description, project.study_field_id, 
                                           project_status_id, user_id
                                    FROM project
                                    WHERE project.project_id = @id";


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
                    while (await reader.ReadAsync())
                    {
                        project.ProjectId = reader.GetInt32(0);
                        project.Title = reader.GetString(1);
                        project.StartDate = !reader.IsDBNull(2) ? (DateTime?)reader.GetDateTime(2) : null;
                        project.EndDate = !reader.IsDBNull(3) ? (DateTime?)reader.GetDateTime(3) : null;
                        project.Description = reader.GetString(4);

                        int studyFieldId = reader.GetInt32(5);
                        project.StudyField = StudyField.fields[studyFieldId];

                        project.ProjectStatus = (ProjectStatus)Enum.ToObject(typeof(ProjectStatus), reader.GetInt32(6));
                        project.AuthorId = reader.GetInt32(7);
                    }
                }
                else
                    project = null;
            }

            return project;
        }

        private async Task ReadCollaboratorProfilesInfo(MySqlCommand command, int id, Project project)
        {
            command.CommandText = @"SELECT collaborator_profile_id, collaborator_profile.description, user_account_type_id, 
	                                       cycle, study_year, student_profile.faculty_id, study_program_id, study_program_specialization_id,
	                                       faculty_member_profile.faculty_id, study_field_id
                                           FROM collaborator_profile
                                           LEFT OUTER JOIN student_profile USING(collaborator_profile_id)
                                           LEFT OUTER JOIN faculty_member_profile USING(collaborator_profile_id)                                                           
                                           WHERE project_id = @id";

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
                    while (await reader.ReadAsync())
                    {
                        CollaboratorProfileType profileType = (CollaboratorProfileType)Enum.ToObject(typeof(CollaboratorProfileType), reader.GetInt32(2));

                        if (profileType is CollaboratorProfileType.Student)
                        {
                            StudentProfile profile = new StudentProfile
                            {
                                CollaboratorProfileId = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                StudyCycle = reader.GetInt32(3),
                                StudyYear = reader.GetInt32(4),
                                FacultyId = !reader.IsDBNull(5) ? (int?)reader.GetInt32(5) : null,
                                StudyProgramId = !reader.IsDBNull(6) ? (int?)reader.GetInt32(6) : null,
                                StudyProgramSpecializationId = !reader.IsDBNull(7) ? (int?)reader.GetInt32(7) : null
                            };

                            project.CollaboratorProfiles.Add(profile);
                        }

                        else if (profileType is CollaboratorProfileType.FacultyMember)
                        {
                            FacultyMemberProfile profile = new FacultyMemberProfile
                            {
                                CollaboratorProfileId = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                FacultyId = !reader.IsDBNull(8) ? (int?)reader.GetInt32(8) : null,
                                StudyField = !reader.IsDBNull(9) ? StudyField.fields[reader.GetInt32(9)] : null
                            };

                            project.CollaboratorProfiles.Add(profile);
                        }
                    }
                }
            }
        }

        private async Task ReadTagsInfo(MySqlCommand command, int id, Project project)
        {
            command.CommandText = @"SELECT tag_id
                                    FROM project_tag
                                    WHERE project_id = @id";

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
                    while (await reader.ReadAsync())
                    {
                        int tagId = reader.GetInt32(0);
                        project.Tags.Add(Tag.tags[tagId]);
                    }
                }
            }
        }

        private async Task ReadCollaboratorIds(MySqlCommand command, int id, Project project)
        {
            command.CommandText = @"SELECT user_id
                                    FROM project_collaborator
                                    WHERE project_collaborator.project_id = @id";

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
                    int row = 0;
                    while (await reader.ReadAsync())
                    {
                        project.CollaboratorIds.Add(reader.GetInt32(row++));
                    }
                }
            }
        }
    }
}
