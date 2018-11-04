using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace PremierLeagueAPI.Core.Models
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}