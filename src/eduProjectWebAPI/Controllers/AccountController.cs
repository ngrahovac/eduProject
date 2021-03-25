using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using eduProjectWebAPI.Services;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
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
                {
                    try
                    {
                        var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, token = confirmationToken }, Request.Scheme);

                        MailSender.SendActivationEmail(model.Email, confirmationLink);
                        return Ok("Na Vašu email adresu je poslan link za aktivaciju naloga.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();
            else
            {
                var result = await userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                    return Ok("Vaš nalog na eduProject platformi je uspješno aktiviran. Zahvaljujemo Vam na saradnji!");
                else
                    return BadRequest("Aktivacija naloga je neuspješna");
            }
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();
                return Ok("Logout from API successful");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return BadRequest("Logout exception happened");
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

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login2(LoginInputModel model)
        {
            //Code beneath must be synchronous to work, but for email confirmation
            //user must be fetched asynchronously which results in code redundancy.
            //This is just for initial testing. Code refarctoring needed here.

            var confUser = await userManager.FindByEmailAsync(model.Email);
            if (confUser != null && await userManager.CheckPasswordAsync(confUser, model.Password)) //!confUser.EmailConfirmed
                return BadRequest("Niste aktivirali vas nalog");


            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
                return BadRequest();

            var user = userManager.FindByEmailAsync(model.Email);
            var id = user.Result.Id.ToString();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.NameIdentifier, id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtAudience"],
                claims, expires: expiry, signingCredentials: creds
            );

            return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token), loggedUserId = id });
        }
    }
}
