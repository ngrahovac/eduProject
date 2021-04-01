using eduProjectModel.Domain;

namespace eduProjectModel.Display
{
    public class AccountDisplayModel
    {
        public int AccountId { get; set; }
        public string Username { get; set; }

        public AccountDisplayModel()
        {

        }

        public AccountDisplayModel(ApplicationUser user)
        {
            AccountId = int.Parse(user.Id);
            Username = user.UserName;
        }
    }
}
