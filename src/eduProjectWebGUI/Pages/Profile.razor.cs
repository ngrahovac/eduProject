using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class Profile
    {
        private string bckgrndColorEmail = "green";
        private string bckgrndColorNumber = "green";
        private string bckgrndColorProjects = "green";
        private bool btnCheckEmail = false;
        private bool btnCheckNumber = false;
        private bool btnCheckProjects = false;

        public void ChangeEmailVisibility()
        {
            bckgrndColorEmail = btnCheckEmail ? "green" : "red";
            btnCheckEmail = btnCheckEmail ? false : true;
        }

        public void ChangeNumberVisibility()
        {
            bckgrndColorNumber = btnCheckNumber ? "green" : "red";
            btnCheckNumber = btnCheckNumber ? false : true;
        }

        public void ChangeProjectsVisibility()
        {
            bckgrndColorProjects = btnCheckProjects ? "green" : "red";
            btnCheckProjects = btnCheckProjects ? false : true;
        }

        private void SaveBiography(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void AddTag(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SaveProfile(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void CancleSavingProfile(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
