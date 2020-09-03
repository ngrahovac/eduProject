namespace eduProjectModel.Domain
{
    public class Student : User
    {
        public int StudyYear { get; set; }
        public StudyProgram StudyProgram { get; set; }
        public StudyProgramSpecialization StudyProgramSpecialization { get; set; }
    }
}
