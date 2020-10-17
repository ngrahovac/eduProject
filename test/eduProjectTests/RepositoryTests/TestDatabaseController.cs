using eduProjectWebAPI.Data;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eduProjectTests.RepositoryTests
{
    class TestDatabaseController
    {
        private readonly TestDbConnectionString dbConnectionString;

        public TestDatabaseController(TestDbConnectionString dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public async Task ClearTables()
        {
            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "clear_tables"
                };

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }

        public async Task SeedData()
        {
            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = "seed_data"
                };

                await connection.OpenAsync();

                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }
    }
}
