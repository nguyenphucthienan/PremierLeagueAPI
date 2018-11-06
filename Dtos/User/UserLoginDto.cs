using System.ComponentModel.DataAnnotations;

namespace PremierLeagueAPI.Dtos.User
{
    public class UserLoginDto
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }
    }
}