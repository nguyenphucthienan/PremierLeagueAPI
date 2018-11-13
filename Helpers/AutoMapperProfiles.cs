using System;
using System.Linq;
using AutoMapper;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Goal;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Dtos.Player;
using PremierLeagueAPI.Dtos.Season;
using PremierLeagueAPI.Dtos.SquadPlayer;
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
            CreateMap<Season, SeasonBriefListDto>();

            CreateMap<Season, SeasonDetailDto>()
                .ForMember(sdd => sdd.Clubs, opt => opt
                    .MapFrom(s => s.SeasonClubs
                        .Select(sc => sc.Club).ToList()));

            CreateMap<SeasonCreateDto, Season>();
            CreateMap<SeasonUpdateDto, Season>();

            CreateMap<PaginatedList<Club>, PaginatedList<ClubListDto>>();
            CreateMap<Club, ClubBriefListDto>();
            CreateMap<Club, ClubDetailDto>();
            CreateMap<ClubCreateDto, Club>();
            CreateMap<ClubUpdateDto, Club>();

            CreateMap<SquadPlayer, SquadPlayerListDto>();

            CreateMap<Player, PlayerListDto>()
                .ForMember(pld => pld.Number, opt => opt
                    .ResolveUsing<int?>((src, dest, destMember, resContext) =>
                    {
                        if (!resContext.Items.ContainsKey("squadId"))
                            return null;

                        var squadId = (int) resContext.Items["squadId"];
                        var squad = src.SquadPlayers.SingleOrDefault(sp => sp.SquadId == squadId);
                        return squad?.Number;
                    }));

            CreateMap<PaginatedList<Player>, PaginatedList<PlayerListDto>>();
            CreateMap<Player, PlayerDetailDto>();
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