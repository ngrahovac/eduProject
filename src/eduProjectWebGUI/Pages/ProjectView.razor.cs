using eduProjectWebGUI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class ProjectView
    {
        void ShowModal()
        {
            Modal.Show<LeaveComment>();
        }
    }
}
