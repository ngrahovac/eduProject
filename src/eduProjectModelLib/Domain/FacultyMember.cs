namespace eduProjectModel.Domain
{
    public class FacultyMember : User
    {
        public AcademicRank AcademicRank { get; set; }
        public StudyField StudyField { get; set; }
    }
}
