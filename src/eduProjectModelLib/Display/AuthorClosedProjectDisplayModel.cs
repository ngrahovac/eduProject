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
        public ICollection<CollaboratorDisplayModel> CollaboratorDisplayModels { get; set; }

        public AuthorClosedProjectDisplayModel(Project project, User author, ICollection<CollaboratorDisplayModel> models) : base(project, author)
        {
            StartDate = project.StartDate;
            EndDate = project.EndDate;
            foreach (var tag in Tags)
                Tags.Add(tag);
            CollaboratorDisplayModels = models;
        }
    }
}
