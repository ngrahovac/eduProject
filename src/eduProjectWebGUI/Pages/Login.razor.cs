using eduProjectModel.Input;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class Login
    {

        LoginInputModel loginInputModel = new LoginInputModel();

        public async Task LoginToSystemAsync()
        {
            await ApiService.PostAsync("account/login", loginInputModel);
            
            // Redirect to Home Page of logged user.
        }

        public void RegisterToSystem()
        {
            Navigation.NavigateTo("account/register");
        }
    }
}
