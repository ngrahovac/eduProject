using eduProjectModel.Display;
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

        public UserSettingsInputModel()
        {

        }

        public UserSettingsInputModel(UserSettingsDisplayModel model)
        {
            UserId = model.UserId;
            EmailVisible = model.EmailVisible;
            PhoneVisible = model.PhoneVisible;
            ProjectsVisible = model.ProjectsVisible;
            LinkedinProfile = model.LinkedinProfile;
            ResearchgateProfile = model.ResearchgateProfile;
            Website = model.Website;
            CvLink = model.CvLink;
            PhotoLink = model.PhotoLink;
            Bio = model.Bio;
            UserTagNames = model.UserTags.Select(t => t.Name).ToList();
        }

        public void MapTo(UserSettings model)
        {
            model.UserId = UserId;
            model.EmailVisible = EmailVisible;
            model.PhoneVisible = PhoneVisible;
            model.ProjectsVisible = ProjectsVisible;
            model.LinkedinProfile = LinkedinProfile;
            model.ResearchgateProfile = ResearchgateProfile;
            model.Website = Website;
            model.CvLink = CvLink;
            model.PhotoLink = PhotoLink;
            model.Bio = Bio;

            HashSet<Tag> tags = new HashSet<Tag>();
            foreach (var name in UserTagNames)
            {
                tags.Add(Tag.tags.Where(i => i.Value.Name == name).First().Value);
            }

            model.UserTags = tags;
        }


    }
}
