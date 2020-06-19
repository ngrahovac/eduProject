using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Data
{
    public partial class StudentProfile
    {
        public int? Cycle { get; set; }
        public int? StudyYear { get; set; }
        public int CollaboratorProfileId { get; set; }
        public int? FacultyId { get; set; }
        public int? StudyProgramId { get; set; }
        public int? StudyProgramSpecializationId { get; set; }

        public virtual CollaboratorProfile CollaboratorProfile { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual StudyProgram StudyProgram { get; set; }
        public virtual StudyProgramSpecialization StudyProgramSpecialization { get; set; }
    }
}
