using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    public class ValueObjectsController : ControllerBase
    {
        private readonly DbConnectionParameters dbConnectionString;
        private readonly IFacultiesRepository faculties;

        public ValueObjectsController(DbConnectionParameters dbConnectionString, IFacultiesRepository faculties)
        {
            this.dbConnectionString = dbConnectionString;
            this.faculties = faculties;
        }

        [HttpGet("/tags")]
        public async Task<ActionResult<Dictionary<string, Tag>>> GetTags()
        {
            Dictionary<string, Tag> tags = new Dictionary<string, Tag>();

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
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
                            tags.Add($"{reader.GetInt32(0)}", new Tag
                            {
                                Name = reader.GetString(1),
                                Description = !reader.IsDBNull(2) ? reader.GetString(2) : null
                            });
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return tags;
        }

        [HttpGet("/fields")]
        public async Task<ActionResult<Dictionary<string, StudyField>>> GetStudyFields()
        {
            Dictionary<string, StudyField> studyFields = new Dictionary<string, StudyField>();

            using (var connection = new MySqlConnection(dbConnectionString.ConnectionString))
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
                            studyFields.Add($"{reader.GetInt32(0)}", new StudyField
                            {
                                Name = reader.GetString(1),
                                Description = !reader.IsDBNull(2) ? reader.GetString(2) : null
                            });
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return studyFields;
        }


        [HttpGet("/faculties")]
        public async Task<ActionResult<ICollection<Faculty>>> GetFaculties()
        {
            return (await faculties.GetAllAsync()).ToList(); // why?
        }
    }
}
