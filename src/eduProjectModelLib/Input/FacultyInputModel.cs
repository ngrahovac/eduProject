using eduProjectModel.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class FacultyInputModel
    {
        [Required(ErrorMessage = "Naziv fakulteta je obavezan.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adresa fakulteta je obavezna.")]
        public string Address { get; set; }

        [MinLength(1, ErrorMessage = "Neophodno je unijeti bar jedan studijski program.")]
        public ICollection<StudyProgramInputModel> StudyProgramInputModels { get; set; } = new List<StudyProgramInputModel>();

        public void MapTo(Faculty faculty)
        {
            faculty.Name = Name;
            faculty.Address = Address;

            foreach (var model in StudyProgramInputModels)
            {
                StudyProgram studyProgram = new StudyProgram();

                model.MapTo(studyProgram);

                faculty.StudyPrograms.Add(studyProgram);
            }
        }
    }
}
