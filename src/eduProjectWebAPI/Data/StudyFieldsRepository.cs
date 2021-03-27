using eduProjectModel.Domain;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class StudyFieldsRepository : IStudyFieldsRepository
    {
        private readonly DbConnectionParameters dbConnectionParameters;
        public StudyFieldsRepository(DbConnectionParameters dbConnectionParameters)
        {
            this.dbConnectionParameters = dbConnectionParameters;
        }
        public async Task AddAsync(StudyField field)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                var command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"INSERT INTO study_field
                                    (name, description)
                                    VALUES
                                    (@name, @description)"
                };

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@name",
                    DbType = DbType.String,
                    Value = field.Name
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@description",
                    DbType = DbType.String,
                    Value = field.Description
                });

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                await connection.CloseAsync();
            }
        }
    }
}
