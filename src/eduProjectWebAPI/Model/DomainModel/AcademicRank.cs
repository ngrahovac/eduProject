using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class AcademicRank
    {
        public AcademicRank()
        {
            FacultyMember = new HashSet<FacultyMember>();
        }

        public int AcademicRankId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<FacultyMember> FacultyMember { get; set; }
    }
}
