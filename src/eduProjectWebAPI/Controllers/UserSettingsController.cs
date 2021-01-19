using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using eduProjectWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/users/{userId}/settings")]
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
            if (HttpContext.Request.ExtractUserId() == null)
            {
                return Unauthorized();
            }
            else
            {
                try
                {
                    int currentUserId = (int)HttpContext.Request.ExtractUserId();

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
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(int userId, UserSettingsInputModel model)
        {
            if (HttpContext.Request.ExtractUserId() == null)
            {
                return Unauthorized();
            }
            else
            {
                try
                {
                    int currentUserId = (int)HttpContext.Request.ExtractUserId();

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
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
        }
    }
}
