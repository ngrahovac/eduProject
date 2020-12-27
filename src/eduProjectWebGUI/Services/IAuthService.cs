using eduProjectModel.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebGUI.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginInputModel model);
        Task Logout();
        //Task<RegisterResult> Register(RegisterInputModel model);
    }
}
