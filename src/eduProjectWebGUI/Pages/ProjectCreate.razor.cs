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
            try
            {
                /*faculties = (await ApiService.GetAsync<ICollection<Faculty>>($"faculties")).Item1;
                studyFields = (await ApiService.GetAsync<Dictionary<string, StudyField>>($"fields")).Item1.Values;
                tags = (await ApiService.GetAsync<Dictionary<string, Tag>>($"tags")).Item1.Values.ToList();

                if (Id > 0)
                {
                    // editing existing project
                    editing = true;
                    var response = await ApiService.GetAsync<ProjectDisplayModel>($"/projects/{Id}");
                    var code = response.Item2;

                    if (code.IsSuccessCode())
                    {
                        var displayModel = response.Item1;
                        projectInputModel = new ProjectInputModel(displayModel);
                        projectInputModel.AuthorId = (int)await LocalStorage.ExtractUserId();
                    }
                    else
                    {
                        if (code.ShouldRedirectTo404())
                            NavigationManager.NavigateTo("/404");

                        else
                        {
                            var parameters = new ModalParameters();
                            parameters.Add(nameof(InfoPopup.Message), code.GetMessage());
                            Modal.Show<InfoPopup>("Obavještenje", parameters);
                        }
                    }
                }
                else
                {
                    // creating new project
                    editing = false;
                }*/
                var responseFaculties = await ApiService.GetAsync<ICollection<Faculty>>($"faculties");
                var responseFields = await ApiService.GetAsync<Dictionary<string, StudyField>>($"fields");
                var responseTags = await ApiService.GetAsync<Dictionary<string, Tag>>($"tags");

                if (!responseFaculties.Item2.IsSuccessCode() || !responseFields.Item2.IsSuccessCode() || !responseTags.Item2.IsSuccessCode())
                {
                    var parameters = new ModalParameters();
                    parameters.Add(nameof(InfoPopup.Message), "Problem pri dohvatanju podataka");
                    Modal.Show<InfoPopup>("Obavještenje", parameters);
                }
                else
                {
                    faculties = responseFaculties.Item1;
                    studyFields = responseFields.Item1.Values;
                    tags = responseTags.Item1.Values.ToList();

                    if (Id > 0)
                    {
                        // editing existing project
                        editing = true;
                        var response = await ApiService.GetAsync<ProjectDisplayModel>($"/projects/{Id}");
                        var code = response.Item2;

                        if (code.IsSuccessCode())
                        {
                            var displayModel = response.Item1;
                            projectInputModel = new ProjectInputModel(displayModel);
                            projectInputModel.AuthorId = (int)await LocalStorage.ExtractUserId();
                        }
                        else
                        {
                            if (code.ShouldRedirectTo404())
                                NavigationManager.NavigateTo("/404");

                            else
                            {
                                var parameters = new ModalParameters();
                                parameters.Add(nameof(InfoPopup.Message), code.GetMessage());
                                Modal.Show<InfoPopup>("Obavještenje", parameters);
                            }
                        }
                    }
                    else
                    {
                        // creating new project
                        editing = false;
                    }
                }

            }
            catch (Exception ex)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
        }

        private async void CreateOrUpdateProject()
        {
            try
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
                        var response = await ApiService.PutAsync($"/projects/{Id}", projectInputModel);
                        var parameters2 = new ModalParameters();
                        parameters2.Add(nameof(InfoPopup.Message), response.StatusCode.GetMessage());

                        var msgForm = Modal.Show<InfoPopup>("Obavještenje", parameters2);
                        var resForm = await msgForm.Result;

                        if (response.IsSuccessStatusCode && !resForm.Cancelled)
                        {
                            NavigationManager.NavigateTo($"/projects/{Id}", true);
                        }
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
                        var response = await ApiService.PostAsync("/projects", projectInputModel);
                        var parameters2 = new ModalParameters();
                        parameters2.Add(nameof(InfoPopup.Message), response.StatusCode.GetMessage());

                        var msgForm = Modal.Show<InfoPopup>("Obavještenje", parameters2);
                        var resForm = await msgForm.Result;

                        //error - stranica ne postoji, status 404 ako navigujem ka projects/{Id}. treba dohvatiti ID novog projekta
                        if (response.IsSuccessStatusCode && !resForm.Cancelled)
                        {
                            NavigationManager.NavigateTo($"/homepage", true);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                Modal.Show<InfoPopup>("Obavještenje", parameters);
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
