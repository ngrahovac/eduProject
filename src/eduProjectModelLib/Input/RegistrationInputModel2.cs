using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eduProjectModel.Input
{
    public class RegistrationInputModel2
    {
        [ValidateComplexType]
        public UserProfileInputModel UserProfileInputModel { get; set; } = new UserProfileInputModel();

        [ValidateComplexType]
        public RegisterInputModel RegisterInputModel { get; set; } = new RegisterInputModel(); // mail, username, password

    }
}
