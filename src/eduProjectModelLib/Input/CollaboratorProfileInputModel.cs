using eduProjectModel.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace eduProjectModel.Input
{
    public class CollaboratorProfileInputModel
    {
        public CollaboratorProfileType CollaboratorProfileType { get; set; }
        public string FacultyName { get; set; }
        public int? Cycle { get; set; }
        public string StudyProgramName { get; set; }
        public int? StudyYear { get; set; }
        public string StudyProgramSpecializationName { get; set; }
        public string StudyFieldName { get; set; }
        public string ActivityDescription { get; set; }

        public CollaboratorProfileInputModel()
        {

        }

        public void MapTo()
        {
            //da li je ovdje potrebno neko mapiranje?
        }
    }
}
