using eduProjectModel.Display;
using eduProjectModel.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eduProjectModel.Input
{
    public class ProjectInputModel
    {
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string Description { get; set; }
        public string StudyFieldName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<CollaboratorProfileInputModel> CollaboratorProfileInputModels { get; set; } = new List<CollaboratorProfileInputModel>();
        public ICollection<int> CollaboratorIds { get; set; } = new HashSet<int>();
        public ICollection<string> TagNames { get; set; } = new HashSet<string>();

        public ProjectInputModel()
        {

        }

        public ProjectInputModel(ProjectDisplayModel model)
        {
            Title = model.Title;
            ProjectStatus = model.ProjectStatus;
            Description = model.Description;
            StudyFieldName = model.StudyField.Name;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            TagNames = model.Tags.Select(t => t.Name).ToHashSet();

            CollaboratorProfileInputModels = new List<CollaboratorProfileInputModel>();
            List<CollaboratorProfileDisplayModel> collaboratorProfileDisplayModels = new List<CollaboratorProfileDisplayModel>();
            foreach (var p in model.StudentProfileDisplayModels)
            {
                collaboratorProfileDisplayModels.Add(p);
            }
            foreach (var p in model.FacultyMemberProfileDisplayModels)
            {
                collaboratorProfileDisplayModels.Add(p);
            }

            foreach (var profileDisplayModel in collaboratorProfileDisplayModels)
            {
                var collaboratorProfileInputModel = CollaboratorProfileInputModel.FromCollaboratorProfileDisplayModel(profileDisplayModel);
                collaboratorProfileInputModel.AddedOnCreate = true;
                CollaboratorProfileInputModels.Add(collaboratorProfileInputModel);
            }
        }

        public void MapTo(Project project, ICollection<Faculty> faculties)
        {
            project.AuthorId = AuthorId;
            project.Title = Title;
            project.ProjectStatus = ProjectStatus;
            project.Description = Description;
            project.StartDate = StartDate;
            project.EndDate = EndDate;
            project.StudyField = StudyField.fields.Values.ToList().Where(sf => sf.Name == StudyFieldName).First();

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

            foreach (var tagName in TagNames)
                project.Tags.Add(Tag.tags.Values.Where(tag => tag.Name.Equals(tagName)).First());

            foreach (var id in CollaboratorIds)
                project.CollaboratorIds.Add(id);
        }
    }
}
