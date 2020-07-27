using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/projects")]
    public class ProjectsController : Controller
    {
        private IProjectsRepository projects;
        public ProjectsController(IProjectsRepository projects)
        {
            this.projects = projects;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDisplayModel>> GetById(int id)
        {
            Project project = await projects.GetAsync(id);
            // dohvati autora
            // napravi display model
            var model = ProjectDisplayModel.FromProject(project, null);
            return model;
        }
    }
}
