using Blazored.Modal;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectWebGUI.Services;
using eduProjectWebGUI.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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

        private bool SignUpCancel = true;
        private bool SignUpButton = false;

        ProjectDisplayModel ProjectDisplayModel { get; set; }
        VisitorActiveProjectDisplayModel VisitorActiveProjectDisplayModel { get; set; }
        VisitorClosedProjectDisplayModel VisitorClosedProjectDisplayModel { get; set; }
        
        async Task ShowModal()
        {
            var messageForm = Modal.Show<LeaveComment>();
            var result = await messageForm.Result;

            if(!result.Cancelled)
            {
                SignUpCancel = false;
                SignUpButton = true;
            }
        }

        async Task ShowCancelWarning()
        {
            var messageForm = Modal.Show<ProjectSignUpCancelConfirmation>();
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                SignUpCancel = true;
                SignUpButton = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            ProjectDisplayModel = await ApiService.GetAsync<ProjectDisplayModel>($"projects/{ProjectId}");
            VisitorActiveProjectDisplayModel = await ApiService.GetAsync<VisitorActiveProjectDisplayModel>($"projects/{ProjectId}");
            VisitorClosedProjectDisplayModel = await ApiService.GetAsync<VisitorClosedProjectDisplayModel>($"projects/{ProjectId}");
        }

    }
}
