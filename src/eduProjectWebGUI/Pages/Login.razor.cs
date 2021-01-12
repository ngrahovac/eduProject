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
        }

        public void RegisterToSystem()
        {
            Navigation.NavigateTo("account/register");
        }

        public async Task Login2()
        {
            var result = await AuthService.Login(loginInputModel);

            if (result.Successful)
                Navigation.NavigateTo("/");
            else
                Console.WriteLine("Error while trying to log in");
        }
    }
}
