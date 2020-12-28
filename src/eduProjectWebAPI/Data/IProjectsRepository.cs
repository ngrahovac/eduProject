using eduProjectModel.Domain;
using eduProjectModel.Input;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IProjectsRepository
    {
        public Task<Project> GetAsync(int id);

        public Task<ICollection<Project>> GetAllAsync();

        public Task<ICollection<Project>> GetAllByAuthorAsync(int authorId);

        public Task AddAsync(Project project);

        public Task UpdateAsync(Project project);

        public Task DeleteAsync(Project project);
    }
}
