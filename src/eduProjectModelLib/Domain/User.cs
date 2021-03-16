using System.Collections.Generic;

namespace eduProjectModel.Domain
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneFormat { get; set; }
        public ICollection<int> AuthoredProjectIds { get; set; } = new List<int>();
        public ICollection<int> ProjectCollaborationIds { get; set; } = new List<int>();
    }
}