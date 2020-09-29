namespace eduProjectModel.Domain
{
    public class Account : IAggregateRoot
    {
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public UserAccountType UserAccountType { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Salt { get; private set; }
        public UserSettings UserSettings { get; private set; }

        public Account(string username, string email, string passwordHash, string salt, User user, UserAccountType accountType)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            Salt = salt;
            UserAccountType = accountType;
        }
    }
}
