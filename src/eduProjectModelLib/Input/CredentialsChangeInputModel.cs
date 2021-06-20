using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class CredentialsChangeInputModel
    {
        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lozinke se ne podudaraju")]
        public string ConfirmNewPassword { get; set; }

        public CredentialsChangeInputModel()
        {

        }
    }
}
