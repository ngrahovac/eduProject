using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using eduProjectWebAPI.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using eduProjectWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/projects")]

    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsRepository projects;
        private readonly IUsersRepository users;
        private readonly IFacultiesRepository faculties;
        private readonly IUserSettingsRepository settings;

        public ProjectsController(IProjectsRepository projects, IUsersRepository users, IFacultiesRepository faculties, IUserSettingsRepository settings)
        {
            this.projects = projects;
            this.users = users;
            this.faculties = faculties;
            this.settings = settings;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDisplayModel>> GetById(int id)
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
                    User currentUser = await users.GetAsync(currentUserId);
                    Project project = await projects.GetAsync(id);

                    if (project == null)
                        return NotFound();

                    var model = await GetProjectDisplayModel(project, currentUserId, currentUser);
                    model.Links.Add("author_profile", $"{project.AuthorId}");

                    return model;
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ProjectDisplayModel>>> GetAll()
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
                    User currentUser = await users.GetAsync(currentUserId);
                    var projectDisplayModels = new List<ProjectDisplayModel>();

                    var projectsList = await projects.GetAllAsync();

                    foreach (var project in projectsList)
                    {
                        var model = await GetProjectDisplayModel(project, currentUserId, currentUser);
                        projectDisplayModels.Add(model);
                    }

                    return projectDisplayModels;
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        private async Task<ProjectDisplayModel> GetProjectDisplayModel(Project project, int currentUserId, User visitor)
        {
            User author = await users.GetAsync(project.AuthorId);
            bool isDisplayForAuthor = currentUserId == project.AuthorId;
            ProjectDisplayModel model;

            if (project.ProjectStatus == ProjectStatus.Active)
            {
                var facultyIds = project.CollaboratorProfiles.Select(p => p.FacultyId).Distinct();
                var facultiesList = new List<Faculty>();
                foreach (var fid in facultyIds)
                    facultiesList.Add(await faculties.GetAsync((int)fid));

                model = new ProjectDisplayModel(project, author, visitor, isDisplayForAuthor, null, facultiesList);
            }
            else
            {
                var collaboratorIds = project.CollaboratorIds;
                List<User> collaborators = new List<User>();
                foreach (int collabId in collaboratorIds)
                    collaborators.Add(await users.GetAsync(collabId));

                model = new ProjectDisplayModel(project, author, visitor, isDisplayForAuthor, collaborators);
            }

            if (!model.Recommended)
            {
                // if no recommended profiles, check tags
                var userTags = (await settings.GetAsync(visitor.UserId)).UserTags;
                foreach (var projectTag in project.Tags)
                {
                    if (userTags.Contains(projectTag))
                    {
                        model.Recommended = true;
                        break;
                    }
                }
            }

            return model;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectInputModel model)
        {
            if (HttpContext.Request.ExtractUserId() == null)
            {
                return Unauthorized();
            }
            else
            {
                try
                {
                    model.AuthorId = (int)HttpContext.Request.ExtractUserId();
                    Project project = new Project();

                    ICollection<Faculty> facultiesList = new List<Faculty>();
                    ICollection<Faculty> allFaculties = await faculties.GetAllAsync();

                    foreach (var collaboratorProfileInputModel in model.CollaboratorProfileInputModels)
                    {
                        var faculty = allFaculties.Where(x => x.Name.Equals(collaboratorProfileInputModel.FacultyName)).FirstOrDefault();
                        facultiesList.Add(faculty);
                    }

                    model.MapTo(project, facultiesList);

                    await projects.AddAsync(project);
                    return NoContent(); // FIX: 201 with link to the resource
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ProjectInputModel model)
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
                    Project project = await projects.GetAsync(id);

                    if (project.AuthorId != currentUserId)
                    {
                        return Forbid();
                    }
                    else
                    {
                        var facultiesList = new List<Faculty>();
                        var allFaculties = await faculties.GetAllAsync();

                        foreach (var collaboratorProfileInputModel in model.CollaboratorProfileInputModels)
                        {
                            var faculty = allFaculties.Where(x => x.Name.Equals(collaboratorProfileInputModel.FacultyName)).FirstOrDefault();
                            facultiesList.Add(faculty);
                        }

                        model.MapTo(project, facultiesList);
                        await projects.UpdateAsync(project);

                        return NoContent();
                    }
                }
                catch (Exception e)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Request.ExtractUserId() == null)
            {
                return Unauthorized();
            }
            else
            {
                try
                {
                    var project = await projects.GetAsync(id);

                    if (project.AuthorId == HttpContext.Request.ExtractUserId())
                    {
                        await projects.DeleteAsync(project);
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

