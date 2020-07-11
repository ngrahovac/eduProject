namespace eduProjectModel.Domain
{
    public class ProjectApplication : IAggregateRoot
    {
        public int ProjectApplicationId { get; private set; }
        public string ApplicantComment { get; private set; }
        public string AuthorComment { get; private set; }
        public virtual int CollaboratorProfileId { get; private set; }
        public virtual ProjectApplicationStatus ProjectApplicationStatus { get; private set; }
        public ProjectApplication(string applicantComment, string authorComment, int collaboratorProfileId, ProjectApplicationStatus status)
        {
            ApplicantComment = applicantComment;
            AuthorComment = authorComment;
            CollaboratorProfileId = collaboratorProfileId;
            ProjectApplicationStatus = status;
        }
    }
}
