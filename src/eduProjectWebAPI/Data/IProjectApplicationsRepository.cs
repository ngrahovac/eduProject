using eduProjectModel.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IProjectApplicationsRepository
    {
        public Task AddAsync(ProjectApplication application);

        public Task<ProjectApplication> GetAsync(int id);

        public Task<ICollection<ProjectApplication>> GetByApplicantIdAsync(int applicantId);

        public Task<ICollection<ProjectApplication>> GetByProjectIdAsync(int projectId);

        public Task<ICollection<ProjectApplication>> GetByAuthorIdAsync(int authorId);

        public Task UpdateAsync(ProjectApplication application);

        public Task DeleteAsync(ProjectApplication application);

    }
}
