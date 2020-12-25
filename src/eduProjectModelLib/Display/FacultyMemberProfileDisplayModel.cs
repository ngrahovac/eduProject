using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace eduProjectModel.Display
{
    public class FacultyMemberProfileDisplayModel : CollaboratorProfileDisplayModel
    {
        public string FacultyName { get; set; }
        public string StudyFieldName { get; set; }

        public FacultyMemberProfileDisplayModel() { }

        public FacultyMemberProfileDisplayModel(FacultyMemberProfile profile, Faculty faculty) : base(profile)
        {
            CollaboratorProfileId = profile.CollaboratorProfileId;
            FacultyName = profile.FacultyId != null ? faculty.Name : null;
            StudyFieldName = profile.StudyField != null ? profile.StudyField.Name : null;
        }
    }
}
