namespace eduProjectModel.Domain
{
    public class Student : User
    {
        public int FacultyId { get; set; }
        public int StudyYear { get; set; }
        public int StudyProgramId { get; set; }
        public int? StudyProgramSpecializationId { get; set; }
    }
}
