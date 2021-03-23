using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eduProjectModel.Input
{
    public class UserProfileInputModel
    {
        public int UserId { get; set; }
        public UserAccountType UserAccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FacultyName { get; set; }
        public int? Cycle { get; set; }
        public string StudyProgramName { get; set; }
        public int? StudyYear { get; set; }
        public string StudyProgramSpecializationName { get; set; }
        public string StudyFieldName { get; set; }
        public AcademicRank AcademicRank { get; set; }

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
