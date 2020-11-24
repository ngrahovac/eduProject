using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Display;
using Microsoft.AspNetCore.Components;

namespace eduProjectWebGUI.Pages
{
    public partial class SentApplicationsOverview
    {
        [Parameter] public int UserId { get; set; }

        private int selectedApplicationId = 0;

        public List<ProjectApplicationsDisplayModel> ProjectApplicationsDisplayModels { get; set; } = new List<ProjectApplicationsDisplayModel>();

        protected override async Task OnInitializedAsync()
        {
            ProjectApplicationsDisplayModels = (await ApiService.GetAsync<ICollection<ProjectApplicationsDisplayModel>>($"/applications/applicant/{UserId}")).ToList();

            foreach (var model in ProjectApplicationsDisplayModels)
            {
                model.CollaboratorProfileApplicationsDisplayModels = model.CollaboratorProfileApplicationsDisplayModels
                                                                          .Where(m => m.ApplicationDisplayModels.Select(a => a.ApplicantId).Contains(UserId)).ToList();
            }
        }

        public async Task RevokeApplication()
        {
            if (selectedApplicationId != 0)
            {
                await ApiService.DeleteAsync($"/applications/{selectedApplicationId}");
            }
        }
    }
}
