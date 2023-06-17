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

        /// <summary>
        /// Ammortization per hour of work
        /// </summary>
        public MoneyAmount Ammortization { get; set; }

        public ResourceStatus Status { get; set; } = ResourceStatus.Free;

        public override string ToString()
        {
            return Name;
        }
    }
}
