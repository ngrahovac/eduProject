using System;
using System.Collections.Generic;

namespace eduProjectWebAPI.Model
{
    public partial class ProjectApplication
    {
        public int ProjectApplicationId { get; set; }
        public string ApplicantComment { get; set; }
        public string AuthorComment { get; set; }
        public int ProjectApplicationStatusId { get; set; }
        public int CollaboratorProfileId { get; set; }
        public int UserId { get; set; }

        public virtual CollaboratorProfile CollaboratorProfile { get; set; }
        public virtual ProjectApplicationStatus ProjectApplicationStatus { get; set; }
        public virtual User User { get; set; }
    }
}
