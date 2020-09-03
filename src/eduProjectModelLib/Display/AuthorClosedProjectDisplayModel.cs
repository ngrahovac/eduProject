using eduProjectModel.Domain;
using System;
using System.Collections.Generic;

namespace eduProjectModel.Display
{
    public sealed class AuthorClosedProjectDisplayModel : ProjectDisplayModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public ICollection<CollaboratorDisplayModel> AppliedUsers { get; set; }

        public AuthorClosedProjectDisplayModel(Project project, User author, ICollection<CollaboratorDisplayModel> appliedUsers) : base(project, author)
        {
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            foreach (var tag in Tags)
                Tags.Add(tag);
            AppliedUsers = appliedUsers;
        }
    }
}
