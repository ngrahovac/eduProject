using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Domain
{
    public enum ProjectApplicationStatus
    {
        [Display(Name = "Prihvaćena")]
        Accepted = 1,

        [Display(Name = "Odbijena")]
        Rejected = 2,

        [Display(Name = "Na čekanju")]
        OnHold = 3
    }
}
