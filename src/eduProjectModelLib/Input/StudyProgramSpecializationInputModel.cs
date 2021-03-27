using eduProjectModel.Domain;

namespace eduProjectModel.Input
{
    public class StudyProgramSpecializationInputModel
    {
        public string Name { get; set; }

        public void MapTo(StudyProgramSpecialization studyProgramSpecialization)
        {
            studyProgramSpecialization.Name = Name;
        }
    }
}
