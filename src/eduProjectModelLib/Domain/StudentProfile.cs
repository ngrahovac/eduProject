namespace eduProjectModel.Domain
{
    public class StudentProfile : CollaboratorProfile
    {
        public int? FacultyId { get; set; }
        public int? StudyProgramId { get; set; }
        public int? StudyProgramSpecializationId { get; set; }
        public int? StudyCycle { get; set; } // edu2: REMOVE
        public int? StudyYear { get; set; }

        public StudentProfile()
        {

        }

        public StudentProfile(Project project, string description,
                              int? facultyId, int? studyProgramId, int? studyProgramSpecializationId,
                              int? cycle, int? studyYear)
                              : base(description)
        {
            FacultyId = facultyId;
            StudyProgramId = studyProgramId;
            StudyProgramSpecializationId = studyProgramSpecializationId;
            StudyCycle = cycle;
            StudyYear = studyYear;
        }
    }
}
