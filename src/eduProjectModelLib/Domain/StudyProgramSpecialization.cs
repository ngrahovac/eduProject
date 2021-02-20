namespace eduProjectModel.Domain
{
    public class StudyProgramSpecialization
    {
        public int SpecializationId { get; set; }
        public string Name { get; set; }

        public StudyProgramSpecialization()
        {

        }
        public StudyProgramSpecialization(string name)
        {
            Name = name;
        }


    }
}
