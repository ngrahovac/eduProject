using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using Microsoft.AspNetCore.Components;
using eduProjectWebGUI.Utils;
using Blazored.LocalStorage;
using Blazored.Modal;
using eduProjectWebGUI.Shared;
using Microsoft.EntityFrameworkCore.Query;

namespace eduProjectWebGUI.Pages
{
    public partial class Homepage
    {
        [Parameter]
        public string QueryString { get; set; }

        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        public string Title { get; set; } = "";

        private ICollection<ProjectDisplayModel> projectDisplayModels = new List<ProjectDisplayModel>();

        private SearchParameters searchParameters = new SearchParameters();

        private ICollection<Tag> userTags;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await ApiService.GetAsync<ICollection<ProjectDisplayModel>>("/projects");
                var code = response.Item2;

                if (!code.IsSuccessCode())
                {
                    var parameters = new ModalParameters();
                    parameters.Add(nameof(InfoPopup.Message), code.GetMessage());
                    Modal.Show<InfoPopup>("Obavještenje", parameters);
                }

                else
                {

                    projectDisplayModels = response.Item1.ToList();
                    var userId = await LocalStorage.ExtractUserId();
                    var settings = (await ApiService.GetAsync<UserSettingsDisplayModel>($"/users/{userId}/settings")).Item1;
                    userTags = settings.UserTags;

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
            }
            catch (Exception ex)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }
        }

        private async Task LoadProject(int projectId)
        {
            NavigationManager.NavigateTo($"/projects/{projectId}");
        }
    }
}
