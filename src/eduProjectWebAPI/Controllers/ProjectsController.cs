using eduProjectWebAPI.Data;
using eduProjectWebAPI.Model;
using eduProjectWebAPI.Model.Display;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var model = ProjectDisplayModel.FromProject(project, null);
            return model;
        }
    }
}
