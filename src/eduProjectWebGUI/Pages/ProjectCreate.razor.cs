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
            set { addedTag = value; projectInputModel.TagNames.Add(addedTag.Name); Console.WriteLine($"Dodan tag sad ih je {projectInputModel.TagNames.Count()}"); }
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
                projectInputModel = new ProjectInputModel(model);
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
                var parameters = new ModalParameters();
                string Title = "Potvrda o čuvanju izmjena";
                parameters.Add(nameof(Title), Title);
                var messageForm = Modal.Show<ActionConfirmationPopup>(nameof(Title), parameters);
                var result = await messageForm.Result;

                if (!result.Cancelled)
                {
                    await ApiService.PutAsync($"/projects/{Id}", projectInputModel);
                    NavigationManager.NavigateTo($"/projects/{Id}", true);
                }

            }
            else
            {
                var parameters = new ModalParameters();
                string Title = "Potvrda o objavljivanju projekta";
                parameters.Add(nameof(Title), Title);
                var messageForm = Modal.Show<ActionConfirmationPopup>(nameof(Title), parameters);
                var result = await messageForm.Result;

                if (!result.Cancelled)
                {
                    await ApiService.PostAsync("/projects", projectInputModel);
                    NavigationManager.NavigateTo("/homepage", true);
                }
            }
        }

        private async void RemoveCollaboratorProfile(CollaboratorProfileInputModel profile)
        {
            projectInputModel.CollaboratorProfileInputModels.Remove(profile);
        }

        private async void EditCollaboratorProfile(CollaboratorProfileInputModel model)
        {
            int index = -1;
            for (int i = 0; i < projectInputModel.CollaboratorProfileInputModels.Count; i++)
            {
                if (projectInputModel.CollaboratorProfileInputModels.ElementAt(i).Equals(model))
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                ShowCreateCollaboratorProfilePopup(true, index);
            }
        }

        private async Task<IEnumerable<Tag>> FilterTags(string searchText)
        {
            Console.WriteLine(tags.Count());
            Console.WriteLine($"pretrazujemo {searchText}");
            return tags.Where(t => t.Name.StartsWith(searchText));
        }

        async void ShowCreateCollaboratorProfilePopup(bool editing, int profileIndex)
        {
            var parameters = new ModalParameters();
            parameters.Add("ProjectInputModel", projectInputModel);
            parameters.Add("editingProfile", editing);
            parameters.Add("profileIndex", profileIndex);

            parameters.Add("faculties", faculties);
            parameters.Add("studyFields", studyFields);
            parameters.Add("tags", tags);

            var form = Modal.Show(typeof(ProjectCreateAddCollaborator), "Kreiranje profila saradnika", parameters);
            var result = await form.Result;
            if (!result.Cancelled)
                StateHasChanged();
        }

        async Task CancelEditing()
        {
            NavigationManager.NavigateTo($"/projects/{Id}");
        }
    }
}
