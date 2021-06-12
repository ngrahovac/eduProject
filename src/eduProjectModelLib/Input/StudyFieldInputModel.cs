using eduProjectModel.Domain;
using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class StudyFieldInputModel
    {
        [Required(ErrorMessage = "Naziv je obavezan.")]
        public string Name { get; set; }

        public string Description { get; set; }

        public void MapTo(StudyField field)
        {
            field.Name = Name;
            field.Description = Description;
        }
    }
}
