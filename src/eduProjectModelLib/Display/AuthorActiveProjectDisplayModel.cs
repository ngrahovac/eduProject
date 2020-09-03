using eduProjectModel.Domain;
using System;
using System.Collections.Generic;

namespace eduProjectModel.Display
{
    public sealed class AuthorActiveProjectDisplayModel : ProjectDisplayModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public ICollection<StudentProfileDisplayModel> StudentProfileDisplayModels { get; set; } = new HashSet<StudentProfileDisplayModel>();
        public ICollection<FacultyMemberProfileDisplayModel> FacultyMemberProfileDisplayModels { get; set; } = new HashSet<FacultyMemberProfileDisplayModel>();

        public AuthorActiveProjectDisplayModel(Project project, User author) : base(project, author)
        {
            StartDate = project.StartDate;
            EndDate = project.EndDate;

            foreach (var tag in Tags)
                Tags.Add(tag);

            foreach (var profile in project.CollaboratorProfiles)
            {
                if (profile is StudentProfile)
                    StudentProfileDisplayModels.Add(StudentProfileDisplayModel.FromStudentProfile((StudentProfile)profile));
                else if (profile is FacultyMemberProfile)
                    FacultyMemberProfileDisplayModels.Add(FacultyMemberProfileDisplayModel.FromFacultyMemberProfile((FacultyMemberProfile)profile));
            }
        }
    }
}
