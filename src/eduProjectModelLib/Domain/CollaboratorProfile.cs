namespace eduProjectModel.Domain
{
    public class CollaboratorProfile
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
