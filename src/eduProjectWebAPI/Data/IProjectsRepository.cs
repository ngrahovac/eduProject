using eduProjectModel.Domain;
using eduProjectModel.Input;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IProjectsRepository
    {
        public Task<Project> GetAsync(int id);

        public Task AddAsync(Project project);

        public Task UpdateAsync(Project project);
    }
}
