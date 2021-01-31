using System;
using System.Collections.Generic;
using System.Text;
using eduProjectModel.Domain;

namespace eduProjectModel.Display
{
    public class ApplicationDisplayModel
    {
        public int ApplicationId { get; set; }
        public int ApplicantId { get; set; }
        public string ApplicantFullName { get; set; }
        public string ApplicantEmail { get; set; }

        public string ApplicantComment { get; set; }
        public string AuthorComment { get; set; }

        public ProjectApplicationStatus ProjectApplicationStatus { get; set; }

        public ApplicationDisplayModel()
        {

        }
        public ApplicationDisplayModel(ProjectApplication application, Tuple<int, string, string> applicantData)
        {
            ApplicationId = application.ProjectApplicationId;
            ApplicantId = application.ApplicantId;
            ApplicantComment = application.ApplicantComment;
            AuthorComment = application.AuthorComment;
            ProjectApplicationStatus = application.ProjectApplicationStatus;
            ApplicantFullName = applicantData.Item2;
            ApplicantEmail = applicantData.Item3;
        }
    }
}