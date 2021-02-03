using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Shared
{
    public partial class InfoPopup
    {
        public static readonly string messageSuccess = "Akcija uspiješno izvršena.";
        public static readonly string messageFailure = "Akcija nije uspiješno izvršena. Molimo pokušajte kasnije.";

        [CascadingParameter]
        BlazoredModalInstance BlazoredModal { get; set; }

        [Parameter]
        public string Message { get; set; }

        void Ok()
        {
            BlazoredModal.Close();
        }
    }
}
