using eduProjectModel.Display;
using eduProjectWebGUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class Profile
    {
        [Parameter]
        public int UserId { get; set; }

        [Inject]
        ApiService ApiService { get; set; }

        public ProfileDisplayModel ProfileDisplayModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ProfileDisplayModel = await ApiService.GetAsync<ProfileDisplayModel>($"/users/{UserId}");
        }
    }
}
