using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IProjectApplicationsRepository
    {
        public Task AddAsync(ProjectApplication application);

        public Task<ProjectApplication> GetById(int id);

        public Task<ICollection<ProjectApplication>> GetByApplicantId(int applicantId);

        public Task<ICollection<ProjectApplication>> GetByProjectId(int projectId);

        public Task Update(ProjectApplication application);

        public Task Delete(ProjectApplication application);

    }
}
