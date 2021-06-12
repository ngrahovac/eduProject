using eduProjectModel.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IFacultiesRepository
    {
        public Task<Faculty> GetAsync(int id);

        public Task<ICollection<Faculty>> GetAllAsync();

        public Task AddAsync(Faculty faculty);

    }
}
