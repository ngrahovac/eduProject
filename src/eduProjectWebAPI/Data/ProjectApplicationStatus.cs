using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Data
{
    public partial class ProjectApplicationStatus
    {
        public ProjectApplicationStatus()
        {
            ProjectApplication = new HashSet<ProjectApplication>();
        }

        public int ProjectApplicationStatusId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ProjectApplication> ProjectApplication { get; set; }
    }
}
