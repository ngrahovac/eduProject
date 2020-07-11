using eduProjectModel.Domain;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IProjectsRepository
    {
        public Task<Project> GetAsync(int id); // FIX: make long
    }
}
