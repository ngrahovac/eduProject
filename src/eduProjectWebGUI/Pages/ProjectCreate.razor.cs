using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
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

        ProjectInputModel projectInputModel = new ProjectInputModel();

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

                // add display model 
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
            var parameters = new ModalParameters();
            parameters.Add("ProjectInputModel", projectInputModel);
            parameters.Add("Editing", editing);
            parameters.Add("faculties", faculties);
            parameters.Add("studyFields", studyFields);
            parameters.Add("tags", tags);

            Modal.Show(typeof(ProjectCreateAddCollaborator), "Kreiranje profila saradnika", parameters);
        }
    }
}
