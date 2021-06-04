using eduProjectModel;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using eduProjectWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/applications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectApplicationsController : ControllerBase
    {
        private readonly IProjectsRepository projects;
        private readonly IProjectApplicationsRepository applications;
        private readonly IUsersRepository users;
        private readonly IUserSettingsRepository settings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProjectApplicationsController(IProjectsRepository projects, IProjectApplicationsRepository applications,
            IUsersRepository users, IUserSettingsRepository settings, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.projects = projects;
            this.applications = applications;
            this.users = users;
            this.settings = settings;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDisplayModel>> GetById(int id)
        {
            try
            {
                int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
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
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(ProjectApplicationInputModel model)
        {
            try
            {
                int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
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
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<ProjectApplicationsDisplayModel>> GetByProject(int projectId)
        {
            try
            {
                int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var project = await projects.GetAsync(projectId);

                if (project == null)
                    return NotFound();

                if (project.AuthorId == currentUserId)
                {
                    var projectApplications = await applications.GetByProjectIdAsync(projectId);

                    //TODO: Refactor

                    var activeAuthorProjectApplications = new List<ProjectApplication>();

                    foreach (var application in projectApplications)
                    {
                        var user = await userManager.FindByIdAsync(application.ApplicantId.ToString());

                        if (user.ActiveStatus)
                            activeAuthorProjectApplications.Add(application);
                    }

                    projectApplications = activeAuthorProjectApplications;

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
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpGet("author/{id:int}")]
        public async Task<ActionResult<ICollection<ApplicationDisplayModel>>> GetByAuthorId(int id)
        {
            if (int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) == id)
            {
                var appls = await applications.GetByAuthorIdAsync(id);
                var models = new List<ApplicationDisplayModel>();

                foreach (var a in appls)
                {
                    var applicant = await users.GetAsync(a.ApplicantId);
                    var applicantAccount = await userManager.FindByIdAsync(applicant.UserId.ToString());
                    var project = await projects.GetAsync(a.ProjectId);
                    models.Add(new ApplicationDisplayModel(a, $"{applicant.FirstName} {applicant.LastName}", applicantAccount.Email, project));
                }

                return models.ToList();
            }
            else
                return Forbid();
        }

        [HttpGet("applicant/{applicantId}")]
        public async Task<ActionResult<ICollection<ProjectApplicationsDisplayModel>>> GetByApplicantId(int applicantId)
        {
            try
            {
                //int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)); - throws exception here: value cannot be null
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
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ProjectApplicationInputModel model)
        {
            try
            {
                int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var project = await projects.GetAsync(model.ProjectId);
                var profile = project.CollaboratorProfiles.Where(p => p.CollaboratorProfileId == model.CollaboratorProfileId).First();

                var projectApplications = (await GetByApplicantId(currentUserId)).Value;

                // collaborator profile ids the user already applied to
                var collaboratorProfileIds = new List<int>();
                foreach (var p in projectApplications)
                    foreach (var c in p.CollaboratorProfileApplicationsDisplayModels)
                        collaboratorProfileIds.Add(c.CollaboratorProfileDisplayModel.CollaboratorProfileId);

                // user can apply if they aren't project author and if they didn't apply already
                // and the profile is open for applying
                if (project.AuthorId != currentUserId &&
                    !collaboratorProfileIds.Contains(model.CollaboratorProfileId) &&
                    profile.ApplicationsOpen)
                {
                    ProjectApplication application = new ProjectApplication();
                    model.ApplicantId = currentUserId;
                    model.MapTo(application);

                    await applications.AddAsync(application);
                    return Created(httpContextAccessor.HttpContext.GetEndpoint().DisplayName, application);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var application = await applications.GetAsync(id);

                if (application.ApplicantId == currentUserId)
                {
                    await applications.DeleteAsync(application);
                    return NoContent();
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
