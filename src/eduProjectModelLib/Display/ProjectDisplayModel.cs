using eduProjectModel.Domain;
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
        public ICollection<CollaboratorDisplayModel> CollaboratorDisplayModels { get; set; } = null;

        public ProjectDisplayModel() { }

        public ProjectDisplayModel(Project project, User author, bool isDisplayForAuthor, bool isProjectActive,
                                   ICollection<User> collaborators = null, ICollection<Faculty> faculties = null)
        {
            IsDisplayForAuthor = isDisplayForAuthor;
            IsProjectActive = isProjectActive;

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
                    if (profile is StudentProfile)
                    {
                        StudentProfileDisplayModels.Add(new StudentProfileDisplayModel((StudentProfile)profile, faculty));
                    }
                    else if (profile is FacultyMemberProfile)
                    {
                        FacultyMemberProfileDisplayModels.Add(new FacultyMemberProfileDisplayModel((FacultyMemberProfile)profile, faculty));
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
    }
}