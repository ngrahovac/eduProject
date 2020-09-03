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
    public class ProjectsController : Controller
    {
        private IProjectsRepository projects;
        private UsersRepository users;

        public ProjectsController(IProjectsRepository projects, UsersRepository users)
        {
            this.projects = projects;
            this.users = users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDisplayModel>> GetById(int id)
        {
            Project project = await projects.GetAsync(id);
            User author = await users.GetAsync(project.AuthorId);
            return await ChooseDisplayModelAsync(project, author, false);
        }

        private async Task<ProjectDisplayModel> ChooseDisplayModelAsync(Project project, User user, bool visitor)
        {
            if (project.ProjectStatus == ProjectStatus.Active)
            {
                if (visitor)
                    return new VisitorActiveProjectDisplayModel(project, user);
                else
                    return new AuthorActiveProjectDisplayModel(project, user);
            }
            else
            {
                if (visitor)
                {
                    var models = await GetCollaboratorDisplayModels(project);
                    return new VisitorClosedProjectDisplayModel(project, user, models);
                }
                else
                {
                    var models = await GetCollaboratorDisplayModels(project);
                    return new AuthorClosedProjectDisplayModel(project, user, models);
                }
            }
        }

        private async Task<ICollection<CollaboratorDisplayModel>> GetCollaboratorDisplayModels(Project project)
        {
            var collaboratorIds = project.CollaboratorIds;
            HashSet<User> collaborators = new HashSet<User>();
            foreach (int collabId in collaboratorIds)
                collaborators.Add(await users.GetAsync(collabId));

            ICollection<CollaboratorDisplayModel> models = new HashSet<CollaboratorDisplayModel>();
            foreach (User applicant in collaborators)
                models.Add(new CollaboratorDisplayModel(applicant));
            return models;
        }
    }
}