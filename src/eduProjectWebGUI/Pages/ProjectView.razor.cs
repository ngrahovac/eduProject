using eduProjectModel.Display;
using eduProjectWebGUI.Services;
using eduProjectWebGUI.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class ProjectView
    {
        [Parameter]
        public string ProjectId { get; set; }

        [Inject]
        ApiService ApiService { get; set; }

        ProjectDisplayModel ProjectDisplayModel { get; set; }

        void ShowModal()
        {
            Modal.Show<LeaveComment>();
        }

        protected override async Task OnInitializedAsync()
        {
            ProjectDisplayModel = await ApiService.GetAsync<ProjectDisplayModel>($"projects/{ProjectId}");
        }
    }
}
