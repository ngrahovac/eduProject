using eduProjectModel.Domain;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IUserSettingsRepository
    {
        public Task<UserSettings> GetAsync(int userId);

        public Task UpdateAsync(UserSettings settings);
    }
}
