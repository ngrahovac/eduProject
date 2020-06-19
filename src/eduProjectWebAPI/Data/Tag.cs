using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Data
{
    public partial class Tag
    {
        public Tag()
        {
            ProjectTag = new HashSet<ProjectTag>();
            UserTag = new HashSet<UserTag>();
        }

        public int TagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProjectTag> ProjectTag { get; set; }
        public virtual ICollection<UserTag> UserTag { get; set; }
    }
}
