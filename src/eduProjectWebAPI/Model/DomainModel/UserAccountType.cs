using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class UserAccountType
    {
        public UserAccountType()
        {
            Account = new HashSet<Account>();
            CollaboratorProfile = new HashSet<CollaboratorProfile>();
        }

        public int UserAccountTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<CollaboratorProfile> CollaboratorProfile { get; set; }
    }
}
