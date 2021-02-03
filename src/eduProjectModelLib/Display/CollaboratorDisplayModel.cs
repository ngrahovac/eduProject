using eduProjectModel.Domain;

namespace eduProjectModel.Display
{
    public class CollaboratorDisplayModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int CollaboratorId { get; set; }

        public CollaboratorDisplayModel() { }
        public CollaboratorDisplayModel(User collaborator)
        {
            FirstName = collaborator.FirstName;
            LastName = collaborator.LastName;
            CollaboratorId = collaborator.UserId;
        }
    }
}
