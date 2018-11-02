using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Dtos.User
{
    public class UserRegisterDto
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public UserRegisterDto()
        {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}