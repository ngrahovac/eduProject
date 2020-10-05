using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace eduProjectModel.Input
{
    public class ProjectInputModel
    {
        public string Title { get; set; }
        public string StudyFieldName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public ICollection<string> TagNames { get; set; } = new List<string>();
        public ICollection<CollaboratorProfileInputModel> CollaboratorProfileInputModels { get; set; } = new List<CollaboratorProfileInputModel>();

        public ProjectInputModel()
        {

        }

        public void MapTo(Project project)
        {
            //mapirati ova polja na objekat tipa Project ako je prosao validaciju, prije upisa u bazu
        }
    }
}
