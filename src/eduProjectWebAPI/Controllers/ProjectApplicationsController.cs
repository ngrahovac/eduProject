using eduProjectModel;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/applications")]
    public class ProjectApplicationsController : ControllerBase
    {
        private IProjectsRepository projects;
        private IProjectApplicationsRepository applications;

        public ProjectApplicationsController(IProjectsRepository projects, IProjectApplicationsRepository applications)
        {
            this.projects = projects;
            this.applications = applications;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDisplayModel>> GetById(int id)
        {
            try
            {
                var projectApplication = await applications.GetAsync(id);

                return new ApplicationDisplayModel(projectApplication);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(ProjectApplicationInputModel model)
        {
            try
            {
                var application = await applications.GetAsync(model.ApplicationId);
                model.MapTo(application);
                await applications.UpdateAsync(application);

                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // FIX: privremene rute
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<ProjectApplicationsDisplayModel>> GetByProject(int projectId)
        {
            try
            {
                var project = await projects.GetAsync(projectId);
                var projectApplications = await applications.GetByProjectIdAsync(projectId);

                return new ProjectApplicationsDisplayModel(project, projectApplications);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<ICollection<ProjectApplicationsDisplayModel>>> GetByProjectAuthor(int authorId)
        {
            try
            {
                var authoredProjects = await projects.GetAllByAuthorAsync(authorId);
                var ids = authoredProjects.Select(p => p.ProjectId);

                List<ProjectApplicationsDisplayModel> projectApplicationsDisplayModels = new List<ProjectApplicationsDisplayModel>();

                foreach (var id in ids)
                {
                    var project = authoredProjects.Where(p => p.ProjectId == id).First();
                    var projectApplications = await applications.GetByProjectIdAsync(id);
                    projectApplicationsDisplayModels.Add(new ProjectApplicationsDisplayModel(project, projectApplications));
                }

                return projectApplicationsDisplayModels;

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("applicant/{applicantId}")]
        public async Task<ActionResult<ICollection<ProjectApplicationsDisplayModel>>> GetByApplicantId(int applicantId)
        {
            try
            {
                ICollection<ProjectApplication> usersApplications = await applications.GetByApplicantIdAsync(applicantId);
                List<ProjectApplicationsDisplayModel> models = new List<ProjectApplicationsDisplayModel>();

                var projectIds = usersApplications.Select(a => a.ProjectId).Distinct();
                foreach (var id in projectIds)
                {
                    var project = await projects.GetAsync(id);
                    models.Add(new ProjectApplicationsDisplayModel(project, usersApplications.Where(a => a.ProjectId == id).ToList()));
                }


                return models;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ProjectApplicationInputModel model)
        {
            ProjectApplication application = new ProjectApplication();
            model.MapTo(application);

            try
            {
                await applications.AddAsync(application);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var application = await applications.GetAsync(id);
                await applications.DeleteAsync(application);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
