using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace eduProjectModel.Input
{
    public class ProjectApplicationInputModel
    {
        public int ApplicantId { get; set; }

        public int ApplicationId { get; set; } // used in status change

        public ProjectApplicationStatus ProjectApplicationStatus { get; set; }

        public string ApplicantComment { get; set; }

        public string AuthorComment { get; set; }

        public int ProjectId { get; set; }

        public int CollaboratorProfileId { get; set; }

        // mapping what we can, the rest is mapped from the controller
        public void MapTo(ProjectApplication application)
        {
            application.CollaboratorProfileId = CollaboratorProfileId;
            application.AuthorComment = AuthorComment;
            application.ApplicantComment = ApplicantComment;
            application.ProjectApplicationStatus = ProjectApplicationStatus;
            application.ApplicantId = ApplicantId;
        }

    }
}
