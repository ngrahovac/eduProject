namespace eduProjectModel.Domain
{
    public class ProjectApplication : IAggregateRoot
    {
        public int ProjectApplicationId { get; set; }

        public int ProjectId { get; set; }
        public int CollaboratorProfileId { get; set; }
        public int ApplicantId { get; set; }
        public string ApplicantComment { get; set; }
        public string AuthorComment { get; set; }
        public ProjectApplicationStatus ProjectApplicationStatus { get; set; }

    }
}
