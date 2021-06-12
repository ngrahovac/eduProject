using Blazored.Modal;
using Blazored.Modal.Services;
using eduProjectModel.Display;
using eduProjectWebGUI.Services;
using eduProjectWebGUI.Shared;
using eduProjectWebGUI.Utils;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class ProfileView
    {
        [Parameter]
        public int UserId { get; set; }

        private string imgUrl = "Winston.jpg";

        [Inject]
        ApiService ApiService { get; set; }

        [Inject]
        IModalService Modal { get; set; }

        public ProfileDisplayModel ProfileDisplayModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var response = await ApiService.GetAsync<ProfileDisplayModel>($"/users/{UserId}");
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
                    ProfileDisplayModel = response.Item1;
                    imgUrl = ProfileDisplayModel.AccountPhoto != null ? (@"https://localhost:44345/Resources/Images/" + ProfileDisplayModel.AccountPhoto) : "Winston.jpg";
                }
            }
            catch (Exception ex)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), "Desila se neočekivana greška. Molimo pokušajte kasnije.");
                Modal.Show<InfoPopup>("Obavještenje", parameters);
            }

        }
    }
}
