using eduProjectModel.Domain;
using System.Collections.Generic;

namespace eduProjectModel.Input
{
    public class StudyProgramInputModel
    {
        public ICollection<StudyProgramSpecializationInputModel> StudyProgramSpecializationInputModels { get; set; } = new List<StudyProgramSpecializationInputModel>();

        public void MapTo(StudyProgram studyProgram)
        {
            foreach (var model in StudyProgramSpecializationInputModels)
            {
                StudyProgramSpecialization specialization = new StudyProgramSpecialization();

                model.MapTo(specialization);

                studyProgram.StudyProgramSpecializations.Add(specialization);
            }

        }
    }
}
