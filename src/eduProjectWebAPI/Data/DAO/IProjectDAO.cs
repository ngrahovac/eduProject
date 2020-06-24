using eduProjectWebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data.DAO
{
    public interface IProjectDAO
    {
        Project Create(Project project);
        Task<Project> Read(long id);
        List<Project> ReadAll(Predicate<Project> predicate);
        Project Update(Project project);
        void Delete(Project project);
    }
}
