using eduProjectModel.Display;
using eduProjectWebGUI.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class AuthorActiveProjectView
    {
        [Parameter]
        public ProjectDisplayModel AuthorActiveProjectDisplayModel { get; set; }
    }
}
