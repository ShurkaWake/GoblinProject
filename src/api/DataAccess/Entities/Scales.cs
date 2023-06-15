using DataAccess.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities
{
    [Index(nameof(SerialNumber), IsUnique = true)]
    public class Scales : IEntity<int>
    {
        public int Id { get; set; }

        public string SerialNumber { get; set; }

        public Business Business { get; set; }

        public ICollection<Measurement> Measurements { get; set; }
    }
}
