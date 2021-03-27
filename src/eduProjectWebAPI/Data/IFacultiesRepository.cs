using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
