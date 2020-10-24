namespace eduProjectModel.Domain
{
    public class CollaboratorProfile : IEntity
    {
        public int CollaboratorProfileId { get; set; }
        public string Description { get; set; }
        public int? FacultyId { get; set; }
        public CollaboratorProfile()
        {

        }
        public CollaboratorProfile(string description, int? facultyId)
        {
            Description = description;
            FacultyId = facultyId;
        }
    }
}
