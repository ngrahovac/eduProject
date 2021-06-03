using eduProjectModel.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class StudyProgramInputModel
    {
        [Required(ErrorMessage = "Naziv studijskog programa je obavezan.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ciklus studijskog programa je obavezan.")]
        [Range(1, 3, ErrorMessage = "Ciklus može biti u opsegu 1-3.")]
        public int Cycle { get; set; }

        [Required(ErrorMessage = "Trajanje studijskog programa je obavezno.")]
        [Range(1, 4, ErrorMessage = "Trajanje može biti u opsegu 1-4.")]
        public int DurationYears { get; set; }

        public ICollection<StudyProgramSpecializationInputModel> StudyProgramSpecializationInputModels { get; set; } = new List<StudyProgramSpecializationInputModel>();

        public void MapTo(StudyProgram studyProgram)
        {
            studyProgram.Name = Name;
            studyProgram.Cycle = (byte)Cycle;
            studyProgram.DurationYears = (byte)DurationYears;

            foreach (var model in StudyProgramSpecializationInputModels)
            {
                StudyProgramSpecialization specialization = new StudyProgramSpecialization();

                model.MapTo(specialization);

                studyProgram.StudyProgramSpecializations.Add(specialization);
            }

        }
    }
}
