using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class StudyProgramSpecialization
    {
        public StudyProgramSpecialization()
        {
            Student = new HashSet<Student>();
            StudentProfile = new HashSet<StudentProfile>();
        }

        public int StudyProgramSpecializationId { get; set; }
        public string Name { get; set; }
        public int StudyProgramId { get; set; }

        public virtual StudyProgram StudyProgram { get; set; }
        public virtual ICollection<Student> Student { get; set; }
        public virtual ICollection<StudentProfile> StudentProfile { get; set; }
    }
}
