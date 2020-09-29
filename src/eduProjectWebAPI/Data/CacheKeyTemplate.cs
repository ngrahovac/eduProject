using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public static class CacheKeyTemplate
    {
        public static readonly string Faculty = "faculty_";
        public static readonly string Program = "program_";
        public static readonly string Specialization = "specialization_";
        public static readonly string Field = "field_";
        public static readonly string Tag = "tag_";
    }
}
