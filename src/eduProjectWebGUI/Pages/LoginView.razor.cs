﻿using eduProjectModel.Input;
using eduProjectWebGUI.Utils;
using System;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class LoginView
    {
        LoginInputModel loginInputModel = new LoginInputModel();

        protected override async Task OnInitializedAsync()
        {
            var result = await LocalStorage.ExtractUserId();
            if (result != null)
                Navigation.NavigateTo("/news");
        }

        public async Task LoginToSystemAsync()
        {
            await ApiService.PostAsync("account/login", loginInputModel);
        }

        public async Task Login2()
        {
            var result = await AuthService.Login(loginInputModel);

            if (result.Successful)
                Navigation.NavigateTo("/news");
            else
                Console.WriteLine(result.Error);
        }
    }
}
