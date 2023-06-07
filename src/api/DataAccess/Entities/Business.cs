using DataAccess.Abstractions;

namespace DataAccess.Entities
{
    public class Business : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<AppUser>? Users { get; set; }

        public IEnumerable<WorkingShift>? WorkingShifts { get; set; }
        
        public IEnumerable<Scales>? Scales { get; set; }

        public IEnumerable<Resource>? Resources { get; set; }
    }
}
