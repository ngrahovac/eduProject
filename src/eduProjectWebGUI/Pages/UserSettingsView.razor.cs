using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
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
                userSettingsDisplayModel = await ApiService.GetAsync<UserSettingsDisplayModel>($"/users/{UserId}/settings");
                userSettingsInputModel = new UserSettingsInputModel(userSettingsDisplayModel);

                var accountDisplayModel = await ApiService.GetAsync<AccountDisplayModel>($"/account/{UserId}");
                registerInputModel = new RegisterInputModel(accountDisplayModel);
                tags = (await ApiService.GetAsync<Dictionary<string, Tag>>($"tags")).Values.ToList();
                faculties = await ApiService.GetAsync<ICollection<Faculty>>($"faculties");
                studyFields = (await ApiService.GetAsync<Dictionary<string, StudyField>>($"fields")).Values;

                var profileDisplayModel = await ApiService.GetAsync<ProfileDisplayModel>($"/users/{UserId}");
                userProfileInputModel = new UserProfileInputModel(profileDisplayModel);
                userProfileInputModel.UserId = UserId;
            }
            catch (Exception ex)
            {
                NavigationManager.NavigateTo("/404");
            }
        }

        private async Task<IEnumerable<Tag>> FilterTags(string searchText)
        {
            return tags.Where(t => t.Name.StartsWith(searchText));
        }

        private async Task UpdateUserInformation()
        {
            await ApiService.PutAsync($"/users/{UserId}", userProfileInputModel);
        }

        private async Task UpdateAccountInformation()
        {
            await ApiService.PutAsync($"/account/{UserId}", registerInputModel);
        }

        private async Task UpdateSettings()
        {
            await ApiService.PutAsync($"/users/{UserId}/settings", userSettingsInputModel);
            //NavigationManager.NavigateTo($"/users/{UserId}/settings", true);
        }

        private async Task CancelUpdateSettings()
        {
            NavigationManager.NavigateTo($"/homepage", true);
        }

    }
}
