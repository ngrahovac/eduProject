using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace eduProjectModel.Display
{
    public class UserSettingsDisplayModel
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

        public UserSettingsDisplayModel()
        {

        }
        public UserSettingsDisplayModel(UserSettings settings)
        {
            UserId = settings.UserId;
            EmailVisible = settings.EmailVisible;
            PhoneVisible = settings.PhoneVisible;
            ProjectsVisible = settings.ProjectsVisible;
            LinkedinProfile = settings.LinkedinProfile;
            ResearchgateProfile = settings.ResearchgateProfile;
            Website = settings.Website;
            CvLink = settings.CvLink;
            PhotoLink = settings.PhotoLink;
            Bio = settings.Bio;
            UserTags = settings.UserTags;
        }
    }
}
