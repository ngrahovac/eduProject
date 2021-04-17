﻿using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eduProjectModel.Display
{
    [Serializable]
    public class ProjectDisplayModel
    {
        public bool IsDisplayForAuthor { get; set; }
        public bool IsProjectActive { get; set; }

        public int ProjectId { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string Title { get; set; }
        public string AuthorFullName { get; set; }
        public StudyField StudyField { get; set; }
        public string Description { get; set; }

        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public ICollection<StudentProfileDisplayModel> StudentProfileDisplayModels { get; set; } = new HashSet<StudentProfileDisplayModel>();
        public ICollection<FacultyMemberProfileDisplayModel> FacultyMemberProfileDisplayModels { get; set; } = new HashSet<FacultyMemberProfileDisplayModel>();
        public ICollection<CollaboratorDisplayModel> CollaboratorDisplayModels { get; set; } = new HashSet<CollaboratorDisplayModel>();
        public bool Recommended { get; set; }

        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();

        public ProjectDisplayModel() { }

        public ProjectDisplayModel(Project project, User author, User visitor, bool isDisplayForAuthor,
                                   ICollection<User> collaborators = null, ICollection<Faculty> faculties = null)
        {
            IsDisplayForAuthor = isDisplayForAuthor;
            IsProjectActive = project.ProjectStatus == ProjectStatus.Active;

            ProjectId = project.ProjectId;
            ProjectStatus = project.ProjectStatus;
            Title = project.Title;
            AuthorFullName = $"{author.FirstName} {author.LastName}";
            StudyField = project.StudyField;
            Description = project.Description;
            StartDate = project.StartDate;
            EndDate = project.EndDate;

            foreach (var tag in project.Tags)
                Tags.Add(tag);

            if (IsProjectActive)
            {
                foreach (var profile in project.CollaboratorProfiles)
                {
                    Faculty faculty = profile.FacultyId == null ? null : faculties.Where(f => f.FacultyId == profile.FacultyId).First();
                    if (profile is StudentProfile sp)
                    {
                        var model = new StudentProfileDisplayModel(sp, faculty);

                        if (visitor is Student s)
                        {
                            if (sp.FacultyId == null ||
                                sp.FacultyId == s.FacultyId && sp.StudyProgramId == null && sp.StudyProgramSpecializationId == null ||
                                sp.FacultyId == s.FacultyId && sp.StudyProgramId == s.StudyProgramId && sp.StudyProgramSpecializationId == null ||
                                sp.FacultyId == s.FacultyId && sp.StudyProgramId == s.StudyProgramId && sp.StudyProgramSpecializationId == s.StudyProgramSpecializationId && sp.StudyYear == null ||
                                sp.FacultyId == s.FacultyId && sp.StudyProgramId == s.StudyProgramId && sp.StudyProgramSpecializationId == s.StudyProgramSpecializationId && sp.StudyYear == s.StudyYear)
                            {
                                model.Recommended = true;
                                Recommended = true;
                            }
                        }

                        StudentProfileDisplayModels.Add(model);
                    }
                    else if (profile is FacultyMemberProfile fmp)
                    {
                        var model = new FacultyMemberProfileDisplayModel(fmp, faculty);

                        if (visitor is FacultyMember fm)
                        {
                            if (fmp.FacultyId == null ||
                                fmp.FacultyId == fm.FacultyId)
                            {
                                model.Recommended = true;
                                Recommended = true;
                            }
                        }

                        FacultyMemberProfileDisplayModels.Add(model);
                    }

                    if (author.UserId == visitor.UserId)
                    {
                        Recommended = false;
                    }
                }
            }
            else
            {
                CollaboratorDisplayModels = collaborators.Select(u => new CollaboratorDisplayModel(u)).ToList();
                StudentProfileDisplayModels = null;
                FacultyMemberProfileDisplayModels = null;
            }
        }


        public CollaboratorProfileDisplayModel GetCollaboratorProfileDisplayModelById(int id)
        {
            if (StudentProfileDisplayModels.Select(p => p.CollaboratorProfileId).Contains(id))
            {
                return StudentProfileDisplayModels.Where(p => p.CollaboratorProfileId == id).First();
            }
            else if (FacultyMemberProfileDisplayModels.Select(p => p.CollaboratorProfileId).Contains(id))
            {
                return FacultyMemberProfileDisplayModels.Where(p => p.CollaboratorProfileId == id).First();
            }

            return null;
        }

    }
}