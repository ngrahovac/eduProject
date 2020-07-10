namespace eduProjectWebAPI.Model
{
    public class CollaboratorProfile : IEntity
    {
        public int CollaboratorProfileId { get; set; }
        public string Description { get; set; }

        public CollaboratorProfile()
        {

        }

        public CollaboratorProfile(string description)
        {
            Description = description;

        }
    }
}
