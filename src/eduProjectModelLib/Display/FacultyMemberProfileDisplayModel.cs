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
        public string StudyFieldName { get; set; }

        public FacultyMemberProfileDisplayModel() { }

        public FacultyMemberProfileDisplayModel(FacultyMemberProfile profile) : base(profile)
        {
            CollaboratorProfileId = profile.CollaboratorProfileId;
            StudyFieldName = profile.StudyField?.Name;
        }
    }
}
