using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Data
{
    public partial class CollaboratorProfile
    {
        public CollaboratorProfile()
        {
            ProjectApplication = new HashSet<ProjectApplication>();
        }

        public int CollaboratorProfileId { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }
        public int UserAccountTypeId { get; set; }

        public virtual Project Project { get; set; }
        public virtual UserAccountType UserAccountType { get; set; }
        public virtual FacultyMemberProfile FacultyMemberProfile { get; set; }
        public virtual StudentProfile StudentProfile { get; set; }
        public virtual ICollection<ProjectApplication> ProjectApplication { get; set; }
    }
}
