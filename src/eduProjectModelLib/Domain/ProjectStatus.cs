using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Domain
{
    public enum ProjectStatus
    {
        [Display(Name = "U pripremi")]
        Active = 1,

        [Display(Name = "Završen")]
        Closed = 2,

        [Display(Name = "U izradi")]
        InProgress = 3
    }
}

