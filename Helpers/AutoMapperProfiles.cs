using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PremierLeagueAPI.Dtos;
using PremierLeagueAPI.Models;

namespace PremierLeagueAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<User, DetailUserDto>();
        }
    }
}