using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class Student
    {
        public int StudyYear { get; set; }
        public int UserId { get; set; }
        public int StudyProgramId { get; set; }
        public int? StudyProgramSpecializationId { get; set; }

        public virtual StudyProgram StudyProgram { get; set; }
        public virtual StudyProgramSpecialization StudyProgramSpecialization { get; set; }
        public virtual User User { get; set; }
    }
}
