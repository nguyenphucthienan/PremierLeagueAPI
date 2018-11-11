﻿using System.Linq;
using AutoMapper;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Goal;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Dtos.Player;
using PremierLeagueAPI.Dtos.Season;
using PremierLeagueAPI.Dtos.Squad;
using PremierLeagueAPI.Dtos.User;

namespace PremierLeagueAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<User, UserDetailDto>();

            CreateMap<Season, SeasonListDto>();
            CreateMap<Season, SeasonDetailDto>();
            CreateMap<SeasonCreateDto, Season>();
            CreateMap<SeasonUpdateDto, Season>();

            CreateMap<PaginatedList<Club>, PaginatedList<ClubListDto>>();
            CreateMap<Club, ClubBriefListDto>();
            CreateMap<Club, ClubDetailDto>();
            CreateMap<ClubCreateDto, Club>();
            CreateMap<ClubUpdateDto, Club>();

            CreateMap<Squad, SquadListDto>();

            CreateMap<PaginatedList<Player>, PaginatedList<PlayerListDto>>();

            CreateMap<Player, PlayerDetailDto>()
                .ForMember(pdd => pdd.Squads, opt => opt
                    .MapFrom(p => p.SquadPlayers
                        .Select(sp => sp.Squad).ToList()));

            CreateMap<PlayerCreateDto, Player>();
            CreateMap<PlayerUpdateDto, Player>();

            CreateMap<PaginatedList<Match>, PaginatedList<MatchListDto>>();

            CreateMap<Match, MatchDetailDto>();
            CreateMap<MatchUpdateDto, Match>();

            CreateMap<GoalCreateDto, Goal>();
            CreateMap<GoalUpdateDto, Goal>();
            CreateMap<Goal, GoalListDto>();
            CreateMap<Goal, GoalDetailDto>();
        }
    }
}