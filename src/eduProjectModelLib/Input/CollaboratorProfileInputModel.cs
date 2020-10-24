using eduProjectModel.Display;
using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eduProjectModel.Input
{
    public class CollaboratorProfileInputModel
    {
        public CollaboratorProfileType CollaboratorProfileType { get; set; }
        public bool AddedOnCreate { get; set; }
        public string ActivityDescription { get; set; }
        public string FacultyName { get; set; }
        public int? Cycle { get; set; }
        public string StudyProgramName { get; set; }
        public int? StudyYear { get; set; }
        public string StudyProgramSpecializationName { get; set; }
        public string StudyFieldName { get; set; }

        public CollaboratorProfileInputModel()
        {

        }

        public void MapTo(StudentProfile profile, Faculty faculty)
        {
            profile.Description = ActivityDescription;
            profile.StudyCycle = Cycle;
            profile.StudyYear = StudyYear;

            foreach (var program in faculty.StudyPrograms)
            {
                if (program.Name.Equals(StudyProgramName))
                {
                    profile.StudyProgramId = program.ProgramId;

                    foreach (var specialization in program.StudyProgramSpecializations)
                    {
                        if (specialization.Name.Equals(StudyProgramSpecializationName))
                            profile.StudyProgramSpecializationId = specialization.SpecializationId;
                    }
                }
            }
        }

        public void MapTo(FacultyMemberProfile profile)
        {
            profile.Description = ActivityDescription;
            profile.StudyField.Name = StudyFieldName;
        }

        public static CollaboratorProfileInputModel FromCollaboratorProfileDisplayModel(CollaboratorProfileDisplayModel profile)
        {

            if (profile is StudentProfileDisplayModel sp)
            {
                return new CollaboratorProfileInputModel
                {
                    CollaboratorProfileType = CollaboratorProfileType.Student,
                    ActivityDescription = profile.Description,
                    FacultyName = sp.FacultyName,
                    Cycle = sp.StudyCycle,
                    StudyProgramName = sp.StudyProgramName,
                    StudyProgramSpecializationName = sp.StudyProgramSpecializationName,
                    StudyYear = sp.StudyYear
                };

            }

            else if (profile is FacultyMemberProfileDisplayModel fp)
            {
                return new CollaboratorProfileInputModel
                {
                    CollaboratorProfileType = CollaboratorProfileType.FacultyMember,
                    ActivityDescription = profile.Description,
                    FacultyName = fp.FacultyName,
                    StudyFieldName = fp.StudyFieldName
                };
            }

            throw new ArgumentException();
        }
    }
}
