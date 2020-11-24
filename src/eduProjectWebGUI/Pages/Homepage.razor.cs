using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Display;
using eduProjectModel.Domain;

namespace eduProjectWebGUI.Pages
{
    public partial class Homepage
    {

        private ICollection<ProjectDisplayModel> projectDisplayModels = new List<ProjectDisplayModel>();

        private SearchParameters searchParameters = new SearchParameters();

        protected override async Task OnInitializedAsync()
        {
            await GetAllProjects();
        }

        private async Task LoadProject(int projectId)
        {
            // navigates to project page
        }

        // filtering functions for different tabs
        // ako se ne mogne namjestiti da tabovi u navigaciji ne mijenjaju stranicu nego okidaju funkcije, napraviti odvojene stranice

        private async Task GetAllProjects()
        {
            projectDisplayModels = (await ApiService.GetAsync<ICollection<ProjectDisplayModel>>("/projects")).ToList();
        }

        private async Task GetActiveProjects()
        {
            projectDisplayModels = projectDisplayModels.Where(p => p.ProjectStatus == ProjectStatus.Active).ToList();
        }

        private async Task GetRecommendedProjects()
        {
            throw new NotImplementedException();
        }

        private async Task GetAuthorProjects()
        {
            throw new NotImplementedException();
        }

        private async Task SearchProjects()
        {
            //
        }

    }
}
