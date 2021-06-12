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
            FacultyName = profile.FacultyId != null ? faculty.Name : null;
            StudyFieldName = profile.StudyField?.Name;
        }

        public override string ToString()
        {
            string result = "Tip profila: nastavno osoblje, ";
            if (FacultyName != null)
            {
                result += $"fakultet: {FacultyName}, ";
                if (StudyFieldName != null)
                {
                    result += $"uža naučna oblast: {StudyFieldName }, ";
                }
            }

            return result[0..^2];
        }
    }
}
