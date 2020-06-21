using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class User
    {
        public User()
        {
            Project = new HashSet<Project>();
            ProjectApplication = new HashSet<ProjectApplication>();
            ProjectCollaborator = new HashSet<ProjectCollaborator>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneFormat { get; set; }

        public virtual Account Account { get; set; }
        public virtual FacultyMember FacultyMember { get; set; }
        public virtual Student Student { get; set; }
        public virtual UserSettings UserSettings { get; set; }
        public virtual ICollection<Project> Project { get; set; }
        public virtual ICollection<ProjectApplication> ProjectApplication { get; set; }
        public virtual ICollection<ProjectCollaborator> ProjectCollaborator { get; set; }
    }
}
