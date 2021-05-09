using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class NewsRepository : INewsRepository
    {
        private readonly DbConnectionParameters dbConnectionString;

        public NewsRepository(DbConnectionParameters dbConnectionString) => this.dbConnectionString = dbConnectionString;

        public async Task AddAsync(News news)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<News>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
