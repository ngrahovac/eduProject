using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using eduProjectModel.Display;
using eduProjectWebGUI.Shared;
using eduProjectWebGUI.Utils;
using Microsoft.AspNetCore.Components;

namespace eduProjectWebGUI.Pages
{
    public partial class SentApplicationsOverview
    {
        [Parameter] public int UserId { get; set; }

        private int selectedApplicationId = 0;

        private HashSet<string> allTables = new HashSet<string>();
        private List<string> tablesToClear = new List<string>();
        private ICollection<int> sentNotifications = new List<int>();

        public List<ProjectApplicationsDisplayModel> ProjectApplicationsDisplayModels;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await ApiService.GetAsync<ICollection<ProjectApplicationsDisplayModel>>($"applications/applicant/{UserId}");
                sentNotifications = (await ApiService.GetAsync<ICollection<int>>($"notifications/user/{UserId}/applications")).Item1;
                await ApiService.DeleteAsync($"/notifications/user/{UserId}/applications");
                var code = response.Item2;

                if (!code.IsSuccessCode())
                {
                    if (code.ShouldRedirectTo404())
                        NavigationManager.NavigateTo("/404");

                    else
                    {
                        var parameters = new ModalParameters();
                        parameters.Add(nameof(InfoPopup.Message), code.GetMessage());
                        Modal.Show<InfoPopup>("Obavještenje", parameters);
                    }
                }
                else
                {
                    ProjectApplicationsDisplayModels = response.Item1.ToList();
                    Console.WriteLine($"Ima {ProjectApplicationsDisplayModels.Count()} prijava");
                    foreach (var model in ProjectApplicationsDisplayModels)
                    {
                        model.CollaboratorProfileApplicationsDisplayModels = model.CollaboratorProfileApplicationsDisplayModels
                                                                                  .Where(m => m.ApplicationDisplayModels.Select(a => a.ApplicantId)
                                                                                  .Contains(UserId)).ToList();
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

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            if (firstRender)
            {
                selectedApplicationId = 0;
            }
        }

        private async Task RevokeApplication()
        {
            if (selectedApplicationId != 0)
            {
                var parameters = new ModalParameters();
                string Title = "Potvrda o povlačenju prijave";
                parameters.Add(nameof(Title), Title);
                var messageForm = Modal.Show<ActionConfirmationPopup>(nameof(Title), parameters);
                var result = await messageForm.Result;

                if (!result.Cancelled)
                {
                    try
                    {
                        var response = await ApiService.DeleteAsync($"/applications/{selectedApplicationId}");
                        var parameters2 = new ModalParameters();
                        parameters2.Add(nameof(InfoPopup.Message), response.StatusCode.GetMessage());

                        var msgForm = Modal.Show<InfoPopup>("Obavještenje", parameters2);
                        var resForm = await msgForm.Result;

                        if (response.IsSuccessStatusCode && !resForm.Cancelled)
                        {
                            NavigationManager.NavigateTo($"users/{UserId}/applications", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        var parameters2 = new ModalParameters();
                        parameters2.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                        Modal.Show<InfoPopup>("Obavještenje", parameters2);
                    }
                    //NavigationManager.NavigateTo($"/users/{UserId}/applications", true);
                }
            }
        }
    }
}
