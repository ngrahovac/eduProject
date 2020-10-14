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

        [Parameter]
        public ProjectDisplayModel ProjectDisplayModel { get; set; }

        async Task ShowModalVisitorActive()
        {
            var messageForm = Modal.Show<LeaveComment>();
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                SignUpCancel = false;
                SignUpButton = true;
            }

            var comment = model.ApplicantComment;
            var numberOfIndex = model.CollaboratorProfileIndex;
            var typeOfProfile = model.CollaboratorProfileType;
            var id = model.ProjectId = Int32.Parse(ProjectId);

        }

        async Task ShowCancelWarningVisitorActive()
        {
            var parameters = new ModalParameters();
            string Title = "Potvrda o poništavanju prijave";
            parameters.Add(nameof(Title), Title);
            var messageForm = Modal.Show<CancelConfirmation>(nameof(Title), parameters);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                SignUpCancel = true;
                SignUpButton = false;
            }
        }

        async Task ShowModalAuthorActive() // Sta treba da se mijenja, kakav pop-up trebam da izbacujem?
        {
            var messageForm = Modal.Show<LeaveComment>();
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                SignUpCancel = false;
                SignUpButton = true;
            }
        }

        async Task ShowCancelWarningAuthorActive()
        {
            var parameters = new ModalParameters();
            string Title = "Potvrda o brisanju projekta";
            parameters.Add(nameof(Title), Title);
            var messageForm = Modal.Show<CancelConfirmation>(nameof(Title), parameters);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                SignUpCancel = true;
                SignUpButton = false;
            }
        }

        async Task ShowCancelWarningAuthorClosed()
        {
            var parameters = new ModalParameters();
            string Title = "Potvrda o brisanju projekta";
            parameters.Add(nameof(Title), Title);
            var messageForm = Modal.Show<CancelConfirmation>(nameof(Title), parameters);
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
        }

    }
}
