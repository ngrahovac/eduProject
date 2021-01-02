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

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/projects")]

    public class ProjectsController : ControllerBase
    {
        private IProjectsRepository projects;
        private IUsersRepository users;
        private IFacultiesRepository faculties;

        public ProjectsController(IProjectsRepository projects, IUsersRepository users, IFacultiesRepository faculties)
        {
            this.projects = projects;
            this.users = users;
            this.faculties = faculties;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDisplayModel>> GetById(int id)
        {
            int userId = int.Parse(HttpContext.Request.ExtractUserId());

            Project project = await projects.GetAsync(id);

            if (project == null)
                return NotFound();

            User author = await users.GetAsync(project.AuthorId);

            bool isDisplayForAuthor = userId == project.AuthorId;

            if (project.ProjectStatus == ProjectStatus.Active)
            {
                var facultyIds = project.CollaboratorProfiles.Select(p => p.FacultyId).Distinct();
                var facultiesList = new List<Faculty>();
                foreach (var fid in facultyIds)
                    facultiesList.Add(await faculties.GetAsync((int)fid));

                return new ProjectDisplayModel(project, author, isDisplayForAuthor, true, null, facultiesList);
            }
            else
            {
                var collaboratorIds = project.CollaboratorIds;
                List<User> collaborators = new List<User>();
                foreach (int collabId in collaboratorIds)
                    collaborators.Add(await users.GetAsync(collabId));

                return new ProjectDisplayModel(project, author, isDisplayForAuthor, false, collaborators);
            }
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<ProjectDisplayModel>>> GetAll()
        {
            var projectDisplayModels = new List<ProjectDisplayModel>();

            try
            {
                var allProjects = await projects.GetAllAsync();

                foreach (var project in allProjects)
                {
                    User author = await users.GetAsync(project.AuthorId);

                    if (project.ProjectStatus == ProjectStatus.Active)
                    {
                        var facultyIds = project.CollaboratorProfiles.Select(p => p.FacultyId).Distinct();
                        var facultiesList = new List<Faculty>();
                        foreach (var fid in facultyIds)
                            facultiesList.Add(await faculties.GetAsync((int)fid));

                        projectDisplayModels.Add(new ProjectDisplayModel(project, author, false, true, null, facultiesList));
                    }
                    else
                    {
                        var collaboratorIds = project.CollaboratorIds;
                        List<User> collaborators = new List<User>();
                        foreach (int collabId in collaboratorIds)
                            collaborators.Add(await users.GetAsync(collabId));

                        projectDisplayModels.Add(new ProjectDisplayModel(project, author, false, false, collaborators));
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return projectDisplayModels;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectInputModel model)
        {
            //ProjectInputValidator validator = new ProjectInputValidator();
            //ValidationResult result = validator.Validate(model);

            //if (result.IsValid)
            //{
            Project project = new Project();

            ICollection<Faculty> facultiesList = new List<Faculty>();
            ICollection<Faculty> allFaculties = await faculties.GetAllAsync();

            foreach (var collaboratorProfileInputModel in model.CollaboratorProfileInputModels)
            {
                var faculty = allFaculties.Where(x => x.Name.Equals(collaboratorProfileInputModel.FacultyName)).FirstOrDefault();
                facultiesList.Add(faculty);
            }

            model.MapTo(project, facultiesList);

            //Initialize AuthorId from Project class - see authentication/authorization
            project.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            //Write created project to database
            try
            {
                await projects.AddAsync(project);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            //}

            /*Possibility of retrieving errors caused by incorrect user input:
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.ErrorMessage);
            }*/
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, ProjectInputModel model)
        {
            Project project = new Project();

            ICollection<Faculty> facultiesList = new List<Faculty>();
            ICollection<Faculty> allFaculties = await faculties.GetAllAsync();

            foreach (var collaboratorProfileInputModel in model.CollaboratorProfileInputModels)
            {
                var faculty = allFaculties.Where(x => x.Name.Equals(collaboratorProfileInputModel.FacultyName)).FirstOrDefault();
                facultiesList.Add(faculty);
            }

            model.MapTo(project, facultiesList);
            project.ProjectId = id;

            await projects.UpdateAsync(project);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var project = await projects.GetAsync(id);
                await projects.DeleteAsync(project);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}

