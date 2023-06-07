﻿using DataAccess.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace DataAccess.Entities
{
    public class AppRole : IdentityRole, IEntity<string>
    {
        public IEnumerable<AppUser>? Users { get; set; }
    }
}
