namespace eduProjectModel.Domain
{
    public class FacultyMemberProfile : CollaboratorProfile
    {
        public StudyField? StudyField { get; set; }

        public FacultyMemberProfile()
        {

        }
        public FacultyMemberProfile(Project project, string description, int? facultyId, StudyField studyField)
                                    : base(description, facultyId)
        {
            StudyField = studyField;
        }
    }
}
