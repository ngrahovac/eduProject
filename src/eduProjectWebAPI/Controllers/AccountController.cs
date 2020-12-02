using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

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
                return (int.Parse(users.Last().Id) + 1).ToString();
        }

        //TEST for fetching current logged in user ID and UserName
        [Route("[action]")]
        [HttpGet]
        public IActionResult WhoAmI()
        {
            return Content(User.FindFirstValue(ClaimTypes.NameIdentifier) + " " + User.FindFirstValue(ClaimTypes.Name));
        }
    }
}
