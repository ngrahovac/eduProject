using System.Collections.Generic;

namespace eduProjectModel.Domain
{
    public class UserSettings : IValueObject
    {
        public int UserId { get; set; }
        public bool EmailVisible { get; set; } = false;
        public bool PhoneVisible { get; set; } = false;
        public bool ProjectsVisible { get; set; } = false;
        public string LinkedinProfile { get; set; }
        public string ResearchgateProfile { get; set; }
        public string Website { get; set; }
        public string CvLink { get; set; }
        public string PhotoLink { get; set; }
        public string Bio { get; set; }
        public ICollection<Tag> UserTags { get; set; } = new HashSet<Tag>();

    }
}
