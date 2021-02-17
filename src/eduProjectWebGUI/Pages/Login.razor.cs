﻿using eduProjectModel.Input;
using eduProjectWebGUI.Utils;
using System;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class Login
    {
        LoginInputModel loginInputModel = new LoginInputModel();

        protected override async Task OnInitializedAsync()
        {
            var result = await LocalStorage.ExtractUserId();
            if (result != null)
                Navigation.NavigateTo("/homepage");
        }

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
                Navigation.NavigateTo("/homepage");
            else
                Console.WriteLine("Error while trying to log in");
        }
    }
}
