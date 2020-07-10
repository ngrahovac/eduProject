using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Model.Display
{
    public class StudentProfileDisplayModel : CollaboratorProfileDisplayModel
    {
        public string FacultyName { get; private set; }
        public string StudyProgramName { get; private set; }
        public string StudyProgramSpecializationName { get; private set; }
        public int? StudyCycle { get; private set; }
        public int? StudyYear { get; private set; }

        public StudentProfileDisplayModel()
        {

        }
        public StudentProfileDisplayModel(string facultyName, string studyProgramName, string studyProgramSpecializationName,
                                          int? studyCycle, int? studyYear, string description)
        {
            Description = description;
            FacultyName = facultyName;
            StudyProgramName = studyProgramName;
            StudyProgramSpecializationName = studyProgramSpecializationName;
            StudyCycle = studyCycle;
            StudyYear = studyYear;
        }

        public static StudentProfileDisplayModel FromStudentProfile(StudentProfile profile)
        {
            StudentProfileDisplayModel model = new StudentProfileDisplayModel();
            model.FacultyName = profile.Faculty.Name;
            model.StudyProgramName = profile.StudyProgram.Name;
            model.StudyProgramSpecializationName = profile.StudyProgramSpecialization.Name;
            model.StudyCycle = profile.StudyCycle;
            model.StudyYear = profile.StudyYear;

            return model;
        }
    }
}
