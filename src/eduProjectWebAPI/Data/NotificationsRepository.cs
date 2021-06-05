using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly DbConnectionParameters dbConnectionParameters;

        public NotificationsRepository(DbConnectionParameters dbConnectionParameters)
        {
            this.dbConnectionParameters = dbConnectionParameters;
        }

        public async Task DeleteReceivedApplicationsNotification(int authorId)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"DELETE FROM notifs_author
                                    WHERE author_id = @authorId"
                };

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@authorId",
                    DbType = DbType.Int32,
                    Value = authorId
                });

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }

        public async Task DeleteSentApplicationsNotification(int userId)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"DELETE FROM notifs_user
                                    WHERE user_id = @userId"
                };

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@userId",
                    DbType = DbType.Int32,
                    Value = userId
                });

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }

        public async Task<ICollection<int>> GetReceivedApplicationsNotification(int authorId)
        {
            var ids = new List<int>();

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT application_id 
                                    FROM notifs_author
                                    WHERE author_id = @authorId"
                };

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@authorId",
                    DbType = DbType.Int32,
                    Value = authorId
                });

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            ids.Add(reader.GetInt32(0));
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return ids;
        }

        public async Task<ICollection<int>> GetSentApplicationsNotification(int userId)
        {
            var ids = new List<int>();

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT application_id 
                                    FROM notifs_user
                                    WHERE user_id = @userId"
                };

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@userId",
                    DbType = DbType.Int32,
                    Value = userId
                });

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            ids.Add(reader.GetInt32(0));
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return ids;
        }
    }
}
