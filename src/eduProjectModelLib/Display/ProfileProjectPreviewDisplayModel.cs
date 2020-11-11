using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace eduProjectModel.Display
{
    public class ProfileProjectPreviewDisplayModel
    {
        public string Title { get; set; }

        public ProfileProjectPreviewDisplayModel()
        {

        }

        public ProfileProjectPreviewDisplayModel(Project project)
        {
            Title = project.Title;
        }
    }
}
