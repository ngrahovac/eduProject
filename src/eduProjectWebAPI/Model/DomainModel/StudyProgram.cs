using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class StudyProgram
    {
        public StudyProgram()
        {
            Student = new HashSet<Student>();
            StudentProfile = new HashSet<StudentProfile>();
            StudyProgramSpecialization = new HashSet<StudyProgramSpecialization>();
        }

        public int StudyProgramId { get; set; }
        public string Name { get; set; }
        public sbyte Cycle { get; set; }
        public sbyte DurationYears { get; set; }
        public int FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Student> Student { get; set; }
        public virtual ICollection<StudentProfile> StudentProfile { get; set; }
        public virtual ICollection<StudyProgramSpecialization> StudyProgramSpecialization { get; set; }
    }
}
