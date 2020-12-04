using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Input;

namespace eduProjectWebGUI.Pages
{
    public partial class Register
    {
        RegisterInputModel model = new RegisterInputModel();

        public async Task RegisterUser()
        {
            await ApiService.PostAsync("account/register", model);

            Navigation.NavigateTo("account/login");
            
        }
    }
}
