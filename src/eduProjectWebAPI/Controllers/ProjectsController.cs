using eduProjectWebAPI.Data.DAO;
using eduProjectWebAPI.Model.DisplayModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/projects")]
    public class ProjectsController : Controller
    {
        private IProjectDAO projectDAO;
        public ProjectsController(IProjectDAO projectDAO)
        {
            this.projectDAO = projectDAO;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDisplayModel>> GetById(int id)
        {
            var project = await projectDAO.Read(id);

            if (project == null)
            {
                return NotFound();
            }

            return ProjectDisplayModel.FromProject(project);
        }
    }
}
