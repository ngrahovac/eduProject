namespace eduProjectModel.Domain
{
    public class StudentProfile : CollaboratorProfile
    {
        public Faculty Faculty { get; set; }
        public StudyProgram StudyProgram { get; set; }
        public StudyProgramSpecialization StudyProgramSpecialization { get; set; }
        public int? StudyCycle { get; set; }
        public int? StudyYear { get; set; }

        public StudentProfile()
        {

        }

        public StudentProfile(Project project, string description,
                              Faculty faculty, StudyProgram studyProgram, StudyProgramSpecialization studyProgramSpecialization,
                              int cycle, int studyYear)
                              : base(description)
        {
            Faculty = faculty;
            StudyProgram = studyProgram;
            StudyProgramSpecialization = studyProgramSpecialization;

            StudyCycle = cycle;
            StudyYear = studyYear;
        }
    }
}
