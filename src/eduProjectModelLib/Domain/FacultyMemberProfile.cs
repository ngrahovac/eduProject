namespace eduProjectModel.Domain
{
    public class FacultyMemberProfile : CollaboratorProfile
    {
        public int? FacultyId { get; set; }
        public StudyField? StudyField { get; set; }

        public FacultyMemberProfile()
        {

        }
        public FacultyMemberProfile(Project project, string description, int? facultyId, StudyField studyField)
                                    : base(description)
        {
            StudyField = studyField;
            FacultyId = facultyId;
        }
    }
}
