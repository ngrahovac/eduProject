using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eduProjectModel.Display
{
    public class ProjectApplicationsDisplayModel
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }

        public ICollection<CollaboratorProfileApplicationsDisplayModel> CollaboratorProfileApplicationsDisplayModels { get; set; } = new List<CollaboratorProfileApplicationsDisplayModel>();

        public ProjectApplicationsDisplayModel()
        {

        }

        public ProjectApplicationsDisplayModel(Project project, ICollection<ProjectApplication> applications, ICollection<Tuple<int, string, string>> modelData)
        {
            ProjectId = project.ProjectId;
            Title = project.Title;

            foreach (var profile in project.CollaboratorProfiles)
            {
                CollaboratorProfileApplicationsDisplayModels.Add(new CollaboratorProfileApplicationsDisplayModel
                                                                (profile, applications.Where(a => a.CollaboratorProfileId == profile.CollaboratorProfileId).ToList(), modelData));
            }
        }
    }
}