using eduProjectModel.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface INewsRepository
    {
        public Task<News> GetByIdAsync(int id);

        public Task<ICollection<News>> GetAllAsync();

        public Task AddAsync(News news);

        public Task DeleteAsync(News news);
    }
}
