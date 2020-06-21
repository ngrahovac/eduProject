using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class FacultyMemberProfile
    {
        public int CollaboratorProfileId { get; set; }
        public int? FacultyId { get; set; }
        public int? StudyFieldId { get; set; }

        public virtual CollaboratorProfile CollaboratorProfile { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual StudyField StudyField { get; set; }
    }
}
