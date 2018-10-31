using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using PremierLeagueAPI.Models;

namespace PremierLeagueAPI.Persistence
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedRolesAndAdminUser()
        {
            if (_userManager.Users.Any())
                return;

            var roles = new List<Role>
            {
                new Role {Name = "Admin"},
                new Role {Name = "Moderator"},
                new Role {Name = "Member"}
            };

            foreach (var role in roles)
            {
                _roleManager.CreateAsync(role).Wait();
            }

            // Create admin user
            var adminUser = new User
            {
                UserName = "admin",
            };

            var result = _userManager.CreateAsync(adminUser, "password").Result;

            if (!result.Succeeded)
                return;

            var admin = _userManager.FindByNameAsync("admin").Result;
            _userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"}).Wait();
        }
    }
}