using eduProjectModel.Display;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/users/{userId}/settings")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserSettingsController : ControllerBase
    {
        private readonly IUserSettingsRepository settings;

        public UserSettingsController(IUserSettingsRepository settings)
        {
            this.settings = settings;
        }

        [HttpGet]
        public async Task<ActionResult<UserSettingsDisplayModel>> Get(int userId)
        {
                try
                {
                    int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                    if (userId == currentUserId)
                    {
                        var userSettings = await settings.GetAsync(currentUserId);
                        return new UserSettingsDisplayModel(userSettings);
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message + "\n" + e.StackTrace);
                }
        }

        [HttpPut]
        public async Task<ActionResult> Update(int userId, UserSettingsInputModel model)
        {
                try
                {
                    int currentUserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

                    if (userId == currentUserId)
                    {
                        var userSettings = await settings.GetAsync(currentUserId);

                        model.MapTo(userSettings);
                        await settings.UpdateAsync(userSettings);

                        return NoContent();
                    }
                    else
                    {
                        return Forbid();
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message + "\n" + e.StackTrace);
                }
        }
    }
}
