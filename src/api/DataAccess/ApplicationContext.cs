using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
        {
        }

        public DbSet<Business> Businesses { get; set; }

        public DbSet<Measurement> Measurements { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Scales> Scales { get; set; }

        public DbSet<WorkingShift> WorkingShifts { get; set;}
    }
}
