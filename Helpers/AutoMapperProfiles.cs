using AutoMapper;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Goal;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Dtos.Player;
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

            CreateMap<PaginatedList<Club>, PaginatedList<ClubListDto>>();
            CreateMap<Club, ClubBriefListDto>();
            CreateMap<Club, ClubDetailDto>();
            CreateMap<ClubCreateDto, Club>();
            CreateMap<ClubUpdateDto, Club>();

            CreateMap<Squad, SquadListDto>();

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