using Blazored.LocalStorage;
using Blazored.Modal;
using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectWebGUI.Services;
using eduProjectWebGUI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class ProjectView
    {
        //private ILocalStorageService localStorage; //bilo je readonly
        //public ProjectView(ILocalStorageService localStorage) => this.localStorage = localStorage;
        public ProjectView() { }

        [Parameter]
        public int ProjectId { get; set; }

        [Inject]
        ApiService ApiService { get; set; }

        [Parameter]
        public ProjectDisplayModel ProjectDisplayModel { get; set; }

        // Metoda koja prikazuje prozor sa mogucnoscu unosenja komentara kada se klikne na button PRIJAVI SE.
        // Kada korisnik gleda tudji projekat na koji se moze prijaviti, a nije prethodno prijavljen.
        async Task ShowApplicationSubmitPopup()
        {
            var messageForm = Modal.Show<LeaveComment>();
            var result = await messageForm.Result;

            // Ako je korisnik kliknuo button PRIHVATI, ulazi se u if.
            if (!result.Cancelled)
            {
                model.ProjectApplicationStatus = ProjectApplicationStatus.OnHold;
                model.ProjectId = ProjectId;
                await ApiService.PostAsync("/applications", model);
            }

            messageForm.Close();
        }

        // Metoda koja bi sa klikom na button IZMIJENI dala mogucnost da autor mijenja nesto na AKTIVNOM projektu.
        // Kada korisnik gleda svoj projekat koji je aktivan. (Autor projekta)
        async Task EditProject()
        {
            NavigationManager.NavigateTo($"/projects/{ProjectId}/edit", true);
        }

        // Metoda koja prikazuje prozor koji pita da li smo sigurni da zelimo da izbrisemo projekat.
        // Kada korisnik gleda projekat koji je on sam napravio. (Autor projekta)
        // Metoda je ista i za aktivan i za zatvoren projekat.
        async Task ShowProjectDeleteWarning()
        {
            var parameters = new ModalParameters();
            string Title = "Potvrda o brisanju projekta";
            parameters.Add(nameof(Title), Title);
            var messageForm = Modal.Show<ActionConfirmationPopup>(nameof(Title), parameters);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {
                await ApiService.DeleteAsync($"projects/{ProjectId}");
                NavigationManager.NavigateTo("/homepage");
            }
        }

        async Task ShowReceivedApplicationsPopup()
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(ProjectId), ProjectId);
            Modal.Show<ReceivedApplicationsOverview>("Pristigle prijave", parameters);
        }

        protected override async Task OnInitializedAsync()
        {
            ProjectDisplayModel = await ApiService.GetAsync<ProjectDisplayModel>($"projects/{ProjectId}");
        }

    }
}
