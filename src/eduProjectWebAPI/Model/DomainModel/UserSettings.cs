using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class UserSettings
    {
        public UserSettings()
        {
            UserTag = new HashSet<UserTag>();
        }

        public sbyte IsEmailVisible { get; set; }
        public sbyte IsPhoneVisible { get; set; }
        public sbyte AreProjectsVisible { get; set; }
        public string LinkedinProfile { get; set; }
        public string ResearchgateProfile { get; set; }
        public string Website { get; set; }
        public string Cv { get; set; }
        public string AccountPhoto { get; set; }
        public string Bio { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<UserTag> UserTag { get; set; }
    }
}
