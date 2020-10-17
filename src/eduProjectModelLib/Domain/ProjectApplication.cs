namespace eduProjectModel.Domain
{
    public class ProjectApplication : IAggregateRoot
    {
        public int ProjectApplicationId { get; set; }
        public int ApplicantId { get; set; }
        public string ApplicantComment { get; set; }
        public string AuthorComment { get; set; }
        public virtual int CollaboratorProfileId { get; set; }
        public virtual ProjectApplicationStatus ProjectApplicationStatus { get; set; }

    }
}
