using System;
using System.Collections.Generic;
using System.Text;
using eduProjectModel.Domain;

namespace eduProjectModel.Display
{
    public class ApplicantProfileDisplayModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public bool CheckedForCollaboration { get; set; } = false;
        public ApplicantProfileDisplayModel() { }
        
    }
}
