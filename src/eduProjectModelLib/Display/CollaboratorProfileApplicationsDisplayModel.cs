using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eduProjectModel.Display
{
    public class CollaboratorProfileApplicationsDisplayModel
    {
        public CollaboratorProfileDisplayModel CollaboratorProfileDisplayModel { get; set; }

        public ICollection<ApplicationDisplayModel> ApplicationDisplayModels { get; set; }

        public CollaboratorProfileApplicationsDisplayModel()
        {

        }

        public CollaboratorProfileApplicationsDisplayModel(CollaboratorProfile profile, ICollection<ProjectApplication> applications, ICollection<Tuple<int, string, string>> modelData)
        {
            CollaboratorProfileDisplayModel = new CollaboratorProfileDisplayModel(profile);

            ApplicationDisplayModels = applications.Select(a => new ApplicationDisplayModel(a, modelData.Where(t => t.Item1 == a.ApplicantId).First())).ToList();
        }
    }
}