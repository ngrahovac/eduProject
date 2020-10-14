using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eduProjectModel.Input
{
    public class ProjectInputModel
    {
        public string Title { get; set; }
        public string StudyFieldName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public ICollection<string> TagNames { get; set; } = new List<string>();
        public ICollection<CollaboratorProfileInputModel> CollaboratorProfileInputModels { get; set; } = new List<CollaboratorProfileInputModel>();

        public ProjectInputModel()
        {

        }

        public void MapTo(Project project, ICollection<Faculty> faculties)
        {
            project.Title = Title;
            project.StartDate = StartDate;
            project.EndDate = EndDate;
            project.StudyField.Name = StudyFieldName;
            project.Description = Description;
            project.ProjectStatus = ProjectStatus.Active;

            foreach (string tagName in TagNames)
                project.Tags.Add(Tag.tags.Values.Where(tag => tag.Name.Equals(tagName)).FirstOrDefault());

            foreach (var model in CollaboratorProfileInputModels)
            {
                if (model.CollaboratorProfileType == CollaboratorProfileType.Student)
                {
                    StudentProfile studentProfile = new StudentProfile();

                    foreach (var faculty in faculties)
                    {
                        if (faculty.Name.Equals(model.FacultyName)) //assumption: unique faculty names
                            model.MapTo(studentProfile, faculty);
                    }

                    project.CollaboratorProfiles.Add(studentProfile);
                }
                else if (model.CollaboratorProfileType == CollaboratorProfileType.FacultyMember)
                {
                    FacultyMemberProfile facultyMemberProfile = new FacultyMemberProfile();

                    model.MapTo(facultyMemberProfile);
                    project.CollaboratorProfiles.Add(facultyMemberProfile);
                }
            }
        }
    }
}
