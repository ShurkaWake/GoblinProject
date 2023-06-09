﻿using DataAccess.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities
{
    public class AppUser : IdentityUser, IEntity<string>
    {
        public Business Job { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public ICollection<WorkingShift>? WorkingShifts { get; set;}

        public override string ToString()
        {
            return FullName;
        }
    }
}
