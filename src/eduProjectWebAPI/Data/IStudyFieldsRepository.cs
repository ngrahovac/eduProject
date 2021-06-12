using eduProjectModel.Domain;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public interface IStudyFieldsRepository
    {
        public Task AddAsync(StudyField field);
    }
}
