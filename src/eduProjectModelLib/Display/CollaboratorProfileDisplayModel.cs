using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectModel.Display
{
    public class CollaboratorProfileDisplayModel
    {
        public int CollaboratorProfileId { get; set; }
        public bool ApplicationsOpen { get; set; }
        public string Description { get; set; }
        public bool Recommended { get; set; } = false;
        public bool AlreadyApplied { get; set; }
        public CollaboratorProfileDisplayModel()
        {

        }

        public CollaboratorProfileDisplayModel(CollaboratorProfile profile)
        {
            CollaboratorProfileId = profile.CollaboratorProfileId;
            Description = profile.Description;
            ApplicationsOpen = profile.ApplicationsOpen;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
