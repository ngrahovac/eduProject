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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                try
                {
                    int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                    User user = await users.GetAsync(id);
                    if (user == null)
                        return NotFound();

                    var userSettings = await settings.GetAsync(user.UserId);
                    var userAccount = await userManager.FindByIdAsync(user.UserId.ToString());

                    if (!userAccount.ActiveStatus)
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
                    return BadRequest(e.Message + "\n" + e.StackTrace);
                }
            
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserProfileInputModel model)
        {
            try
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

                var usersIds = await userManager.Users.Select(u => int.Parse(u.Id)).ToListAsync();

                int userId = usersIds.Count == 0 ? 1 : usersIds.Max();
                user.UserId = userId;

                await users.AddAsync(user);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UserProfileInputModel model)
        {
            try
            {
                var faculty = (await faculties.GetAllAsync()).Where(f => f.Name == model.FacultyName).First();
                var existingUser = await users.GetAsync(id);

                if (existingUser is Student s)
                {
                    model.MapTo(s, faculty);
                }
                else if (existingUser is FacultyMember fm)
                {
                    model.MapTo(fm, faculty);
                }

                await users.UpdateAsync(existingUser);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpGet("{id}/tags")]
        public async Task<ActionResult<ICollection<Tag>>> GetUserTags(int userId)
        {
            try
            {
                Debug.WriteLine("helo");
                var userSettings = await settings.GetAsync(userId);
                return userSettings.UserTags.ToList();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }

        }
    }
}
