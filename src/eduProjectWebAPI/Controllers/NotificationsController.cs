using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("notifications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsRepository notifications;

        public NotificationsController(INotificationsRepository notifications)
        {
            this.notifications = notifications;
        }

        [HttpGet("author/{authorId:int}/applications")]
        public async Task<ActionResult<ICollection<int>>> GetReceivedApplicationsNotification(int authorId)
        {
            try
            {
                if (int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) == authorId)
                {
                    var result = await notifications.GetReceivedApplicationsNotification(authorId);
                    return result.ToList();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpGet("user/{userId:int}/applications")]
        public async Task<ActionResult<ICollection<int>>> GetSentApplicationsNotification(int userId)
        {
            try
            {
                if (int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) == userId)
                {
                    var result = await notifications.GetSentApplicationsNotification(userId);
                    return result.ToList();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpDelete("author/{authorId:int}/applications")]
        public async Task<ActionResult> DeleteReceivedApplicationsNotification(int authorId)
        {
            try
            {
                if (int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) == authorId)
                {
                    await notifications.DeleteReceivedApplicationsNotification(authorId);
                    return NoContent();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }

        [HttpDelete("user/{userId:int}/applications")]
        public async Task<ActionResult> DeleteSentApplicationsNotification(int userId)
        {
            try
            {
                if (int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) == userId)
                {
                    await notifications.DeleteSentApplicationsNotification(userId);
                    return NoContent();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
