using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Domain
{
    public enum UserAccountType
    {
        [Display(Name = "Student")]
        Student = 1,

        [Display(Name = "Nastavno osoblje")]
        FacultyMember = 2
    }
}
