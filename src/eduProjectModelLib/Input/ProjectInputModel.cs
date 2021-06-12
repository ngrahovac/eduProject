using eduProjectModel.Display;
using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace eduProjectModel.Input
{
    public class ProjectInputModel
    {
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        public string Title { get; set; }

        private ProjectStatus projectStatus;

        [Required(ErrorMessage = "Status projekta je obavezan.")]
        public ProjectStatus ProjectStatus
        {
            get { return projectStatus; }
            set
            {
                projectStatus = value;
                ProjectStatusNum = (int)projectStatus;
            }
        }

        [Range(1, 3, ErrorMessage = "Status projekta je obavezan.")]
        public int? ProjectStatusNum { get; set; } = 0;

        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Polje ne može biti prazno.")]
        public string StudyFieldName { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<string> TagNames { get; set; } = new HashSet<string>();

        [MinLength(1, ErrorMessage = "Nije naveden niti jedan profil traženog saradnika.")]
        public List<CollaboratorProfileInputModel> CollaboratorProfileInputModels { get; set; } = new List<CollaboratorProfileInputModel>();

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
                collaboratorProfileInputModel.ExistingProfile = true;
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
            
            project.CollaboratorProfiles.Clear();

            foreach (var model in CollaboratorProfileInputModels)
            {
                if (model.CollaboratorProfileType == CollaboratorProfileType.Student)
                {
                    StudentProfile studentProfile = new StudentProfile();

                    if (model.FacultyName != null)
                    {
                        model.MapTo(studentProfile, faculties.Where(f => f.Name == model.FacultyName).First());
                    }
                    else
                    {
                        model.MapTo(studentProfile, null);
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

            project.Tags.Clear();
            foreach (var tagName in TagNames)
                project.Tags.Add(Tag.tags.Values.Where(tag => tag.Name.Equals(tagName)).First());
        }
    }
}
