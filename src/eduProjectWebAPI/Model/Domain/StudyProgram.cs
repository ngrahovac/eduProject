﻿using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public class StudyProgram : IValueObject
    {
        public string Name { get; set; }
        public byte Cycle { get; set; }
        public byte DurationYears { get; set; }
        public ICollection<StudyProgramSpecialization> StudyProgramSpecializations { get; set; }

        public StudyProgram(string name, byte cycle, byte durationYears)
        {
            Name = name;
            Cycle = cycle;
            DurationYears = durationYears;
            StudyProgramSpecializations = new HashSet<StudyProgramSpecialization>();
        }

        public StudyProgram()
        {
            StudyProgramSpecializations = new HashSet<StudyProgramSpecialization>();
        }

        public void AddProgramSpecialization(StudyProgramSpecialization specialization)
        {
            StudyProgramSpecializations.Add(specialization);
        }
        // ograniciti smjerove?
    }
}
