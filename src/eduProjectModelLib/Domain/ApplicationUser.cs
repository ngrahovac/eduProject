using Microsoft.AspNetCore.Identity;

namespace eduProjectModel.Domain
{
    public class ApplicationUser : IdentityUser, IAggregateRoot
    {
        /*This class inherits from IdentityUser in case of additional data requirement*/
    }
}
