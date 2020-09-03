using System.Collections.Generic;

namespace eduProjectModel.Domain
{
    public class User : IAggregateRoot
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneFormat { get; set; }
        public int UserId { get; set; }
        public ICollection<int> AuthoredProjectIds { get; set; }
        public ICollection<int> ProjectApplicationIds { get; set; }
        public ICollection<int> ProjectCollaborationIds { get; set; }
    }
}