using System.Linq;
using AutoMapper;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Goal;
using PremierLeagueAPI.Dtos.Kit;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Dtos.Player;
using PremierLeagueAPI.Dtos.Season;
using PremierLeagueAPI.Dtos.Squad;
using PremierLeagueAPI.Dtos.SquadPlayer;
using PremierLeagueAPI.Dtos.Stadium;
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

            CreateMap<PaginatedList<Season>, PaginatedList<SeasonListDto>>();
            CreateMap<Season, SeasonBriefListDto>();

            CreateMap<Season, SeasonDetailDto>()
                .ForMember(sdd => sdd.Clubs, opt => opt
                    .MapFrom(s => s.SeasonClubs
                        .Select(sc => sc.Club).ToList()));

            CreateMap<SeasonCreateDto, Season>();
            CreateMap<SeasonUpdateDto, Season>();

            CreateMap<PaginatedList<Stadium>, PaginatedList<StadiumListDto>>();
            CreateMap<Stadium, StadiumBriefListDto>();
            CreateMap<Stadium, StadiumDetailDto>();
            CreateMap<StadiumCreateDto, Stadium>();
            CreateMap<StadiumUpdateDto, Stadium>();

            CreateMap<PaginatedList<Club>, PaginatedList<ClubListDto>>();
            CreateMap<Club, ClubBriefListDto>();
            CreateMap<Club, ClubDetailDto>();
            CreateMap<ClubCreateDto, Club>();
            CreateMap<ClubUpdateDto, Club>();

            CreateMap<PaginatedList<Squad>, PaginatedList<SquadListDto>>();
            CreateMap<Squad, SquadListDto>();
            CreateMap<Squad, SquadDetailDto>();
            CreateMap<SquadCreateDto, Squad>();
            CreateMap<SquadUpdateDto, Squad>();

            CreateMap<SquadPlayer, SquadPlayerListDto>();

            CreateMap<Kit, KitListDto>();

            CreateMap<PaginatedList<Player>, PaginatedList<PlayerListDto>>();
            CreateMap<PaginatedList<Player>, PaginatedList<PlayerSquadListDto>>();
            CreateMap<Player, PlayerListDto>();

            CreateMap<Player, PlayerDetailDto>()
                .ForMember(pdd => pdd.Club, opt => opt
                    .ResolveUsing((src, dest, destMember, context) =>
                    {
                        var squad = src.SquadPlayers
                            .SingleOrDefault(sp => sp.StartDate == src.SquadPlayers.Max(sps => sps.StartDate));

                        return squad?.Squad.Club;
                    }))
                .ForMember(pdd => pdd.Number, opt => opt
                    .ResolveUsing((src, dest, destMember, context) =>
                    {
                        var squad = src.SquadPlayers
                            .SingleOrDefault(sp => sp.StartDate == src.SquadPlayers.Max(sps => sps.StartDate));

                        return squad?.Number;
                    }));

            CreateMap<Player, PlayerSquadListDto>()
                .ForMember(pld => pld.Number, opt => opt
                    .ResolveUsing((src, dest, destMember, context) =>
                    {
                        if (!context.Items.ContainsKey("squadId"))
                            return null;

                        var squadId = (int) context.Items["squadId"];
                        var squad = src.SquadPlayers.SingleOrDefault(sp => sp.SquadId == squadId);

                        return squad?.Number;
                    }));

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