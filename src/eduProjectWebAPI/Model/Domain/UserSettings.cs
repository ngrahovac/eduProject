using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public class UserSettings : IValueObject // jer nam je bitna samo vrijednost podesavanja a ne lifetime, nema identitet
    {
        public bool IsEmailVisible { get; private set; } = false;
        public bool IsPhoneVisible { get; private set; } = false;
        public bool AreProjectsVisible { get; private set; } = false;
        public string LinkedinProfile { get; private set; }
        public string ResearchgateProfile { get; private set; }
        public string Website { get; private set; }
        public string Cv { get; private set; }
        public string AccountPhoto { get; private set; }
        public string Bio { get; private set; }
        public ICollection<Tag> UserTags { get; private set; }
        public UserSettings(bool isEmailVisible, bool isPhoneVisible, bool areProjectsVisible, string linkedinProfile,
                            string researchgateProfile, string website, string cv, string accountPhoto, string bio, User user)
        {
            IsEmailVisible = isEmailVisible;
            IsPhoneVisible = isPhoneVisible;
            AreProjectsVisible = areProjectsVisible;
            LinkedinProfile = linkedinProfile;
            ResearchgateProfile = researchgateProfile;
            Website = website;
            Cv = cv;
            AccountPhoto = accountPhoto;
            Bio = bio;
            // User = user;

            UserTags = new HashSet<Tag>();
        }


    }
}
