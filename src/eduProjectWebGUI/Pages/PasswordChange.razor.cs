using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Pages
{
    public partial class PasswordChange
    {
        public string TxtType1 = "password";
        public string ShowAttribute1 = "Prikaži";

        public string TxtType2 = "password";
        public string ShowAttribute2 = "Prikaži";

        public string TxtType3 = "password";
        public string ShowAttribute3 = "Prikaži";

        public void ShowPassword1()
        {
            if (this.TxtType1 == "password")
            {
                this.TxtType1 = "text";
                this.ShowAttribute1 = "Sakrij";
            }
            else
            {
                this.TxtType1 = "password";
                this.ShowAttribute1 = "Prikaži";
            }
        }

        public void ShowPassword2()
        {
            if (this.TxtType2 == "password")
            {
                this.TxtType2 = "text";
                this.ShowAttribute2 = "Sakrij";
            }
            else
            {
                this.TxtType2 = "password";
                this.ShowAttribute2 = "Prikaži";
            }
        }

        public void ShowPassword3()
        {
            if (this.TxtType3 == "password")
            {
                this.TxtType3 = "text";
                this.ShowAttribute3 = "Sakrij";
            }
            else
            {
                this.TxtType3 = "password";
                this.ShowAttribute3 = "Prikaži";
            }
        
        }
    }
}
