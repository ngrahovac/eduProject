using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebGUI.Shared;
using eduProjectWebGUI.Utils;
using Microsoft.AspNetCore.Components;

namespace eduProjectWebGUI.Pages
{
    public partial class ProjectCreate
    {
        [Parameter]
        public int Id { get; set; }

        private bool editing = false;

        ProjectInputModel projectInputModel = new ProjectInputModel();

        private Tag addedTag;
        public Tag AddedTag
        {
            get { return addedTag; }
            set { addedTag = value; projectInputModel.TagNames.Add(addedTag.Name); }
        }

        // combo boxes backing lists
        private ICollection<Faculty> faculties = new List<Faculty>();
        private ICollection<StudyField> studyFields = new List<StudyField>();
        private ICollection<Tag> tags = new List<Tag>();

        protected override async Task OnInitializedAsync()
        {
            faculties = await ApiService.GetAsync<ICollection<Faculty>>($"faculties");
            studyFields = (await ApiService.GetAsync<Dictionary<string, StudyField>>($"fields")).Values;
            tags = (await ApiService.GetAsync<Dictionary<string, Tag>>($"tags")).Values.ToList();

            if (Id > 0)
            {
                // editing existing project
                editing = true;
                var displayModel = await ApiService.GetAsync<ProjectDisplayModel>($"/projects/{Id}");
                projectInputModel = new ProjectInputModel(displayModel);
                projectInputModel.AuthorId = (int)await LocalStorage.ExtractUserId();
            }
            else
            {
                // creating new project
                editing = false;
            }
        }

        private async void CreateOrUpdateProject()
        {
            if (editing)
            {
                var parameters = new ModalParameters();
                var title = "Potvrda o čuvanju izmjena";
                parameters.Add(nameof(title), title);
                var messageForm = Modal.Show<ActionConfirmationPopup>(nameof(title), parameters);
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
                string title = "Potvrda o objavljivanju projekta";
                parameters.Add(nameof(title), title);
                var messageForm = Modal.Show<ActionConfirmationPopup>(nameof(title), parameters);
                var result = await messageForm.Result;

                if (!result.Cancelled)
                {
                    projectInputModel.ProjectStatus = ProjectStatus.Active;
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
