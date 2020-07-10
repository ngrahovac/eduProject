using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Model.Display
{
    public class FacultyMemberProfileDisplayModel : CollaboratorProfileDisplayModel
    {
        public string FacultyName { get; private set; }
        public string StudyFieldName { get; private set; }

        public FacultyMemberProfileDisplayModel()
        {

        }
        public FacultyMemberProfileDisplayModel(string academicRank, string facultyName, string studyFieldName, string description)
        {
            Description = description;
            FacultyName = facultyName;
            StudyFieldName = studyFieldName;
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
