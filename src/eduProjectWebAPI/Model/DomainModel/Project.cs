using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class Project
    {
        public Project()
        {
            CollaboratorProfile = new HashSet<CollaboratorProfile>();
            ProjectCollaborator = new HashSet<ProjectCollaborator>();
            ProjectTag = new HashSet<ProjectTag>();
        }

        public int ProjectId { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public int? StudyFieldId { get; set; }
        public int ProjectStatusId { get; set; }
        public int UserId { get; set; }

        public virtual ProjectStatus ProjectStatus { get; set; }
        public virtual StudyField StudyField { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CollaboratorProfile> CollaboratorProfile { get; set; }
        public virtual ICollection<ProjectCollaborator> ProjectCollaborator { get; set; }
        public virtual ICollection<ProjectTag> ProjectTag { get; set; }
    }
}
