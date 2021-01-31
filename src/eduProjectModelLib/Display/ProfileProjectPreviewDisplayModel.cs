using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace eduProjectModel.Display
{
    public class ProfileProjectPreviewDisplayModel
    {
        public string Title { get; set; }

        public int ProjectId { get; set; }

        public ProfileProjectPreviewDisplayModel()
        {

        }

        public ProfileProjectPreviewDisplayModel(Project project)
        {
            Title = project.Title;
            ProjectId = project.ProjectId;
        }
    }
}
