using System;
using System.Collections.Generic;
using eduProjectModel.Domain;

namespace eduProjectModel.Display
{
    public class ProjectDisplayModel
    {
        public string ProjectStatus { get; set; }
        public string Title { get; set; }
        public string AuthorFullName { get; set; }
        public StudyField StudyField { get; set; }
        public string Description { get; set; }

        public ProjectDisplayModel()
        {

        }

        public ProjectDisplayModel(Project project, User author)
        {
            ProjectStatus = project.ProjectStatus.ToString();
            Title = project.Title;
            AuthorFullName = $"{author.FirstName} {author.LastName}";
            StudyField = project.StudyField;
            Description = project.Description;
        }
    }
}