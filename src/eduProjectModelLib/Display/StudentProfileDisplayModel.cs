using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectModel.Display
{
    public class StudentProfileDisplayModel : CollaboratorProfileDisplayModel
    {
        public string FacultyName { get; set; }
        public string StudyProgramName { get; set; }
        public string StudyProgramSpecializationName { get; set; }
        public int? StudyCycle { get; set; }
        public int? StudyYear { get; set; }

        public StudentProfileDisplayModel() { }

        public StudentProfileDisplayModel(StudentProfile profile, Faculty faculty) : base(profile)
        {
            CollaboratorProfileId = profile.CollaboratorProfileId;
            StudyCycle = profile.StudyCycle;
            StudyYear = profile.StudyYear;

            if (profile.FacultyId != null)
            {
                FacultyName = faculty.Name;

                if (profile.StudyProgramId != null)
                {
                    StudyProgram program = faculty.StudyPrograms.Where(p => p.ProgramId == profile.StudyProgramId).First();
                    StudyProgramName = program.Name;

                    if (profile.StudyProgramSpecializationId != null)
                    {
                        StudyProgramSpecializationName = program.StudyProgramSpecializations
                                                                      .Where(s => s.SpecializationId == profile.StudyProgramSpecializationId)
                                                                      .First().Name;
                    }
                }
            }
        }

        public static StudentProfileDisplayModel FromStudentProfile(StudentProfile profile, Faculty faculty)
        {
            StudentProfileDisplayModel model = new StudentProfileDisplayModel
            {
                StudyCycle = profile.StudyCycle,
                StudyYear = profile.StudyYear
            };

            if (profile.FacultyId != null)
            {
                model.FacultyName = faculty.Name;

                if (profile.StudyProgramId != null)
                {
                    StudyProgram program = faculty.StudyPrograms.Where(p => p.ProgramId == profile.StudyProgramId).First();
                    model.StudyProgramName = program.Name;

                    if (profile.StudyProgramSpecializationId != null)
                    {
                        model.StudyProgramSpecializationName = program.StudyProgramSpecializations
                                                                      .Where(s => s.SpecializationId == profile.StudyProgramSpecializationId)
                                                                      .First().Name;
                    }
                }
            }

            return model;
        }
    }
}
