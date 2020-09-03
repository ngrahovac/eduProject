using System;
using System.Collections.Generic;

namespace eduProjectModel.Domain
{
    public class Project : IAggregateRoot
    {
        public int ProjectId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string Description { get; set; }
        public StudyField StudyField { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<CollaboratorProfile> CollaboratorProfiles { get; set; } = new HashSet<CollaboratorProfile>();
        public ICollection<int> CollaboratorIds { get; set; } = new HashSet<int>();
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        
        public Project(int projectId, int authorId, string title, string description, StudyField studyField, DateTime? startDate, DateTime? endDate)
        {
            ProjectId = projectId;
            AuthorId = authorId;
            Title = title;
            Description = description;
            StudyField = studyField;
            StartDate = startDate;
            EndDate = endDate;
        }
        public Project()
        {

        }
        public void AddCollaboratorProfile(CollaboratorProfile profile)
        {
            CollaboratorProfiles.Add(profile);
        }

        public void AddCollaboratorId(int id)
        {
            CollaboratorIds.Add(id);
        }

        public void AddTag(Tag tag)
        {
            Tags.Add(tag);
        }
    }
}
