using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IUsersRepository
    {
        public Task<User> GetAsync(int id);
    }
}
