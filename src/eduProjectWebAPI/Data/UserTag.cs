using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Data
{
    public partial class UserTag
    {
        public int UserId { get; set; }
        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual UserSettings User { get; set; }
    }
}
