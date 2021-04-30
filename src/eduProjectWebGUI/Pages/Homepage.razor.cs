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

        public string Title { get; set; } = "";

        private ICollection<ProjectDisplayModel> projectDisplayModels = new List<ProjectDisplayModel>();

        private SearchParameters searchParameters = new SearchParameters();

        protected override async Task OnInitializedAsync()
        {
            projectDisplayModels = (await ApiService.GetAsync<ICollection<ProjectDisplayModel>>("/projects")).ToList();

            var queryString = NavigationManager.QueryString();

            if (queryString.Count == 0)
            {
                Title = "Preporučeni projekti";
                projectDisplayModels = projectDisplayModels.Where(m => m.Recommended).ToList();
            }

            if (queryString["status"] == "active")
            {
                Title = "Aktivni projekti";
                projectDisplayModels = projectDisplayModels.Where(m => m.ProjectStatus == ProjectStatus.Active).ToList();
            }

            if (queryString["authored"] == "true")
            {
                Title = "Moji projekti";
                projectDisplayModels = projectDisplayModels.Where(m => m.IsForAuthor == true).ToList();
            }

            if (queryString["query"] != null)
            {
                Title = $"Rezultati pretrage za \"{queryString["query"]}\"";
                var query = queryString["query"];
                projectDisplayModels = projectDisplayModels.Where(m => m.Title.Contains(query) || m.Description.Contains(query)
                 || m.Tags.Where(t => t.Name.Contains(query)).Count() > 0).ToList();
            }
        }

        private async Task LoadProject(int projectId)
        {
            NavigationManager.NavigateTo($"/projects/{projectId}");
        }
    }
}
