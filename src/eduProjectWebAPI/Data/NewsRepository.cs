using eduProjectModel.Domain;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly DbConnectionParameters dbConnectionParameters;

        public NewsRepository(DbConnectionParameters dbConnectionParameters) => this.dbConnectionParameters = dbConnectionParameters;

        public async Task AddAsync(News news)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    CommandText = @"INSERT INTO news
                                    (title, content, date)
                                    VALUES
                                    (@title, @content, @date)",
                    Connection = connection
                };

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@title",
                    DbType = DbType.String,
                    Value = news.Title
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@content",
                    DbType = DbType.String,
                    Value = news.Content
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@date",
                    DbType = DbType.DateTime,
                    Value = news.Date
                });

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }

        public async Task DeleteAsync(News news)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    CommandText = @"DELETE FROM news
                                    WHERE news_id = @id",
                    Connection = connection
                };

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = news.Id
                });

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }

        public async Task<ICollection<News>> GetAllAsync()
        {
            var news = new List<News>();

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    CommandText = @"SELECT news_id, title, content, date
                                    FROM news",
                    Connection = connection
                };

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var n = GetNewsFromRow(reader);
                            news.Add(n);
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return news;
        }

        public async Task<News> GetByIdAsync(int id)
        {
            News news = null;

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    CommandText = @"SELECT news_id, title, content, date
                                    FROM news
                                    WHERE news_id = @id",
                    Connection = connection
                };

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
                        news = GetNewsFromRow(reader);
                    }
                }

                await connection.CloseAsync();

                return news;
            }
        }

        private News GetNewsFromRow(MySqlDataReader reader)
        {
            return new News
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Content = reader.GetString(2),
                Date = reader.GetDateTime(3)
            };
        }
    }
}

