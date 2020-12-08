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
        public int ProjectId { get; set; }

        [Inject]
        ApiService ApiService { get; set; }

        [Parameter]
        public ProjectDisplayModel ProjectDisplayModel { get; set; }

        // Metoda koja prikazuje prozor sa mogucnoscu unosenja komentara kada se klikne na button PRIJAVI SE.
        // Kada korisnik gleda tudji projekat na koji se moze prijaviti, a nije prethodno prijavljen.
        async Task ShowModalVisitorActive()
        {
            var messageForm = Modal.Show<LeaveComment>();
            var result = await messageForm.Result;

            // Ako je korisnik kliknuo button PRIHVATI, ulazi se u if.
            if (!result.Cancelled)
            {
                // TODO: remove button enabling/disabling
                // Bane je uklonio ↑ na client strani. 
                model.ProjectApplicationStatus = ProjectApplicationStatus.OnHold;
                await ApiService.PostAsync("/applications", model);
            }
        }

        // Metoda koja bi sa klikom na button IZMIJENI dala mogucnost da autor mijenja nesto na AKTIVNOM projektu.
        // Kada korisnik gleda svoj projekat koji je aktivan. (Autor projekta)
        async Task ShowModalAuthorActive()
        {
            // TODO something
        }

        // Metoda koja izbacuje prozor koji pita da li smo sigurni da zelimo da ponistimo prijavu na projekat. (ako je korisnik uopste prijavljen na dati projekat)
        // Kada korisnik gleda tudji projekat na koji se prethodno vec prijavio.
        async Task ShowCancelWarningVisitorActive()
        {
            var parameters = new ModalParameters();
            string Title = "Potvrda o poništavanju prijave";
            parameters.Add(nameof(Title), Title);
            var messageForm = Modal.Show<CancelConfirmation>(nameof(Title), parameters);
            var result = await messageForm.Result;

            // Ako je korisnik kliknuo button PRIHVATI, ulazi se u if.
            if (!result.Cancelled)
            {
                // revoke application
            }
        }

        // Metoda koja prikazuje prozor koji pita da li smo sigurni da zelimo da izbrisemo projekat.
        // Kada korisnik gleda projekat koji je on sam napravio. (Autor projekta)
        // Metoda je ista i za aktivan i za zatvoren projekat.
        async Task ShowProjectDeleteWarning()
        {
            var parameters = new ModalParameters();
            string Title = "Potvrda o brisanju projekta";
            parameters.Add(nameof(Title), Title);
            var messageForm = Modal.Show<CancelConfirmation>(nameof(Title), parameters);
            var result = await messageForm.Result;

            // Ako je korisnik kliknuo button PRIHVATI, ulazi se u if.
            if (!result.Cancelled)
            {
                await ApiService.DeleteAsync($"projects/{ProjectId}");
            }
        }

        async Task ShowReceivedApplicationsModal()
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
