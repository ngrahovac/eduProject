using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectModel
{
    public class ProjectApplicationInputModel
    {
        public string ApplicantComment { get; set; }

        public int ProjectId { get; set; }

        public CollaboratorProfileType CollaboratorProfileType { get; set; }    // FIX - display all profiles in one list
        public int CollaboratorProfileIndex { get; set; }

        // mapping what we can, the rest is mapped from the controller
        public void MapTo(ProjectApplication application)
        {
            application.ApplicantComment = ApplicantComment;
            application.ProjectApplicationStatus = ProjectApplicationStatus.OnHold;
        }

    }
}
