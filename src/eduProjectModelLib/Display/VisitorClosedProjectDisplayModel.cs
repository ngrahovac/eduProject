using eduProjectModel.Domain;
using System;
using System.Collections.Generic;

namespace eduProjectModel.Display
{
    public sealed class VisitorClosedProjectDisplayModel : ProjectDisplayModel
    {
        public ICollection<CollaboratorDisplayModel> AppliedUsers { get; set; }

        public VisitorClosedProjectDisplayModel(Project project, User author, ICollection<CollaboratorDisplayModel> appliedUsers) : base(project, author)
        {
            AppliedUsers = appliedUsers;
        }
    }
}
