using eduProjectModel.Display;

namespace eduProjectModel.Input
{
    public class AccountManagementInputModel
    {
        public int AccountId { get; set; }
        public bool ActiveStatus { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public AccountManagementInputModel()
        {

        }

        public AccountManagementInputModel(AccountDisplayModel model)
        {
            AccountId = model.AccountId;
            ActiveStatus = model.ActiveStatus;
            Username = model.Username;
            Email = model.Email;
        }
    }
}
