using eduProjectModel.Domain;
using System;
using System.Collections.Generic;

namespace eduProjectModel.Display
{
    public class ProjectDisplayModel
    {
        public bool IsDisplayForAuthor { get; set; }
        public bool IsProjectActive { get; set; }

        public string ProjectStatus { get; set; }
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

        public ProjectDisplayModel(Project project, User author, bool isDisplayForAuthor, bool isProjectActive, ICollection<CollaboratorDisplayModel> models = null)
        {
            IsDisplayForAuthor = isDisplayForAuthor;
            IsProjectActive = isProjectActive;

            ProjectStatus = project.ProjectStatus.ToString();
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
                    if (profile is StudentProfile)
                        StudentProfileDisplayModels.Add(StudentProfileDisplayModel.FromStudentProfile((StudentProfile)profile));
                    else if (profile is FacultyMemberProfile)
                        FacultyMemberProfileDisplayModels.Add(FacultyMemberProfileDisplayModel.FromFacultyMemberProfile((FacultyMemberProfile)profile));
                }
            }
            else
            {
                CollaboratorDisplayModels = models;
                StudentProfileDisplayModels = null;
                FacultyMemberProfileDisplayModels = null;
            }
        }
    }
}