using Blazored.Modal;
using eduProjectModel.Input;
using eduProjectWebGUI.Shared;
using eduProjectWebGUI.Utils;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class LoginPage
    {
        LoginInputModel loginInputModel = new LoginInputModel();

        protected override async Task OnInitializedAsync()
        {
            var token = await LocalStorage.GetItemAsync<string>("authToken");
            if (token != null)
            {
                var claims = ExtensionMethods.ParseClaimsFromJwt(token);
                if (claims.First(c => c.Type == ClaimTypes.Role).Value == "Admin")
                    Navigation.NavigateTo("/admin/accounts");
                else
                    Navigation.NavigateTo("/news");
            }
        }

        public async Task Login2()
        {
            try
            {
                var result = await AuthService.Login(loginInputModel);

                if (result.Successful)
                {
                    var token = result.Token;
                    var claims = ExtensionMethods.ParseClaimsFromJwt(token);
                    if (claims.First(c => c.Type == ClaimTypes.Role).Value == "Admin")
                        Navigation.NavigateTo("/admin/accounts");
                    else
                        Navigation.NavigateTo("/news");
                }
                else
                {
                    var parameters = new ModalParameters();
                    parameters.Add(nameof(InfoPopup.Message), result.Error);
                    Modal.Show<InfoPopup>("Obavještenje", parameters);
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
