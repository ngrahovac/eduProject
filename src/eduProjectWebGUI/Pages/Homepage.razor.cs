using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using Microsoft.AspNetCore.Components;
using eduProjectWebGUI.Utils;

namespace eduProjectWebGUI.Pages
{
    public partial class Homepage
    {
        [Parameter]
        public string QueryString { get; set; }

        private ICollection<ProjectDisplayModel> projectDisplayModels = new List<ProjectDisplayModel>();

        private SearchParameters searchParameters = new SearchParameters();

        protected override async Task OnInitializedAsync()
        {
            projectDisplayModels = (await ApiService.GetAsync<ICollection<ProjectDisplayModel>>("/projects")).ToList();

            var queryString = NavigationManager.QueryString();

            if (queryString.Count == 0)
            {
                // recommended projects
            }

            if (queryString["status"] == "active")
            {
                projectDisplayModels = projectDisplayModels.Where(m => m.ProjectStatus == ProjectStatus.Active).ToList();
            }

            if (queryString["authored"] == "true")
            {
                projectDisplayModels = projectDisplayModels.Where(m => m.IsDisplayForAuthor == true).ToList();
            }
        }

        private async Task LoadProject(int projectId)
        {
            NavigationManager.NavigateTo($"/projects/{projectId}");
        }

        private async void SearchProjects()
        {

        }

    }
}
