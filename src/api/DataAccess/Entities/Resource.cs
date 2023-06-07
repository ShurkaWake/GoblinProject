using DataAccess.Abstractions;
using DataAccess.Enums;

namespace DataAccess.Entities
{
    public class Resource : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Business Business { get; set; }
        
        public ResourceStatus status { get; set; }
    }
}
