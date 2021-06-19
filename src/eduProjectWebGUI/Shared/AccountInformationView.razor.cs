using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using eduProjectModel.Input;
using eduProjectWebGUI.Utils;
using Microsoft.AspNetCore.Components;

namespace eduProjectWebGUI.Shared
{
    public partial class AccountInformationView
    {
        [Parameter]
        public RegisterInputModel RegisterInputModel { get; set; }

        [Parameter]
        public int UserId { get; set; }


        private async Task UpdateAccountInformation()
        {
            try
            {
                var response = await ApiService.PutAsync($"/account/{UserId}/credentials", RegisterInputModel);
                var parameters = new ModalParameters();
                parameters.Add(nameof(InfoPopup.Message), response.StatusCode.GetMessage());
                Modal.Show<InfoPopup>("Obavještenje", parameters);
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
