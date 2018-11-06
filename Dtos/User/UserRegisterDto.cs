using System;
using System.ComponentModel.DataAnnotations;

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