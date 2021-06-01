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



        public List<ProjectApplicationsDisplayModel> ProjectApplicationsDisplayModels { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await ApiService.GetAsync<ICollection<ProjectApplicationsDisplayModel>>($"/applications/applicant/{UserId}");
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
                    /*ProjectApplicationsDisplayModels = (await ApiService.GetAsync<ICollection<ProjectApplicationsDisplayModel>>($"/applications/applicant/{UserId}")).Item1.ToList();
                    sentNotifications = (await ApiService.GetAsync<ICollection<int>>($"notifications/user/{UserId}/applications")).Item1;
                    //await ApiService.DeleteAsync($"notifications/user/{UserId}/applications"); - vec bilo zakomentarisano

                    foreach (var model in ProjectApplicationsDisplayModels)
                    {
                        model.CollaboratorProfileApplicationsDisplayModels = model.CollaboratorProfileApplicationsDisplayModels
                                                                                  .Where(m => m.ApplicationDisplayModels.Select(a => a.ApplicantId)
                                                                                  .Contains(UserId)).ToList();
                    }*/
                    var responseDisplayModels = await ApiService.GetAsync<ICollection<ProjectApplicationsDisplayModel>>($"/applications/applicant/{UserId}");
                    var responseDisplayModelsCode = responseDisplayModels.Item2;

                    var responseSentNotifications = await ApiService.GetAsync<ICollection<int>>($"notifications/user/{UserId}/applications");
                    var responseSentNotificationsCode = responseSentNotifications.Item2;

                    if (!responseDisplayModelsCode.IsSuccessCode())
                    {
                        if (responseDisplayModelsCode.ShouldRedirectTo404())
                            NavigationManager.NavigateTo("/404");
                        else
                        {
                            var parameters = new ModalParameters();
                            parameters.Add(nameof(InfoPopup.Message), responseDisplayModelsCode.GetMessage());
                            Modal.Show<InfoPopup>("Obavještenje", parameters);
                        }
                    }
                    else
                    {
                        ProjectApplicationsDisplayModels = responseDisplayModels.Item1.ToList();

                        foreach (var model in ProjectApplicationsDisplayModels)
                        {
                            model.CollaboratorProfileApplicationsDisplayModels = model.CollaboratorProfileApplicationsDisplayModels
                                                                                      .Where(m => m.ApplicationDisplayModels.Select(a => a.ApplicantId)
                                                                                      .Contains(UserId)).ToList();
                        }
                    }

                    if (!responseSentNotificationsCode.IsSuccessCode())
                    {
                        var parameters = new ModalParameters();
                        parameters.Add(nameof(InfoPopup.Message), responseSentNotificationsCode.GetMessage());
                        Modal.Show<InfoPopup>("Obavještenje", parameters);
                    }
                    else
                        sentNotifications = responseSentNotifications.Item1;
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
                        Modal.Show<InfoPopup>("Obavještenje", parameters2);

                        if (response.IsSuccessStatusCode)
                        {
                            // Navigation
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
