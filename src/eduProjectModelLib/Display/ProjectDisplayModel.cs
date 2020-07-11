using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectModel.Display
{
    public class ProjectDisplayModel
    {
        public int ProjectId { get; private set; }
        public string ProjectStatus { get; private set; }
        public string Title { get; private set; }
        public string AuthorFullName { get; private set; }
        public StudyField StudyField { get; private set; }
        public string Description { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public ICollection<FacultyMemberProfileDisplayModel> FacultyMemberProfileDisplayModels { get; private set; } = new HashSet<FacultyMemberProfileDisplayModel>();// ZORANE bolji naziv?
        public ICollection<StudentProfileDisplayModel> StudentProfileDisplayModels { get; private set; } = new HashSet<StudentProfileDisplayModel>();
        public ICollection<Tag> Tags { get; private set; } = new HashSet<Tag>();

        public static ProjectDisplayModel FromProject(Project project, User author)
        {
            ProjectDisplayModel model = new ProjectDisplayModel();

            model.ProjectId = project.ProjectId;
            model.ProjectStatus = project.ProjectStatus.ToString();
            model.Title = project.Title;
            if (author != null)
                model.AuthorFullName = $"{author.FirstName} {author.LastName}";
            model.StudyField = project.StudyField;
            model.Description = project.Description;
            model.StartDate = project.StartDate;
            model.EndDate = project.EndDate;

            foreach (var tag in project.Tags)
                model.Tags.Add(tag);

            foreach (var profile in project.CollaboratorProfiles)
            {
                if (profile is StudentProfile)
                {
                    model.StudentProfileDisplayModels.Add(StudentProfileDisplayModel.FromStudentProfile((StudentProfile)profile));
                }
                else if (profile is FacultyMemberProfile)
                {
                    model.FacultyMemberProfileDisplayModels.Add(FacultyMemberProfileDisplayModel.FromFacultyMemberProfile((FacultyMemberProfile)profile));
                }
            }

            return model;
        }

        private void AddCollaboratorProfileDisplayModel(CollaboratorProfileDisplayModel model)
        {
            if (model is StudentProfileDisplayModel)
            {
                StudentProfileDisplayModels.Add((StudentProfileDisplayModel)model);
            }
            else
            {
                FacultyMemberProfileDisplayModels.Add((FacultyMemberProfileDisplayModel)model);
            }
        }



    }
}
