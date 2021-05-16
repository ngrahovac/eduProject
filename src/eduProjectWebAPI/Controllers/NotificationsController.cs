using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Controllers
{
    [ApiController]
    [Route("notifications")]
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
            var result = await notifications.GetReceivedApplicationsNotification(authorId);
            return result.ToList();
        }

        [HttpGet("user/{userId:int}/applications")]
        public async Task<ActionResult<ICollection<int>>> GetSentApplicationsNotification(int userId)
        {
            var result = await notifications.GetSentApplicationsNotification(userId);
            return result.ToList();
        }

        [HttpDelete("author/{authorId:int}/applications")]
        public async Task<ActionResult> DeleteReceivedApplicationsNotification(int authorId)
        {
            await notifications.DeleteReceivedApplicationsNotification(authorId);
            return NoContent();
        }

        [HttpDelete("user/{userId:int}/applications")]
        public async Task<ActionResult> DeleteSentApplicationsNotification(int userId)
        {
            await notifications.DeleteSentApplicationsNotification(userId);
            return NoContent();
        }
    }
}
