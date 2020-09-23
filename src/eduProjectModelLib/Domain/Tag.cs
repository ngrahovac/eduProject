namespace eduProjectModel.Domain
{
    public class Tag : IValueObject
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public Tag()
        {

        }
        public Tag(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
