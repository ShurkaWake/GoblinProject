using DataAccess.Abstractions;

namespace DataAccess.Entities
{
    public class Business : IEntity<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public ICollection<AppUser>? Users { get; set; }

        public ICollection<WorkingShift>? WorkingShifts { get; set; }
        
        public ICollection<Scales>? Scales { get; set; }

        public ICollection<Resource>? Resources { get; set; }
    }
}
