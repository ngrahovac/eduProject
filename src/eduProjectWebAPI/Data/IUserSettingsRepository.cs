using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IUserSettingsRepository
    {
        public Task<UserSettings> GetAsync(int userId);

        public Task UpdateAsync(UserSettings settings);
    }
}
