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
    public partial class UserSettings
    {
        [Parameter]
        public int UserId { get; set; }

        private UserSettingsDisplayModel displayModel;
        private UserSettingsInputModel inputModel;

        private ICollection<Tag> tags = new List<Tag>();

        private Tag addedTag;
        private Tag AddedTag
        {
            get { return addedTag; }
            set { addedTag = value; inputModel.UserTagNames.Add(value.Name); }
        }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                tags = (await ApiService.GetAsync<Dictionary<string, Tag>>($"tags")).Values.ToList();
                displayModel = await ApiService.GetAsync<UserSettingsDisplayModel>($"/users/{UserId}/settings");
                inputModel = new UserSettingsInputModel(displayModel);
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

        private async Task UpdateSettings()
        {
            await ApiService.PutAsync($"/users/{UserId}/settings", inputModel);
            NavigationManager.NavigateTo($"/users/{UserId}/settings", true);
        }

        private async Task CancelUpdateSettings()
        {
            NavigationManager.NavigateTo($"/homepage", true);
        }

    }
}
