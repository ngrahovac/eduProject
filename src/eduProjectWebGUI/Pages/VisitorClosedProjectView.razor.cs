using eduProjectModel.Display;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class VisitorClosedProjectView
    {
        [Parameter]
        public ProjectDisplayModel VisitorClosedProjectDisplayModel { get; set; }
    }
}
