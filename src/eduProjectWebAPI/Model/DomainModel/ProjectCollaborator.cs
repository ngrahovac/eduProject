using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class ProjectCollaborator
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
