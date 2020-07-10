namespace eduProjectWebAPI.Model
{
    public class Student : User
    {
        public int StudyYear { get; private set; }
        public StudyProgram StudyProgram { get; private set; }
        public StudyProgramSpecialization StudyProgramSpecialization { get; private set; }

        public Student(string firstName, string lastName, string phoneNumber, string phoneFormat, Account account, UserSettings userSettings,
                        int studyYear, StudyProgram studyProgram, StudyProgramSpecialization studyProgramSpecialization)
                      : base(firstName, lastName, phoneNumber, phoneFormat)
        {
            StudyYear = studyYear;
            StudyProgram = studyProgram;
            StudyProgramSpecialization = studyProgramSpecialization;
        }

    }
}
