namespace eduProjectWebAPI.Model
{
    public class StudyProgramSpecialization : IValueObject
    {
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
