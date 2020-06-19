using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Data
{
    public partial class StudyField
    {
        public StudyField()
        {
            FacultyMember = new HashSet<FacultyMember>();
            FacultyMemberProfile = new HashSet<FacultyMemberProfile>();
            Project = new HashSet<Project>();
        }

        public int StudyFieldId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<FacultyMember> FacultyMember { get; set; }
        public virtual ICollection<FacultyMemberProfile> FacultyMemberProfile { get; set; }
        public virtual ICollection<Project> Project { get; set; }
    }
}
