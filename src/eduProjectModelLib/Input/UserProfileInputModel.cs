using eduProjectModel.Display;
using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace eduProjectModel.Input
{
    public class UserProfileInputModel
    {
        public int UserId { get; set; }
        [Required]
        public UserAccountType UserAccountType { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FacultyName { get; set; }
        public int? Cycle { get; set; }
        public string StudyProgramName { get; set; }
        public int? StudyYear { get; set; }
        public string StudyProgramSpecializationName { get; set; }
        public string StudyFieldName { get; set; }
        public AcademicRank AcademicRank { get; set; }

        public UserProfileInputModel()
        {

        }

        public UserProfileInputModel(ProfileDisplayModel model)
        {
            UserAccountType = model.UserAccountType;
            FirstName = model.FirstName;
            LastName = model.LastName;
            FacultyName = model.FacultyName;

            if (UserAccountType is UserAccountType.Student)
            {
                Cycle = model.Cycle;
                StudyProgramName = model.StudyProgramName;
                StudyYear = model.StudyYear;
                StudyProgramSpecializationName = model.StudyProgramSpecializationName;
            }
            else if (UserAccountType is UserAccountType.FacultyMember)
            {
                AcademicRank = model.AcademicRank;
                StudyFieldName = model.StudyFieldName;
            }
        }

        public void Clear()
        {
            UserId = 0;
            UserAccountType = 0;
            AcademicRank = 0;
            FacultyName = StudyProgramName = StudyProgramSpecializationName = StudyFieldName = String.Empty;
            Cycle = StudyYear = null;
        }

        public void MapTo(Student student, Faculty faculty)
        {
            student.FirstName = FirstName;
            student.LastName = LastName;

            student.FacultyId = faculty.FacultyId;
            student.StudyYear = (int)StudyYear;

            var program = faculty.StudyPrograms.Where(sp => sp.Name == StudyProgramName).First();
            student.StudyProgramId = program.ProgramId;

            if (StudyProgramSpecializationName != null)
            {
                student.StudyProgramSpecializationId = program.StudyProgramSpecializations.Where(sps => sps.Name == StudyProgramSpecializationName)
                                                                                          .First()
                                                                                          .SpecializationId;
            }
            else
            {
                student.StudyProgramSpecializationId = null;
            }
        }

        public void MapTo(FacultyMember facultyMember, Faculty faculty)
        {
            facultyMember.FirstName = FirstName;
            facultyMember.LastName = LastName;

            facultyMember.FacultyId = faculty.FacultyId;
            facultyMember.StudyField = StudyField.fields.Where(sf => sf.Value.Name == StudyFieldName).First().Value;
            facultyMember.AcademicRank = AcademicRank;
        }
    }
}
