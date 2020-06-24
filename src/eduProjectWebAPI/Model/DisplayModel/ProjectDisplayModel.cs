using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Model.DisplayModel
{
    public class ProjectDisplayModel
    {
        public ProjectDisplayModel()
        {
        }

        public int ProjectId { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }

        //public int? StudyFieldId { get; set; }
        //public int ProjectStatusId { get; set; }
        //public int UserId { get; set; }

        // public virtual ProjectStatus ProjectStatus { get; set; }
        public string ProjectStatus { get; set; }
        // public virtual StudyField StudyField { get; set; }
        // public virtual User User { get; set; }
        // public virtual ICollection<CollaboratorProfile> CollaboratorProfiles { get; set; }
        // public virtual ICollection<ProjectCollaborator> ProjectCollaborators { get; set; }
        // public virtual ICollection<ProjectTag> ProjectTag { get; set; }

        public static ProjectDisplayModel FromProject(Project project)
        {
            return new ProjectDisplayModel
            {
                ProjectId = project.ProjectId,
                Title = project.Title,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Description = project.Description,
                ProjectStatus = project.ProjectStatus.Name
            };
        }
    }
}
