using eduProjectModel;
using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public async Task<ActionResult> CreateAsync(ProjectApplicationInputModel model)
        {
            ProjectApplication application = new ProjectApplication();
            model.MapTo(application);

            Project project = await projects.GetAsync(model.ProjectId);
            if (model.CollaboratorProfileType == CollaboratorProfileType.Student)
            {
                CollaboratorProfile profile = project.CollaboratorProfiles.Where(p => p is StudentProfile).ElementAt(model.CollaboratorProfileIndex);
                application.CollaboratorProfileId = profile.CollaboratorProfileId;
            }
            else if (model.CollaboratorProfileType == CollaboratorProfileType.FacultyMember)
            {
                CollaboratorProfile profile = project.CollaboratorProfiles.Where(p => p is FacultyMemberProfile).ElementAt(model.CollaboratorProfileIndex);
                application.CollaboratorProfileId = profile.CollaboratorProfileId;
            }

            application.ApplicantId = 1; // FIX

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
    }
}
