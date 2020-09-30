using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace eduProjectModel.Display
{
    public class ProfileDisplayModel
    {
        public bool IsDisplayPersonal { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }

        public string LinkedinProfile { get; set; }
        public string ResearchgateProfile { get; set; }
        public string Website { get; set; }

        public string AccountPhoto { get; set; }
        public string Cv { get; set; }

        public string FacultyName { get; set; }
        public int Cycle { get; set; }
        public string StudyProgramName { get; set; }
        public int StudyYear { get; set; }

        public ICollection<ProfileProjectPreviewDisplayModel> AuthoredProjectDisplayModels { get; set; } = new HashSet<ProfileProjectPreviewDisplayModel>();
        public ICollection<ProfileProjectPreviewDisplayModel> ProjectCollaborationsDisplayModels { get; set; } = new HashSet<ProfileProjectPreviewDisplayModel>();

        public ProfileDisplayModel()
        {

        }

        public ProfileDisplayModel(User user, bool isPersonalProfile, ICollection<Project> authoredProjects,
                                    ICollection<Project> projectCollaborations)
        {
            IsDisplayPersonal = isPersonalProfile;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PhoneNumber = user.PhoneNumber;

            if (authoredProjects != null)
            {
                foreach (var project in authoredProjects)
                    AuthoredProjectDisplayModels.Add(new ProfileProjectPreviewDisplayModel(project));
            }

            if (isPersonalProfile)
            {
                if (projectCollaborations != null)
                {
                    foreach (var project in projectCollaborations)
                        ProjectCollaborationsDisplayModels.Add(new ProfileProjectPreviewDisplayModel(project));
                }
                //Dugme za izmjenu?
            }
            else
                ProjectCollaborationsDisplayModels = null;
        }
    }
}
