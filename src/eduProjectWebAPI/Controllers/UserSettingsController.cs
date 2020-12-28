using eduProjectModel.Display;
using eduProjectModel.Domain;
using eduProjectModel.Input;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("/users/{userId}/settings")]
    public class UserSettingsController : ControllerBase
    {
        private IUserSettingsRepository settings;

        public UserSettingsController(IUserSettingsRepository settings)
        {
            this.settings = settings;
        }

        [HttpGet]
        public async Task<ActionResult<UserSettingsDisplayModel>> Get(int userId)
        {
            try
            {
                var userSettings = await settings.GetAsync(userId);
                return new UserSettingsDisplayModel(userSettings);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(int userId, UserSettingsInputModel model)
        {
            try
            {
                var userSettings = await settings.GetAsync(userId);
                model.MapTo(userSettings);
                await settings.UpdateAsync(userSettings);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
