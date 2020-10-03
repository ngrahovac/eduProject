using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Display;

namespace eduProjectWebGUI.Pages
{
    public partial class ApplicationsReceived
    {
        [Parameter]
        public ApplicationsReceivedDisplayModel ApplicationsReceivedDisplayModel { get; set; }


        public void Close()
        {

        }
    }
}
