using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class ProjectStatus
    {
        public ProjectStatus()
        {
            Project = new HashSet<Project>();
        }

        public int ProjectStatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Project> Project { get; set; }
    }
}
