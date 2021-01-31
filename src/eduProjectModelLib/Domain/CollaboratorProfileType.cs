using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Domain
{
    public enum CollaboratorProfileType
    {
        [Display(Name = "Student")]
        Student = 1,

        [Display(Name = "Nastavno osoblje")]
        FacultyMember = 2
    }
}
