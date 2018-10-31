using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Dtos
{
    public class DetailUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}