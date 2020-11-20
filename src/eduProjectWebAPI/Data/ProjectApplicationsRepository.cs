using eduProjectModel.Domain;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class ProjectApplicationsRepository : IProjectApplicationsRepository
    {
        private readonly DbConnectionParameters dbConnectionParameters;

        public ProjectApplicationsRepository(DbConnectionParameters dbConnectionParameters)
        {
            this.dbConnectionParameters = dbConnectionParameters;
        }

        public async Task AddAsync(ProjectApplication application)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"INSERT INTO project_application
                                    (applicant_comment, project_application_status_id, collaborator_profile_id, user_id)
                                    VALUES
                                    (@applicantComment, @statusId, @profileId, @applicantId)"
                };

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@applicantComment",
                    DbType = DbType.String,
                    Value = application.ApplicantComment
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@statusId",
                    DbType = DbType.Int32,
                    Value = (int)application.ProjectApplicationStatus
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@profileId",
                    DbType = DbType.Int32,
                    Value = application.CollaboratorProfileId
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@applicantId",
                    DbType = DbType.Int32,
                    Value = application.ApplicantId
                });

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }

        public async Task<ProjectApplication> GetById(int id)
        {
            ProjectApplication application = null;

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT project_application_id, applicant_comment, author_comment,
                                           project_application_status_id, collaborator_profile_id, user_id,
                                           project_id
                                    FROM project_application
                                    INNER JOIN collaborator_profile USING(collaborator_profile_id)
                                    WHERE project_application_id = @id"
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
                        application = GetProjectApplicationFromRow(reader);
                    }
                }

                await connection.CloseAsync();
            }

            return application;
        }

        public async Task<ICollection<ProjectApplication>> GetByApplicantId(int applicantId)
        {
            ICollection<ProjectApplication> applications = new List<ProjectApplication>();

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT project_application_id, applicant_comment, author_comment,
                                           project_application_status_id, collaborator_profile_id, user_id,
                                           project_id
                                    FROM project_application                                    
                                    INNER JOIN collaborator_profile USING(collaborator_profile_id)
                                    WHERE user_id = @id"
                };

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = applicantId
                });

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            applications.Add(GetProjectApplicationFromRow(reader));
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return applications;
        }

        public async Task<ICollection<ProjectApplication>> GetByProjectId(int projectId)
        {
            ICollection<ProjectApplication> applications = new List<ProjectApplication>();

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT project_application_id, applicant_comment, author_comment,
                                           project_application_status_id, collaborator_profile_id, project_application.user_id,
                                           project.project_id
                                    FROM project
                                    INNER JOIN collaborator_profile USING(project_id)
                                    INNER JOIN project_application USING(collaborator_profile_id)
                                    WHERE project.project_id = @id"
                };

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = projectId
                });

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            applications.Add(GetProjectApplicationFromRow(reader));
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return applications;
        }

        private ProjectApplication GetProjectApplicationFromRow(MySqlDataReader reader)
        {
            return new ProjectApplication
            {
                ProjectApplicationId = reader.GetInt32(0),
                ApplicantComment = reader.IsDBNull(1) ? null : reader.GetString(1),
                AuthorComment = reader.IsDBNull(2) ? null : reader.GetString(2),
                ProjectApplicationStatus = (ProjectApplicationStatus)Enum.ToObject(typeof(ProjectApplicationStatus), reader.GetInt32(3)),
                CollaboratorProfileId = reader.GetInt32(4),
                ApplicantId = reader.GetInt32(5),
                ProjectId = reader.GetInt32(6)
            };
        }

        public async Task Update(ProjectApplication application)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"UPDATE project_application
                                    SET 
                                    author_comment = @authorComment,
                                    applicant_comment = @applicantComment,
                                    project_application_status_id = @statusId
                                    WHERE project_application_id = @applicationId"
                };

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "@authorComment",
                    Value = application.AuthorComment
                });

                command.Parameters.Add(new MySqlParameter
                {
                    DbType = DbType.String,
                    ParameterName = "@applicantComment",
                    Value = application.ApplicantComment
                });

                command.Parameters.Add(new MySqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "@statusId",
                    Value = (int)application.ProjectApplicationStatus
                });

                command.Parameters.Add(new MySqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "@applicationId",
                    Value = application.ProjectApplicationId
                });

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }

        public async Task Delete(ProjectApplication application)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"DELETE FROM project_application WHERE project_application_id = @id"
                };

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = application.ProjectApplicationId
                });

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }

    }
}
