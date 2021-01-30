using eduProjectModel;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using eduProjectWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/applications")]
    public class ProjectApplicationsController : ControllerBase
    {
        private readonly IProjectsRepository projects;
        private readonly IProjectApplicationsRepository applications;
        private readonly IUsersRepository users;
        private readonly IUserSettingsRepository settings;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectApplicationsController(IProjectsRepository projects, IProjectApplicationsRepository applications, IUsersRepository users, IUserSettingsRepository settings, UserManager<ApplicationUser> userManager)
        {
            this.projects = projects;
            this.applications = applications;
            this.users = users;
            this.settings = settings;
            this.userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDisplayModel>> GetById(int id)
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
                    var projectApplication = await applications.GetAsync(id);

                    if (projectApplication == null)
                        return NotFound();

                    int authorId = (await projects.GetAsync(projectApplication.ProjectId)).AuthorId;

                    if (projectApplication.ApplicantId == currentUserId || authorId == currentUserId)
                    {
                        var applicant = await users.GetAsync(projectApplication.ApplicantId);
                        var userSettings = await settings.GetAsync(projectApplication.ApplicantId);
                        var email = (await userManager.FindByIdAsync($"{applicant.UserId}")).Email;
                        return new ApplicationDisplayModel(projectApplication,
                            new Tuple<int, string, string>(applicant.UserId, $"{applicant.FirstName} {applicant.LastName}", email));
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(ProjectApplicationInputModel model)
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
                    int authorId = (await projects.GetAsync(model.ProjectId)).AuthorId;

                    if (model.ApplicantId == currentUserId || authorId == currentUserId)
                    {
                        var application = await applications.GetAsync(model.ApplicationId);
                        model.MapTo(application);
                        await applications.UpdateAsync(application);

                        return NoContent();
                    }
                    else
                    {
                        return Forbid();
                    }

                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<ProjectApplicationsDisplayModel>> GetByProject(int projectId)
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
                    var project = await projects.GetAsync(projectId);

                    if (project == null)
                        return NotFound();

                    if (project.AuthorId == currentUserId)
                    {
                        var projectApplications = await applications.GetByProjectIdAsync(projectId);

                        List<Tuple<int, string, string>> modelData = new List<Tuple<int, string, string>>();
                        foreach (var appl in projectApplications)
                        {
                            var applicant = await users.GetAsync(appl.ApplicantId);
                            var email = (await userManager.FindByIdAsync($"{applicant.UserId}")).Email;
                            modelData.Add(new Tuple<int, string, string>(appl.ApplicantId,
                                $"{applicant.FirstName} {applicant.LastName}", email));
                        }

                        return new ProjectApplicationsDisplayModel(project, projectApplications, modelData);
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.StackTrace);
                    //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<ICollection<ProjectApplicationsDisplayModel>>> GetByProjectAuthor(int authorId)
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

                    if (authorId == currentUserId)
                    {
                        var authoredProjects = await projects.GetAllByAuthorAsync(authorId);
                        var ids = authoredProjects.Select(p => p.ProjectId);

                        List<ProjectApplicationsDisplayModel> projectApplicationsDisplayModels = new List<ProjectApplicationsDisplayModel>();

                        foreach (var id in ids)
                        {
                            var project = authoredProjects.Where(p => p.ProjectId == id).First();
                            var projectApplications = await applications.GetByProjectIdAsync(id);

                            List<Tuple<int, string, string>> modelData = new List<Tuple<int, string, string>>();
                            foreach (var appl in projectApplications)
                            {
                                var applicant = await users.GetAsync(appl.ApplicantId);
                                var email = (await userManager.FindByIdAsync($"{applicant.UserId}")).Email;
                                modelData.Add(new Tuple<int, string, string>(appl.ApplicantId,
                                    $"{applicant.FirstName} {applicant.LastName}", "dada"));
                            }
                            projectApplicationsDisplayModels.Add(new ProjectApplicationsDisplayModel(project, projectApplications, modelData));
                        }

                        return projectApplicationsDisplayModels;
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpGet("applicant/{applicantId}")]
        public async Task<ActionResult<ICollection<ProjectApplicationsDisplayModel>>> GetByApplicantId(int applicantId)
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

                    if (applicantId == currentUserId)
                    {
                        ICollection<ProjectApplication> usersApplications = await applications.GetByApplicantIdAsync(applicantId);
                        List<ProjectApplicationsDisplayModel> models = new List<ProjectApplicationsDisplayModel>();

                        var projectIds = usersApplications.Select(a => a.ProjectId).Distinct();
                        foreach (var id in projectIds)
                        {
                            var project = await projects.GetAsync(id);

                            List<Tuple<int, string, string>> modelData = new List<Tuple<int, string, string>>();
                            foreach (var appl in usersApplications.Where(a => a.ProjectId == id))
                            {
                                var applicant = await users.GetAsync(appl.ApplicantId);
                                var email = (await settings.GetAsync(appl.ApplicantId)).Email;
                                modelData.Add(new Tuple<int, string, string>(appl.ApplicantId,
                                    $"{applicant.FirstName} {applicant.LastName}", email));
                            }


                            models.Add(new ProjectApplicationsDisplayModel(project, usersApplications.Where(a => a.ProjectId == id).ToList(), modelData));
                        }

                        return models;
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ProjectApplicationInputModel model)
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
                    var project = await projects.GetAsync(model.ProjectId);

                    if (project.AuthorId != currentUserId)
                    {
                        ProjectApplication application = new ProjectApplication();
                        model.ApplicantId = currentUserId;
                        model.MapTo(application);

                        await applications.AddAsync(application);
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception e)
                {
                    //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    return BadRequest(e.StackTrace);
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
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
                    var application = await applications.GetAsync(id);

                    if (application.ApplicantId == currentUserId)
                    {
                        await applications.DeleteAsync(application);
                        return Ok();
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
