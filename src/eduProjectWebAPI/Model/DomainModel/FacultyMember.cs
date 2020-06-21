using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class FacultyMember
    {
        public int UserId { get; set; }
        public int FacultyId { get; set; }
        public int StudyFieldId { get; set; }
        public int AcademicRankId { get; set; }

        public virtual AcademicRank AcademicRank { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual StudyField StudyField { get; set; }
        public virtual User User { get; set; }
    }
}
