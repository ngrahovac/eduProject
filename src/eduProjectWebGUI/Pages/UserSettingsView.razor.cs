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
    public partial class UserSettingsView
    {
        [Parameter]
        public int UserId { get; set; }

        private UserSettingsDisplayModel userSettingsDisplayModel;
        private UserSettingsInputModel userSettingsInputModel;

        private UserProfileInputModel userProfileInputModel;
        private RegisterInputModel registerInputModel;

        private ICollection<Faculty> faculties;
        private ICollection<StudyField> studyFields;
        private ICollection<Tag> tags = new List<Tag>();

        private Tag addedTag;
        private Tag AddedTag
        {
            get { return addedTag; }
            set { addedTag = value; userSettingsInputModel.UserTagNames.Add(value.Name); }
        }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                // detaljna provjera za ovaj api call
                // da sprijecimo posjecivanje stranice sa idjem koji nije korisnikov
                var response = await ApiService.GetAsync<UserSettingsDisplayModel>($"/users/{UserId}/settings");
                var code = response.Item2;

                if (!code.IsSuccessCode())
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
                else
                {
                    // korisnik je autorizovan da dohvati svoj account i ostale informacije
                    // ako bude problema u nastavku koda i dohvatanju ostalih komponenti, propada se na exception prozor
                    userSettingsDisplayModel = response.Item1;
                    userSettingsInputModel = new UserSettingsInputModel(userSettingsDisplayModel);

                    var accountDisplayModel = (await ApiService.GetAsync<AccountDisplayModel>($"/account/{UserId}")).Item1;
                    registerInputModel = new RegisterInputModel(accountDisplayModel);
                    tags = (await ApiService.GetAsync<Dictionary<string, Tag>>($"tags")).Item1.Values.ToList();
                    faculties = (await ApiService.GetAsync<ICollection<Faculty>>($"faculties")).Item1;
                    studyFields = (await ApiService.GetAsync<Dictionary<string, StudyField>>($"fields")).Item1.Values;

                    var profileDisplayModel = (await ApiService.GetAsync<ProfileDisplayModel>($"/users/{UserId}")).Item1;
                    userProfileInputModel = new UserProfileInputModel(profileDisplayModel);
                    userProfileInputModel.UserId = UserId;
                }
            }
            catch (Exception ex)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
        }

        private async Task<IEnumerable<Tag>> FilterTags(string searchText)
        {
            return tags.Where(t => t.Name.StartsWith(searchText));
        }

        private async Task UpdateUserInformation()
        {
            try
            {
                var response = await ApiService.PutAsync($"/users/{UserId}", userProfileInputModel);
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), response.StatusCode.GetMessage());
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
            catch (Exception ex)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
        }

        private async Task UpdateAccountInformation()
        {
            try
            {
                var response = await ApiService.PutAsync($"/account/{UserId}", registerInputModel);
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), response.StatusCode.GetMessage());
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
            catch (Exception ex)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
        }

        private async Task UpdateSettings()
        {
            try
            {
                var response = await ApiService.PutAsync($"/users/{UserId}/settings", userSettingsInputModel);
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), response.StatusCode.GetMessage());
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
            catch (Exception ex)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
        }

        private async Task CancelUpdateSettings()
        {
            NavigationManager.NavigateTo($"/homepage", true);
        }

    }
}
