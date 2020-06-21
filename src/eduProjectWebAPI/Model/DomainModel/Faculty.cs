using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class Faculty
    {
        public Faculty()
        {
            FacultyMember = new HashSet<FacultyMember>();
            FacultyMemberProfile = new HashSet<FacultyMemberProfile>();
            StudentProfile = new HashSet<StudentProfile>();
            StudyProgram = new HashSet<StudyProgram>();
        }

        public int FacultyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<FacultyMember> FacultyMember { get; set; }
        public virtual ICollection<FacultyMemberProfile> FacultyMemberProfile { get; set; }
        public virtual ICollection<StudentProfile> StudentProfile { get; set; }
        public virtual ICollection<StudyProgram> StudyProgram { get; set; }
    }
}
