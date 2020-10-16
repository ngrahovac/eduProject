using eduProjectModel.Domain;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class ProjectApplicationsRepository : IProjectApplicationsRepository
    {
        private readonly DbConnectionStringBase dbConnectionString;
        public ProjectApplicationsRepository(DbConnectionStringBase dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }
        public async Task AddAsync(ProjectApplication application)
        {
            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
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
    }
}
