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
using System.Collections.Generic;
using eduProjectModel.Display;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using eduProjectWebAPI.Data;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin, User", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IFacultiesRepository faculties;
        private readonly IUsersRepository users;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            IFacultiesRepository faculties, IUsersRepository users)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.faculties = faculties;
            this.users = users;
        }

        [AllowAnonymous]
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationInputModel2 model)
        {
            var newUser = new ApplicationUser
            {
                UserName = model.RegisterInputModel.Email,
                Email = model.RegisterInputModel.Email,
                ActiveStatus = true
            };

            newUser.Id = GetNextAvailableUserId();
            var result = await userManager.CreateAsync(newUser, model.RegisterInputModel.Password);

            if (result.Succeeded)
            {
                try
                {
                    // popunjavanje user dijela

                    var faculty = (await faculties.GetAllAsync()).Where(f => f.Name == model.UserProfileInputModel.FacultyName).First();

                    if (model.UserProfileInputModel.UserAccountType == UserAccountType.Student)
                    {
                        var student = new Student();
                        model.UserProfileInputModel.MapTo(student, faculty);
                        student.UserId = int.Parse(newUser.Id);
                        await users.AddAsync(student);
                    }
                    else
                    {
                        var facultyMember = new FacultyMember();
                        model.UserProfileInputModel.MapTo(facultyMember, faculty);
                        facultyMember.UserId = int.Parse(newUser.Id);

                        try
                        {
                            await users.AddAsync(facultyMember);
                        }
                        catch (Exception ex)
                        {
                            return BadRequest();
                        }
                    }

                    var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, token = confirmationToken }, Request.Scheme);

                    MailSender.SendActivationEmail(model.RegisterInputModel.Email, confirmationLink);
                    return Ok("Na Vašu email adresu je poslan link za aktivaciju naloga.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest(ex);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(RegisterInputModel model)
        {
            int? currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (currentUserId != null)
            {
                var user = await userManager.FindByIdAsync(currentUserId.ToString());

                user.Email = model.Email;
                user.UserName = model.Email;

                var result = await userManager.UpdateAsync(user);

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await userManager.ResetPasswordAsync(user, token, model.Password);

                if (result.Succeeded && passResult.Succeeded)
                    return Ok();
            }

            return NotFound();
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
        [AllowAnonymous]
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

            //var confUser = await userManager.FindByEmailAsync(model.Email);
            //if (confUser != null && await userManager.CheckPasswordAsync(confUser, model.Password)) //!confUser.EmailConfirmed
            //    return BadRequest("Niste aktivirali vas nalog");


            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
                return BadRequest("Korisničko ime ili lozinka nisu ispravni.");

            var user = await userManager.FindByEmailAsync(model.Email);

            if (!user.ActiveStatus)
                return BadRequest("Vaš nalog je suspendovan. Za više informacija, molimo kontaktirajte administratora.");

            var id = user.Id.ToString();

            string role = await userManager.IsInRoleAsync(user, "Admin") ? "Admin" : "User";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Role, role)
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


        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateAccountStatus(int id, AccountManagementInputModel model)
        {

            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                user.ActiveStatus = model.ActiveStatus;
                await userManager.UpdateAsync(user);
                return Ok();
            }
            else
                return NotFound();
        }


        [HttpGet]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<ICollection<AccountDisplayModel>>> GetAllAccounts()
        {
            var users = await userManager.Users.ToListAsync();
            var models = new List<AccountDisplayModel>();

            foreach (var user in users)
            {
                if (!(await userManager.IsInRoleAsync(user, "Admin")))
                    models.Add(new AccountDisplayModel(user));
            }

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDisplayModel>> GetById(int id)
        {
            var userAccount = await userManager.FindByIdAsync($"{id}");

            if (userAccount == null)
                return NotFound();

            return new AccountDisplayModel(userAccount);
        }

    }
}
