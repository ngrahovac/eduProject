using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eduProjectModel.Display
{
    public class ProfileDisplayModel
    {
        public bool IsDisplayPersonal { get; set; }

        public UserAccountType UserAccountType { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool EmailVisible { get; set; }
        public string Email { get; set; }

        public bool PhoneNumberVisible { get; set; }
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

        public string StudyProgramSpecializationName { get; set; }
        public int StudyYear { get; set; }

        public string StudyFieldName { get; set; }

        public AcademicRank AcademicRank { get; set; }

        public bool ProjectsVisible { get; set; }
        public ICollection<ProfileProjectPreviewDisplayModel> AuthoredProjectDisplayModels { get; set; } = new HashSet<ProfileProjectPreviewDisplayModel>();
        public ICollection<ProfileProjectPreviewDisplayModel> ProjectCollaborationsDisplayModels { get; set; } = new HashSet<ProfileProjectPreviewDisplayModel>();

        public ProfileDisplayModel()
        {

        }

        public ProfileDisplayModel(User user, bool isPersonalProfile, Faculty faculty, ICollection<Project> authoredProjects,
                                    ICollection<Project> projectCollaborations, UserSettings userSettings)
        {
            EmailVisible = userSettings.EmailVisible;
            PhoneNumberVisible = userSettings.PhoneVisible;
            ProjectsVisible = userSettings.ProjectsVisible;

            IsDisplayPersonal = isPersonalProfile;
            FirstName = user.FirstName;
            LastName = user.LastName;

            LinkedinProfile = userSettings.LinkedinProfile;
            ResearchgateProfile = userSettings.ResearchgateProfile;
            Website = userSettings.Website;
            Cv = userSettings.CvLink;
            Bio = userSettings.Bio;
            AccountPhoto = userSettings.PhotoLink;

            if (userSettings.PhoneVisible)
                PhoneNumber = user.PhoneNumber;

            if (user is Student s)
            {
                UserAccountType = UserAccountType.Student;
                FacultyName = faculty.Name;

                var program = faculty.StudyPrograms.Where(sp => sp.ProgramId == s.StudyProgramId).First();
                StudyProgramName = program.Name;

                Cycle = program.Cycle;

                if (s.StudyProgramSpecializationId != null)
                {
                    StudyProgramSpecializationName = program.StudyProgramSpecializations.Where(sps => sps.SpecializationId == s.StudyProgramSpecializationId)
                                                                                        .First().Name;
                }

                StudyYear = s.StudyYear;
            }
            else if (user is FacultyMember fm)
            {
                UserAccountType = UserAccountType.FacultyMember;
                FacultyName = faculty.Name;
                StudyFieldName = fm.StudyField.Name;
                AcademicRank = fm.AcademicRank;
            }

            foreach (var project in authoredProjects)
                AuthoredProjectDisplayModels.Add(new ProfileProjectPreviewDisplayModel(project));

            if (userSettings.ProjectsVisible)
            {
                if (projectCollaborations != null)
                {
                    foreach (var project in projectCollaborations)
                        ProjectCollaborationsDisplayModels.Add(new ProfileProjectPreviewDisplayModel(project));
                }
            }
        }
    }
}
