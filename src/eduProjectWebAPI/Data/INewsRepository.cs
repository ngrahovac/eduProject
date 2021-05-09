using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface INewsRepository
    {
        public Task<ICollection<News>> GetAllAsync();

        public Task AddAsync(News news);
    }
}
