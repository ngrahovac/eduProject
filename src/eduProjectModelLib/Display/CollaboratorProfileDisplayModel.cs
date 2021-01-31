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
        public string Description { get; set; }
        public bool Recommended { get; set; } = false;
        public CollaboratorProfileDisplayModel()
        {

        }

        public CollaboratorProfileDisplayModel(CollaboratorProfile profile)
        {
            Description = profile.Description;
        }
    }
}
