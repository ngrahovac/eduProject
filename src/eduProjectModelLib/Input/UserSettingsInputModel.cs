using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eduProjectModel.Input
{
    public class UserSettingsInputModel
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
        public ICollection<string> UserTagNames { get; set; } = new HashSet<string>();

        public void MapTo(UserSettings settings)
        {
            settings.UserId = UserId;
            settings.EmailVisible = EmailVisible;
            settings.PhoneVisible = PhoneVisible;
            settings.ProjectsVisible = ProjectsVisible;
            settings.LinkedinProfile = LinkedinProfile;
            settings.ResearchgateProfile = ResearchgateProfile;
            settings.Website = Website;
            settings.CvLink = CvLink;
            settings.PhotoLink = PhotoLink;
            settings.Bio = Bio;

            HashSet<Tag> tags = new HashSet<Tag>();
            foreach (var name in UserTagNames)
            {
                tags.Add(Tag.tags.Where(i => i.Value.Name == name).First().Value);
            }

            settings.UserTags = tags;
        }
    }
}
