using eduProjectModel.Display;
using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace eduProjectModel.Input
{
    public class CollaboratorProfileInputModel
    {
        [Required(ErrorMessage = "Tip saradnika se mora odabrati.")]
        public CollaboratorProfileType CollaboratorProfileType { get; set; }
        public bool ExistingProfile { get; set; }

        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        public string ActivityDescription { get; set; }
        public string FacultyName { get; set; }
        public int? Cycle { get; set; }
        public string StudyProgramName { get; set; }
        public int? StudyYear { get; set; }
        public string StudyProgramSpecializationName { get; set; }
        public string StudyFieldName { get; set; }
        public bool ApplicationsOpen { get; set; } = true;

        public CollaboratorProfileInputModel()
        {

        }

        public void MapTo(StudentProfile profile, Faculty faculty)
        {
            profile.ApplicationsOpen = ApplicationsOpen;
            profile.Description = ActivityDescription;
            profile.StudyCycle = Cycle;
            profile.StudyYear = StudyYear;

            if (FacultyName != null)
            {
                profile.FacultyId = faculty.FacultyId;
                if (StudyProgramName != null)
                {
                    var program = faculty.StudyPrograms.Where(sp => sp.Name == StudyProgramName).First();
                    profile.StudyProgramId = program.ProgramId;

                    if (StudyProgramSpecializationName != null)
                    {
                        profile.StudyProgramSpecializationId = program.StudyProgramSpecializations.Where(sps => sps.Name == StudyProgramSpecializationName)
                                                                                                  .First()
                                                                                                  .SpecializationId;
                    }
                }
            }
        }

        public void MapTo(FacultyMemberProfile profile)
        {
            profile.Description = ActivityDescription;
            profile.ApplicationsOpen = ApplicationsOpen;
            if (StudyFieldName != null)
                profile.StudyField = StudyField.fields.Where(sf => sf.Value.Name == StudyFieldName).First().Value;
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
