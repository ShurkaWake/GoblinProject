using DataAccess.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities
{
    public class AppUser : IdentityUser, IEntity<string>
    {
        public AppRole Role { get; set; }

        public Business Job { get; set; }

        public IEnumerable<WorkingShift>? WorkingShifts { get; set;}
    }
}
