using eduProjectModel.Domain;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IUsersRepository
    {
        public Task<User> GetAsync(int id);

        public Task AddAsync(User user);
        public Task UpdateAsync(User user);

    }
}
