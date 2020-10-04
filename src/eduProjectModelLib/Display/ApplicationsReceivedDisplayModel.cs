using System;
using System.Collections.Generic;
using System.Text;

namespace eduProjectModel.Display
{
    public class ApplicationsReceivedDisplayModel
    {
            public string Title { get; set; }
            public ICollection<ApplicantProfileDisplayModel> ApplicantProfileDisplayModel { get; set; } = new HashSet<ApplicantProfileDisplayModel>();
    }
}
