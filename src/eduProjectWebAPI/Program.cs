using System.Threading.Tasks;
using eduProjectModel.Domain;
using eduProjectWebAPI;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace TimeTracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var service = (DbConnectionParameters)host.Services.GetService(typeof(DbConnectionParameters));
            var connString = service.ConnectionString;
            await InitializeTags(connString);
            await InitializeStudyFields(connString);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
     Host.CreateDefaultBuilder(args)
         .ConfigureWebHostDefaults(webBuilder =>
         {
             webBuilder.UseStartup<Startup>();
         });

        private static async Task InitializeTags(string dbConnectionString)
        {
            using (var connection = new MySqlConnection(dbConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT tag_id, name, description FROM tag"
                };

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            Tag.tags.Add(reader.GetInt32(0), new Tag
                            {
                                Name = reader.GetString(1),
                                Description = !reader.IsDBNull(2) ? reader.GetString(2) : null
                            });
                        }
                    }
                }

                await connection.CloseAsync();
            }
        }

        private static async Task InitializeStudyFields(string dbConnectionString)
        {
            using (var connection = new MySqlConnection(dbConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT study_field_id, name, description FROM study_field"
                };

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            StudyField.fields.Add(reader.GetInt32(0), new StudyField
                            {
                                Name = reader.GetString(1),
                                Description = !reader.IsDBNull(2) ? reader.GetString(2) : null
                            });
                        }
                    }
                }

                await connection.CloseAsync();
            }
        }
    }
}
