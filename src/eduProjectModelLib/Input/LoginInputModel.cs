using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        [EmailAddress(ErrorMessage = "Email adresa nije ispravnog oblika.")]
        public string Email { get; set; }

        //Password check happens on the API side. How to display wrong password error message on the GUI?
        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Zapamti me")]
        public bool RememberMe { get; set; }
    }
}
