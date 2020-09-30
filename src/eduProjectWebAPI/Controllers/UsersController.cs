using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private IProjectsRepository projects;
        private IUsersRepository users;
        private IFacultiesRepository faculties;

        public UsersController(IProjectsRepository projects, IUsersRepository users, IFacultiesRepository faculties)
        {
            this.projects = projects;
            this.users = users;
            this.faculties = faculties;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDisplayModel>> GetById(int id)
        {
            User user = await users.GetAsync(id);

            if (user == null)
                return NotFound();

            ICollection<Project> authoredProjects = null;
            ICollection<Project> projectCollaborations = null;

            if (user.AuthoredProjectIds != null)
            {
                authoredProjects = new HashSet<Project>();
                foreach (int projectId in user.AuthoredProjectIds)
                    authoredProjects.Add(await projects.GetAsync(projectId));
            }

            if (user.ProjectCollaborationIds != null)
            {
                projectCollaborations = new HashSet<Project>();
                foreach (int projectId in user.ProjectCollaborationIds)
                    projectCollaborations.Add(await projects.GetAsync(projectId));
            }

            return new ProfileDisplayModel(user, true, authoredProjects, projectCollaborations);
        }
    }
}
