using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebGUI.Shared;
using Microsoft.AspNetCore.Components;

namespace eduProjectWebGUI.Pages
{
    public partial class ProjectCreate
    {
        [Parameter]
        public int Id { get; set; }

        private bool editing = false;

        private ProjectInputModel projectInputModel = new ProjectInputModel();

        // input model for collaborator profile the user is currently creating
        private CollaboratorProfileInputModel collaboratorProfileInputModel = new CollaboratorProfileInputModel();

        // selected values
        private string cycleStr;
        private string yearStr;

        private Tag addedTag;
        public Tag AddedTag
        {
            get { return addedTag; }
            set { addedTag = value; Console.WriteLine($"Dodan {addedTag.Name}"); projectInputModel.TagNames.Add(addedTag.Name); }
        }

        private Faculty faculty;
        int cycle;
        private StudyProgram program;
        private StudyProgramSpecialization specialization;

        // combo boxes backing lists
        private ICollection<StudyField> studyFields = new List<StudyField>();
        private ICollection<Faculty> faculties = new List<Faculty>();
        private ICollection<string> cycles = new List<string>();
        private ICollection<StudyProgram> programs = new List<StudyProgram>();
        private ICollection<StudyProgramSpecialization> specializations = new List<StudyProgramSpecialization>();
        private ICollection<int> years = new List<int>();
        private ICollection<Tag> tags = new List<Tag>();


        protected override async Task OnInitializedAsync()
        {
            faculties = await ApiService.GetAsync<ICollection<Faculty>>($"faculties");
            studyFields = (await ApiService.GetAsync<Dictionary<string, StudyField>>($"fields")).Values;
            tags = (await ApiService.GetAsync<Dictionary<string, Tag>>($"tags")).Values.ToList();

            if (Id > 0)
            {
                // editing project
                editing = true;
                var model = await ApiService.GetAsync<ProjectDisplayModel>($"/projects/{Id}");
                projectInputModel = new ProjectInputModel
                {
                    Title = model.Title,
                    Description = model.Description,
                    StudyFieldName = model.StudyField != null ? model.StudyField.Name : null,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    TagNames = model.Tags.Select(t => t.Name).ToList()
                };

                projectInputModel.CollaboratorProfileInputModels = new List<CollaboratorProfileInputModel>();
                List<CollaboratorProfileDisplayModel> collaboratorProfileDisplayModels = new List<CollaboratorProfileDisplayModel>();
                foreach (var p in model.StudentProfileDisplayModels)
                {
                    collaboratorProfileDisplayModels.Add(p);
                }
                foreach (var p in model.FacultyMemberProfileDisplayModels)
                {
                    collaboratorProfileDisplayModels.Add(p);
                }

                foreach (var profileDisplayModel in collaboratorProfileDisplayModels)
                {
                    var collaboratorProfileInputModel = CollaboratorProfileInputModel.FromCollaboratorProfileDisplayModel(profileDisplayModel);
                    collaboratorProfileInputModel.AddedOnCreate = true;
                    projectInputModel.CollaboratorProfileInputModels.Add(collaboratorProfileInputModel);
                }

            }
            else
            {
                editing = false;
                // creating new project
            }
        }

        private async void CreateOrUpdateProject()
        {
            if (editing)
            {
                await ApiService.PutAsync($"/projects/{Id}", projectInputModel);
            }
            else
            {
                await ApiService.PostAsync("/projects", projectInputModel);
            }
        }

        private async void CollaboratorProfileTypeChanged(CollaboratorProfileType type)
        {
            collaboratorProfileInputModel = new CollaboratorProfileInputModel();
            yearStr = string.Empty;
            cycleStr = string.Empty;

            collaboratorProfileInputModel.CollaboratorProfileType = type;
            Console.WriteLine($"Odabran tip saradnika {type}");
        }

        private async void FacultySelected(string facultyName)
        {
            Console.WriteLine($"Odabran fakultet {facultyName}");
            cycles.Clear();
            programs.Clear();
            specializations.Clear();
            years.Clear();
            cycleStr = string.Empty;
            yearStr = string.Empty;

            if (facultyName != string.Empty)
            {
                collaboratorProfileInputModel.FacultyName = facultyName;
                faculty = faculties.Where(f => f.Name == facultyName).First();
                cycles = faculty.StudyPrograms.Select(p => p.Cycle).Distinct().Select(c => $"{c}").ToList();
            }
            else
            {
                collaboratorProfileInputModel.FacultyName = null;
            }
        }

        private async void CycleSelected(string cycleStr)
        {
            this.cycleStr = cycleStr;
            Console.WriteLine($"Odabran ciklus {cycleStr}");
            programs.Clear();
            specializations.Clear();
            years.Clear();

            if (cycleStr != string.Empty)
            {
                collaboratorProfileInputModel.Cycle = int.Parse(cycleStr);
                cycle = int.Parse(cycleStr);
                programs = faculty.StudyPrograms.Where(sp => sp.Cycle == cycle).ToList();
            }
            else
            {
                collaboratorProfileInputModel.Cycle = null;
            }
        }

        private async void ProgramSelected(string programName)
        {
            Console.WriteLine($"Odabran program {programName}");
            specializations.Clear();
            years.Clear();
            yearStr = string.Empty;

            if (programName != string.Empty)
            {
                collaboratorProfileInputModel.StudyProgramName = programName;
                var program = programs.Where(p => p.Cycle == cycle && p.Name == programName).First();
                specializations = program.StudyProgramSpecializations.ToList();
                years = Enumerable.Range(1, program.DurationYears).ToList();
            }
            else
            {
                collaboratorProfileInputModel.StudyProgramName = null;
            }
        }

        private async void YearSelected(string yearStr)
        {
            this.yearStr = yearStr;
            Console.WriteLine($"Odabran ciklus {yearStr}");

            if (yearStr != string.Empty)
            {
                collaboratorProfileInputModel.StudyYear = int.Parse(yearStr);
            }
            else
            {
                collaboratorProfileInputModel.StudyYear = null;
            }
        }

        private async void SpecializationSelected(string specializationName)
        {
            Console.WriteLine($"Odabran smjer {specializationName}");

            if (specializationName != string.Empty)
            {
                collaboratorProfileInputModel.StudyProgramSpecializationName = specializationName;
            }
            else
            {
                collaboratorProfileInputModel.StudyProgramSpecializationName = null;
            }

        }

        private async void AddCollaboratorProfile()
        {
            Console.WriteLine($"Adding {collaboratorProfileInputModel.CollaboratorProfileType}");
            collaboratorProfileInputModel.AddedOnCreate = !editing;
            projectInputModel.CollaboratorProfileInputModels.Add(collaboratorProfileInputModel);
            collaboratorProfileInputModel = new CollaboratorProfileInputModel();

            yearStr = string.Empty;
            cycleStr = string.Empty;
        }

        private async void RemoveCollaboratorProfile(CollaboratorProfileInputModel profile)
        {
            projectInputModel.CollaboratorProfileInputModels.Remove(profile);
        }

        private async Task<IEnumerable<Tag>> FilterTags(string searchText)
        {
            Console.WriteLine(tags.Count());
            Console.WriteLine($"pretrazujemo {searchText}");
            return tags.Where(t => t.Name.StartsWith(searchText));
        }

        async Task AddCollaboratorPopUp()
        {
            var messageForm = Modal.Show<ProjectCreateAddCollaborator>();
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                // Do something
            }
        }
    }
}
