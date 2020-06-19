using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Data
{
    public partial class ProjectTag
    {
        public int ProjectId { get; set; }
        public int TagId { get; set; }

        public virtual Project Project { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
