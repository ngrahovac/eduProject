using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using eduProjectWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly IProjectsRepository projects;
        private readonly IUsersRepository users;
        private readonly IFacultiesRepository faculties;

        public UsersController(IProjectsRepository projects, IUsersRepository users, IFacultiesRepository faculties)
        {
            this.projects = projects;
            this.users = users;
            this.faculties = faculties;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDisplayModel>> GetById(int id)
        {
            if (HttpContext.Request.ExtractUserId() == null)
            {
                return Unauthorized();
            }
            else
            {
                try
                {
                    int currentUserId = (int)HttpContext.Request.ExtractUserId();

                    User user = await users.GetAsync(id);

                    if (user == null)
                        return NotFound();

                    var authoredProjects = new List<Project>();
                    var projectCollaborations = new List<Project>();

                    if (user.AuthoredProjectIds != null)
                    {
                        foreach (int projectId in user.AuthoredProjectIds)
                            authoredProjects.Add(await projects.GetAsync(projectId));
                    }

                    if (user.ProjectCollaborationIds != null)
                    {
                        foreach (int projectId in user.ProjectCollaborationIds)
                            projectCollaborations.Add(await projects.GetAsync(projectId));
                    }

                    var faculty = await faculties.GetAsync(user.FacultyId);
                    bool isPersonal = currentUserId == id;

                    return new ProfileDisplayModel(user, isPersonal, faculty, authoredProjects, projectCollaborations);
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
