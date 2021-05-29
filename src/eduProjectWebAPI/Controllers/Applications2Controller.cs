﻿using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/applications2")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Applications2Controller : ControllerBase
    {
        private readonly IProjectsRepository projects;
        private readonly IProjectApplicationsRepository applications;
        private readonly IUsersRepository users;
        private readonly IUserSettingsRepository settings;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public Applications2Controller(IProjectsRepository projects, IProjectApplicationsRepository applications,
            IUsersRepository users, IUserSettingsRepository settings, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.projects = projects;
            this.applications = applications;
            this.users = users;
            this.settings = settings;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("applicant/{id:int}")]
        public async Task<ActionResult<ICollection<ApplicationDisplayModel>>> GetByApplicantId(int id)
        {
            if (int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) == id)
            {
                var appls = await applications.GetByApplicantIdAsync(id);
                //appls.OrderBy(a => a.ProjectId); //////////////////////////////////////

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
    }
}
