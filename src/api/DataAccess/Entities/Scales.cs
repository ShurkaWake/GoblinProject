using DataAccess.Abstractions;

namespace DataAccess.Entities
{
    public class Scales : IEntity<int>
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; }

        public Business Business { get; set; }

        public ICollection<Measurement> Measurements { get; set; }
    }
}
