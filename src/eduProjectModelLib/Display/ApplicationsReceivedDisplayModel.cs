using System;
using System.Collections.Generic;
using System.Text;

namespace eduProjectModel.Display
{
    public class ApplicationsReceivedDisplayModel
    {
            public string Title { get; set; }
            public ICollection<StudentProfileDisplayModel> StudentProfileDisplayModels { get; set; } = new HashSet<StudentProfileDisplayModel>();
            public ICollection<FacultyMemberProfileDisplayModel> FacultyMemberProfileDisplayModels { get; set; } = new HashSet<FacultyMemberProfileDisplayModel>();
            public ICollection<CollaboratorDisplayModel> CollaboratorDisplayModels { get; set; } = null;
    }
}
