namespace eduProjectModel.Domain
{
    public class StudyField : IValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public StudyField(string name, string description)
        {
            Name = name;
            Description = description;
        }


    }
}
