using System.Collections.Generic;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface INotificationsRepository
    {
        public Task<ICollection<int>> GetReceivedApplicationsNotification(int authorId);
        public Task DeleteReceivedApplicationsNotification(int authorId);
        public Task<ICollection<int>> GetSentApplicationsNotification(int userId);
        public Task DeleteSentApplicationsNotification(int userId);
    }
}
