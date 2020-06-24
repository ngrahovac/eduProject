using eduProjectWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eduProjectWebAPI.Data.DAO
{
    public class ProjectDAOMySQL : IProjectDAO
    {
        private EduProjectDbContext dbContext;

        public ProjectDAOMySQL(EduProjectDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public Project Create(Project project)
        {
            throw new NotImplementedException();
        }

        public void Delete(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<Project> Read(long id)
        {
            var project = await dbContext.Projects
                .Include(x => x.ProjectStatus)
                .Include(x => x.StudyField)
                .Include(x => x.User)
                .Include(x => x.CollaboratorProfiles)
                .Include(x => x.ProjectCollaborators)
                .Include(x => x.ProjectTags)
                .SingleOrDefaultAsync(x => x.ProjectId == id);

            return project;
        }

        public List<Project> ReadAll(Predicate<Project> predicate)
        {
            throw new NotImplementedException();
        }

        public Project Update(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
