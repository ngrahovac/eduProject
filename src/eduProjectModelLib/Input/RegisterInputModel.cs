using eduProjectModel.Display;
using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class RegisterInputModel
    {
        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        [EmailAddress(ErrorMessage = "Email adresa nije ispravnog oblika.")]
        public string Email { get; set; }

        //Asp.Net Core Identity has its own set of password rules, defined in the Startup class.
        //How to display corresponding errors on GUI based on the user input?
        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju")]
        public string ConfirmPassword { get; set; }

        public RegisterInputModel()
        {

        }

        public RegisterInputModel(AccountDisplayModel model)
        {
            Email = model.Email;
        }
    }
}
