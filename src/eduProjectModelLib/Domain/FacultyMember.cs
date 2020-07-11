namespace eduProjectModel.Domain
{
    public class FacultyMember : User
    {
        public AcademicRank AcademicRank { get; private set; }
        public Faculty Faculty { get; private set; }
        public StudyField StudyField { get; private set; }

        public FacultyMember(string firstName, string lastName, string phoneNumber, string phoneFormat, Account account, UserSettings userSettings,
                                  AcademicRank academicRank, Faculty faculty, StudyField studyField)
                                : base(firstName, lastName, phoneNumber, phoneFormat)
        {
            AcademicRank = academicRank;
            Faculty = faculty;
            StudyField = studyField;
        }
    }
}
