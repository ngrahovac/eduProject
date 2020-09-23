using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/projects")]

    public class ProjectsController : ControllerBase
    {
        private IProjectsRepository projects;
        private IUsersRepository users;

        public ProjectsController(IProjectsRepository projects, IUsersRepository users)
        {
            this.projects = projects;
            this.users = users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDisplayModel>> GetById(int id)
        {
            Project project = await projects.GetAsync(id);

            if (project == null)
                return NotFound();

            User author = await users.GetAsync(project.AuthorId);

            if (project.ProjectStatus == ProjectStatus.Active)
                return new ProjectDisplayModel(project, author, false, true);
            else
            {
                var models = await GetCollaboratorDisplayModels(project);
                return new ProjectDisplayModel(project, author, false, false, models);
            }
        }

        private async Task<ICollection<CollaboratorDisplayModel>> GetCollaboratorDisplayModels(Project project)
        {
            var collaboratorIds = project.CollaboratorIds;
            List<User> collaborators = new List<User>();
            foreach (int collabId in collaboratorIds)
                collaborators.Add(await users.GetAsync(collabId));

            ICollection<CollaboratorDisplayModel> models = new List<CollaboratorDisplayModel>();
            foreach (User applicant in collaborators)
                models.Add(new CollaboratorDisplayModel(applicant));

            return models;
        }
    }
}