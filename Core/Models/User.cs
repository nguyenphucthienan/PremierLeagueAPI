using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PremierLeagueAPI.Core.Models
{
    public class User : IdentityUser<int>
    {
        public byte[] PasswordSalt { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}