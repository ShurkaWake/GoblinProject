using DataAccess.Abstractions;

namespace DataAccess.Entities
{
    public class WorkingShift : IEntity<int>
    {
        public int Id { get; set; }

        public AppUser? Foreman { get; set; }

        public Business Business { get; set; }

        public DateTime Start { get; set; } = DateTime.Now;

        public DateTime? End { get; set; }

        public ICollection<Resource>? UsedResources { get; set; }

        public Measurement? Measurement { get; set; }
    }
}
