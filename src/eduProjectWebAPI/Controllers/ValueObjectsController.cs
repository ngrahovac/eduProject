using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using eduProjectWebAPI.Services;
using Microsoft.AspNetCore.Identity;
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
        private readonly IStudyFieldsRepository studyFields;
        private readonly UserManager<ApplicationUser> userManager;

        public ValueObjectsController(DbConnectionParameters dbConnectionString, IFacultiesRepository faculties, IStudyFieldsRepository studyFields, UserManager<ApplicationUser> userManager)
        {
            this.dbConnectionString = dbConnectionString;
            this.faculties = faculties;
            this.studyFields = studyFields;
            this.userManager = userManager;
        }

        [HttpGet("/tags")]
        public async Task<ActionResult<Dictionary<string, Tag>>> GetTags()
        {
            /*
            if (HttpContext.Request.ExtractUserId() == null)
            {
                return Unauthorized();
            }
            else
            {*/
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
            //}
        }

        [HttpGet("/fields")]
        public async Task<ActionResult<Dictionary<string, StudyField>>> GetStudyFields()
        {
            /*
            if (HttpContext.Request.ExtractUserId() == null)
            {
                return Unauthorized();
            }
            else
            {*/
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
            //}
        }


        [HttpGet("/faculties")]
        public async Task<ActionResult<ICollection<Faculty>>> GetFaculties()
        {/*
            if (HttpContext.Request.ExtractUserId() == null)
            {
                return Unauthorized();
            }
            else
            {*/
            return (await faculties.GetAllAsync()).ToList(); // used by blazor since it can't access repositories
                                                             // }
        }

        [HttpPost("/fields")]
        public async Task<ActionResult> AddStudyField(StudyFieldInputModel model)
        {
            //TODO: Add auth check
            if (await IsUserAdmin())
            {
                try
                {
                    StudyField newField = new StudyField();
                    model.MapTo(newField);

                    await studyFields.AddAsync(newField);

                    //TODO: Change to Created
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message + "\n" + e.StackTrace);
                }
            }
            else
                return Forbid();
        }

        [HttpPost("/faculties")]
        public async Task<ActionResult> AddFaculty(FacultyInputModel model)
        {
            //TODO: Add auth check
            if (await IsUserAdmin())
            {
                try
                {
                    Faculty newFaculty = new Faculty();
                    model.MapTo(newFaculty);

                    await faculties.AddAsync(newFaculty);

                    //TODO: Change to Created
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message + "\n" + e.StackTrace);
                }
            }
            else
                return Forbid();
        }

        private async Task<bool> IsUserAdmin()
        {
            int? currentUserId = HttpContext.Request.ExtractUserId();

            if (currentUserId != null)
            {
                var user = await userManager.FindByIdAsync(currentUserId.ToString());

                return await userManager.IsInRoleAsync(user, "Admin");
            }
            else
                return false; //Other alternative?
        }
    }
}
