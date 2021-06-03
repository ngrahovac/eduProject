using eduProjectModel.Domain;
using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class StudyProgramSpecializationInputModel
    {
        [Required(ErrorMessage = "Naziv studijskog smjera je obavezan.")]
        public string Name { get; set; }

        public void MapTo(StudyProgramSpecialization studyProgramSpecialization)
        {
            studyProgramSpecialization.Name = Name;
        }
    }
}
