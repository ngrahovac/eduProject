using Blazored.Modal;
using Blazored.Modal.Services;
using eduProjectModel.Display;
using eduProjectWebGUI.Services;
using eduProjectWebGUI.Shared;
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
                ProfileDisplayModel = await ApiService.GetAsync<ProfileDisplayModel>($"/users/{UserId}");
                imgUrl = ProfileDisplayModel.AccountPhoto != null ? (@"https://localhost:44345/Resources/Images/" + ProfileDisplayModel.AccountPhoto) : "Winston.jpg";

                //TODO: Add redirect if not found
            }
            catch (Exception ex)
            {
                NavigationManager.NavigateTo("/404");
            }

        }
    }
}
