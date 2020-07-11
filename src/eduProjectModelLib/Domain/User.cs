using System.Collections.Generic;

namespace eduProjectModel.Domain
{
    public class User : IAggregateRoot
    {
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string PhoneFormat { get; protected set; }
        public ICollection<int> AuthoredProjectIds { get; protected set; }
        public ICollection<int> ProjectApplicationIds { get; protected set; }
        public ICollection<int> ProjectCollaborationIds { get; protected set; } // ZORANE imamo li bolji naziv za ovo?

        public User(string firstName, string lastName, string phoneNumber, string phoneFormat)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            PhoneFormat = phoneFormat;

            AuthoredProjectIds = new HashSet<int>();
            ProjectApplicationIds = new HashSet<int>();
            ProjectCollaborationIds = new HashSet<int>();
        }
    }
}
