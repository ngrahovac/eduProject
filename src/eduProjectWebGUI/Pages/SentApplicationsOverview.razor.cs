using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using eduProjectModel.Display;
using eduProjectWebGUI.Shared;
using Microsoft.AspNetCore.Components;

namespace eduProjectWebGUI.Pages
{
    public partial class SentApplicationsOverview
    {
        [Parameter] public int UserId { get; set; }

        private int selectedApplicationId = 0;

        private HashSet<string> allTables = new HashSet<string>();
        private List<string> tablesToClear = new List<string>();

        public List<ProjectApplicationsDisplayModel> ProjectApplicationsDisplayModels { get; set; } = new List<ProjectApplicationsDisplayModel>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ProjectApplicationsDisplayModels = (await ApiService.GetAsync<ICollection<ProjectApplicationsDisplayModel>>($"/applications/applicant/{UserId}")).ToList();

                foreach (var model in ProjectApplicationsDisplayModels)
                {
                    model.CollaboratorProfileApplicationsDisplayModels = model.CollaboratorProfileApplicationsDisplayModels
                                                                              .Where(m => m.ApplicationDisplayModels.Select(a => a.ApplicantId)
                                                                              .Contains(UserId)).ToList();
                }
            }
            catch (Exception ex)
            {
                NavigationManager.NavigateTo("/404");
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                selectedApplicationId = 0;
            }
        }

        public async Task RevokeApplication()
        {
            if (selectedApplicationId != 0)
            {
                var parameters = new ModalParameters();
                string Title = "Potvrda o povlačenju prijave";
                parameters.Add(nameof(Title), Title);
                var messageForm = Modal.Show<ActionConfirmationPopup>(nameof(Title), parameters);
                var result = await messageForm.Result;

                if (!result.Cancelled)
                {
                    await ApiService.DeleteAsync($"/applications/{selectedApplicationId}");
                    NavigationManager.NavigateTo($"/users/{UserId}/applications", true);
                }
            }
        }
    }
}
