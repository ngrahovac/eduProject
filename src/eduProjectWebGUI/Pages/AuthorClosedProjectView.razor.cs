using eduProjectModel.Display;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class AuthorClosedProjectView
    {
        [Parameter]
        public ProjectDisplayModel AuthorClosedProjectDisplayModel { get; set; }
    }
}
