using eduProjectModel.Display;
using eduProjectWebGUI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eduProjectModel.Domain;
using eduProjectWebGUI.Shared;
using System.Diagnostics.Tracing;
using System.Net.Http;
using System.Net.Http.Json;


namespace eduProjectWebGUI.Pages
{
    public partial class VisitorActiveProjectView
    {
        [Parameter]
        public VisitorActiveProjectDisplayModel VisitorActiveProjectDisplayModel { get; set; }

    }
}
