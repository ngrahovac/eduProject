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

        public FacultyMemberProfileDisplayModel()
        {

        }
        public static FacultyMemberProfileDisplayModel FromFacultyMemberProfile(FacultyMemberProfile profile)
        {
            FacultyMemberProfileDisplayModel model = new FacultyMemberProfileDisplayModel();

            model.FacultyName = profile.Faculty.Name;
            model.StudyFieldName = profile.StudyField.Name;

            return model;
        }
    }
}
