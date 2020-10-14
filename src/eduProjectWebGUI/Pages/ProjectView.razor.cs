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
                // SLANJE PRIJAVE
                // ako korisnik potvrdi da salje prijavu, mi treba da popunimo ProjectInputModel sa podacima i to posaljemo kontroleru za prijave
                // API poziv izgleda ovako: 
                // await ApiService.PostAsync("applications", model);
                // gdje je model ProjectApplicationInputModel - popunjen podacima od klijenta pri slanju prijave
                // pogledaj u klasi eduProjectModel, van svih foldera se nalazi
                // ApplicantComment treba da se popuni komentarom prijavljenog koji se nalazi u onom modal prozoru
                // ProjectId se puni odozgo sa vrha, ovaj isti fajl
                // CollaboratorProfileType se puni na osnovu stringa - vidi u VisitorActive razor fajlu, u zavisnosti od kliknute tabele se zna koji je tip
                // CollaboratorProfileIndex vidi u istom fajlu
                // dakle problem je sto nam se podaci koje trebamo imati ovdje nalaze u 3 4 fajla... kad bi sve bilo jedna ogromna forma (ProjectView.razor)
                // onda bi nam sve bilo na jednom mjestu, pa da znas da mozes sve u jednu formu staviti na kraju krajeva da se ne zezas sa komunikacijom izmedju formi
                // ali i to ima na tutorijalu

                // e da, ima jos jedna stvar
                // kod tebe kad se korisnik prijavi, automatski se ukljuci cancel dugme a prijavi se dugme je ugaseno
                // sto sprjecava osobu da izvrsi nekoliko prijava na nekoliko razlicitih profila
                // moj prijedlog je da se ta polja ne aktiviraju / deaktiviraju uopste
                // nego da se lijepo sa API strane vrati odgovarajuca poruka, recimo 404, ako je prijava vec napravljena na taj profil
                // ili ako se pokusalo odustati od prijave na profil na koji se osoba nije ni prijavila
                // tako da se API brine o tome, a ne ti da podesavas dugmice
                // jer da bi podesavao dugmice, moras na blazor strani voditi racuna o tome na sta se sve korisnik isprijavljivao sto mi se cini komplikovano

            }
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
