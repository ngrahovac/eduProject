using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                newUser.Id = GetNextAvailableUserId();

                var result = await userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                    return RedirectToAction("Login", "Account");

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpGet]
        public ActionResult<LoginInputModel> Login()
        {
            return new LoginInputModel();
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                    return Ok("Login successful"); //Redirect here to the HOME PAGE
                else
                    ModelState.AddModelError(string.Empty, "Neispravan pokušaj logovanja");
            }

            return BadRequest(ModelState);
        }

        private string GetNextAvailableUserId()
        {
            var users = userManager.Users.ToList();

            if (users.Count == 0)
                return "1";
            else
            {
                var ids = users.Select(x => x.Id).ToList();

                for (int i = 1; true; i++)
                    if (!ids.Contains(i.ToString()))
                        return i.ToString();
            }
        }

        //TEST for fetching current logged in user ID and UserName
        [Route("[action]")]
        [HttpGet]
        public IActionResult WhoAmI()
        {
            return Content(User.FindFirstValue(ClaimTypes.NameIdentifier) + " " + User.FindFirstValue(ClaimTypes.Name));
        }

        [AllowAnonymous]
        [Route("[action]")]
        public IActionResult ExternalLogin(string returnUrl = null)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            //Configure where to redirect after a successful login in case of returnUrl being null
            returnUrl = returnUrl ?? Url.Content("~/Projects/1");

            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
                return BadRequest("Greška pri učitavanju eksternih login informacija");

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
                return LocalRedirect(returnUrl);
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        user.Id = GetNextAvailableUserId();

                        await userManager.CreateAsync(user);
                    }

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                return BadRequest("Greška pri logovanju");
            }
        }
    }
}
