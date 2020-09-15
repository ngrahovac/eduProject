using eduProjectModel.Domain;
using System;
using System.Collections.Generic;

namespace eduProjectModel.Display
{
    public sealed class VisitorClosedProjectDisplayModel : ProjectDisplayModel
    {
        public ICollection<CollaboratorDisplayModel> CollaboratorDisplayModels { get; set; }

        public VisitorClosedProjectDisplayModel()
        {

        }

        public VisitorClosedProjectDisplayModel(Project project, User author, ICollection<CollaboratorDisplayModel> models) : base(project, author)
        {
            CollaboratorDisplayModels = models;
        }
    }
}
