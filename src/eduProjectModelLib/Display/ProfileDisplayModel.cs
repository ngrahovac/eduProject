﻿using eduProjectModel.Domain;
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

        public string StudyProgramSpecializationName { get; set; }
        public int StudyYear { get; set; }

        public string StudyFieldName { get; set; }

        public AcademicRank AcademicRank { get; set; }

        public ICollection<ProfileProjectPreviewDisplayModel> AuthoredProjectDisplayModels { get; set; } = new HashSet<ProfileProjectPreviewDisplayModel>();
        public ICollection<ProfileProjectPreviewDisplayModel> ProjectCollaborationsDisplayModels { get; set; } = new HashSet<ProfileProjectPreviewDisplayModel>();

        public ProfileDisplayModel()
        {

        }

        public ProfileDisplayModel(User user, bool isPersonalProfile, Faculty faculty, ICollection<Project> authoredProjects,
                                    ICollection<Project> projectCollaborations)
        {
            IsDisplayPersonal = isPersonalProfile;
            FirstName = user.FirstName;
            LastName = user.LastName;
            PhoneNumber = user.PhoneNumber;

            if (user is Student s)
            {
                UserAccountType = UserAccountType.Student;
                FacultyName = faculty.Name;

                if (s.StudyProgramId != 0)
                {
                    var program = faculty.StudyPrograms.Where(sp => sp.ProgramId == s.StudyProgramId).First();
                    StudyProgramName = program.Name;

                    Cycle = program.Cycle;

                    if (s.StudyProgramSpecializationId != 0)
                    {
                        StudyProgramSpecializationName = program.StudyProgramSpecializations.Where(sps => sps.SpecializationId == s.StudyProgramSpecializationId)
                                                                                            .First().Name;
                    }

                    StudyYear = s.StudyYear;
                }
            }
            else if (user is FacultyMember fm)
            {
                UserAccountType = UserAccountType.FacultyMember;
                FacultyName = faculty.Name;
                StudyFieldName = fm.StudyField.Name;
                AcademicRank = fm.AcademicRank;
            }

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
        }
    }
}
