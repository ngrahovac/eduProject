using System.Collections.Generic;
using System.Threading.Tasks;

namespace eduProjectModel.Domain
{
    public class Tag : IValueObject
    {
        public static Dictionary<int, Tag> tags = new Dictionary<int, Tag>();

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
