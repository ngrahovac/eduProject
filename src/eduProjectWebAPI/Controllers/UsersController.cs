using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using eduProjectWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/users")]
    public class UsersController : ControllerBase
    {
        private readonly IProjectsRepository projects;
        private readonly IUsersRepository users;
        private readonly IFacultiesRepository faculties;
        private readonly IUserSettingsRepository settings;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(IProjectsRepository projects, IUsersRepository users, IFacultiesRepository faculties, IUserSettingsRepository settings, UserManager<ApplicationUser> userManager)
        {
            this.projects = projects;
            this.users = users;
            this.faculties = faculties;
            this.settings = settings;
            this.userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileDisplayModel>> GetById(int id)
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

                    User user = await users.GetAsync(id);
                    var userSettings = await settings.GetAsync(user.UserId);

                    if (user == null)
                        return NotFound();

                    var authoredProjects = new List<Project>();
                    var projectCollaborations = new List<Project>();

                    if (userSettings.ProjectsVisible)
                    {
                        foreach (int projectId in user.AuthoredProjectIds)
                            authoredProjects.Add(await projects.GetAsync(projectId));

                        foreach (int projectId in user.ProjectCollaborationIds)
                            projectCollaborations.Add(await projects.GetAsync(projectId));
                    }

                    var faculty = await faculties.GetAsync(user.FacultyId);
                    bool isPersonal = currentUserId == id;

                    var model = new ProfileDisplayModel(user, isPersonal, faculty, authoredProjects, projectCollaborations, userSettings);

                    if (userSettings.EmailVisible)
                    {
                        model.Email = (await userManager.FindByIdAsync($"{user.UserId}")).Email;
                    }

                    if (userSettings.PhoneVisible)
                    {
                        model.PhoneNumber = (await userManager.FindByIdAsync($"{user.UserId}")).PhoneNumber;
                    }

                    return model;
                }
                catch (Exception e)
                {
                    //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    return BadRequest(e.Message + "\n" + e.StackTrace);
                }
            }
        }

        public async Task<ActionResult> Create(UserProfileInputModel model)
        {
            var faculty = (await faculties.GetAllAsync()).Where(f => f.Name == model.FacultyName).First();

            User user = null;

            if (model.UserAccountType is UserAccountType.Student)
            {
                Student s = new Student();
                model.MapTo(s, faculty);
                user = s;
            }
            else if (model.UserAccountType is UserAccountType.FacultyMember)
            {
                FacultyMember fm = new FacultyMember();
                model.MapTo(fm, faculty);
                user = fm;
            }

            await users.AddAsync(user);
            return NoContent(); // TODO: Change to 201
        }
    }
}
