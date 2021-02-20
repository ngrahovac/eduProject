using System.Collections.Generic;

namespace eduProjectModel.Domain
{
    public class StudyField
    {
        public static Dictionary<int, StudyField> fields = new Dictionary<int, StudyField>();
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
