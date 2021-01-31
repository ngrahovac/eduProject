using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Domain
{
    public enum ProjectStatus
    {
        [Display(Name = "Aktivan")]
        Active = 1,

        [Display(Name = "Zatvoren")]
        Closed = 2

    }
}

