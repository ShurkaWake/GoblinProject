using BusinessLogic.Core;
using BusinessLogic.Options;
using BusinessLogic.Services.Abstractions;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BusinessLogic.Services
{
    public sealed class Seeder : ISeeder
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SeederOptions _settings;

        public Seeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SeederOptions settings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _settings = settings;
        }

        public async Task<Result> SeedAsync()
        {
            return await SeedAdminAndRoles();
        }

        private async Task<Result> SeedAdminAndRoles()
        {
            var admin = await _userManager.Users
                        .Where(u => u.NormalizedUserName == _settings.AdminLogin.ToUpper())
                        .FirstOrDefaultAsync();

            if (admin is null)
            {
                admin = new AppUser { UserName = _settings.AdminLogin, Email = _settings.AdminEmail, EmailConfirmed = true };

                var adminResult = await _userManager.CreateAsync(admin, _settings.AdminPassword);

                if (adminResult.Succeeded == false)
                {
                    return Result.Fail("Unable to seed admin user");
                }
            }

            admin.EmailConfirmed = true;
            await _userManager.UpdateAsync(admin);
            var roles = await _userManager.GetRolesAsync(admin);
            var expectedRoles = Roles.AllowedRoles;

            foreach (var expectedRole in expectedRoles)
            {
                if (roles.Contains(expectedRole))
                {
                    continue;
                }

                var role = await _roleManager.FindByIdAsync(expectedRole);

                if (role is null)
                {
                    await _roleManager.CreateAsync(new AppRole { Name = expectedRole });
                }
            }

            await _userManager.AddToRoleAsync(admin, Roles.Admin);

            return Result.Ok();
        }
    }
}
