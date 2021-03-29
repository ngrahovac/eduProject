using eduProjectModel.Domain;
using System.Collections.Generic;

namespace eduProjectModel.Input
{
    public class StudyProgramInputModel
    {
        public string Name { get; set; }
        public int Cycle { get; set; }
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
