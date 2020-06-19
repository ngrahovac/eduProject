using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Data
{
    public partial class Account
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public int UserId { get; set; }
        public int UserAccountTypeId { get; set; }

        public virtual User User { get; set; }
        public virtual UserAccountType UserAccountType { get; set; }
    }
}
