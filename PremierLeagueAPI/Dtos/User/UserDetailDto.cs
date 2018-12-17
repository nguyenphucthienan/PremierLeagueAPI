using System;

namespace PremierLeagueAPI.Dtos.User
{
    public class UserDetailDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}