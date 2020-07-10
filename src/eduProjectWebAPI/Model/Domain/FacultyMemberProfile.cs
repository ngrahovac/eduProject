namespace eduProjectWebAPI.Model
{
    public class FacultyMemberProfile : CollaboratorProfile
    {
        public Faculty Faculty { get; set; }
        public StudyField StudyField { get; set; }

        public FacultyMemberProfile()
        {

        }
        public FacultyMemberProfile(Project project, string description, Faculty faculty, StudyField studyField)
                                    : base(description)
        {
            Faculty = faculty;
            StudyField = studyField;
        }
    }
}
