﻿using System;
using System.Linq;
using AutoMapper;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Dtos.Card;
using PremierLeagueAPI.Dtos.Club;
using PremierLeagueAPI.Dtos.Goal;
using PremierLeagueAPI.Dtos.Kit;
using PremierLeagueAPI.Dtos.Manager;
using PremierLeagueAPI.Dtos.Match;
using PremierLeagueAPI.Dtos.Player;
using PremierLeagueAPI.Dtos.Season;
using PremierLeagueAPI.Dtos.Squad;
using PremierLeagueAPI.Dtos.SquadManager;
using PremierLeagueAPI.Dtos.SquadPlayer;
using PremierLeagueAPI.Dtos.Stadium;
using PremierLeagueAPI.Dtos.Table;
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
            CreateMap<Season, SeasonDetailDto>();
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

            CreateMap<SquadManager, SquadManagerListDto>();
            CreateMap<SquadManagerUpdateDto, SquadManager>();

            CreateMap<SquadPlayer, SquadPlayerListDto>();
            CreateMap<SquadPlayerUpdateDto, SquadPlayer>();

            CreateMap<Kit, KitListDto>();
            CreateMap<Kit, KitDetailDto>();
            CreateMap<KitCreateDto, Kit>();
            CreateMap<KitUpdateDto, Kit>();

            CreateMap<PaginatedList<Manager>, PaginatedList<ManagerListDto>>();
            CreateMap<Manager, ManagerListDto>();
            CreateMap<Manager, ManagerBriefListDto>();

            CreateMap<Manager, ManagerDetailDto>()
                .ForMember(mdd => mdd.Club, opt => opt
                    .ResolveUsing((src, dest, destMember, context) =>
                    {
                        var squad = src.SquadManagers
                            .SingleOrDefault(sp => sp.StartDate == src.SquadManagers.Max(sms => sms.StartDate)
                                                   && (sp.EndDate == null || sp.EndDate > DateTime.Now));

                        return squad?.Squad.Club;
                    }));

            CreateMap<ManagerCreateDto, Manager>();
            CreateMap<ManagerUpdateDto, Manager>();

            CreateMap<PaginatedList<Player>, PaginatedList<PlayerListDto>>();
            CreateMap<Player, PlayerListDto>();
            CreateMap<Player, PlayerBriefListDto>();

            CreateMap<Player, PlayerDetailDto>()
                .ForMember(pdd => pdd.Club, opt => opt
                    .ResolveUsing((src, dest, destMember, context) =>
                    {
                        var squad = src.SquadPlayers
                            .SingleOrDefault(sp => sp.StartDate == src.SquadPlayers.Max(sps => sps.StartDate)
                                                   && (sp.EndDate == null || sp.EndDate > DateTime.Now));

                        return squad?.Squad.Club;
                    }))
                .ForMember(pdd => pdd.Number, opt => opt
                    .ResolveUsing((src, dest, destMember, context) =>
                    {
                        var squad = src.SquadPlayers
                            .SingleOrDefault(sp => sp.StartDate == src.SquadPlayers.Max(sps => sps.StartDate)
                                                   && (sp.EndDate == null || sp.EndDate > DateTime.Now));

                        return squad?.Number;
                    }));

            CreateMap<PlayerCreateDto, Player>();
            CreateMap<PlayerUpdateDto, Player>();

            CreateMap<PaginatedList<Match>, PaginatedList<MatchListDto>>();
            CreateMap<Match, MatchListDto>();
            CreateMap<Match, MatchBriefListDto>();
            CreateMap<Match, MatchDetailDto>();
            CreateMap<MatchCreateDto, Match>();
            CreateMap<MatchUpdateDto, Match>();

            CreateMap<PaginatedList<Goal>, PaginatedList<GoalListDto>>();
            CreateMap<Goal, GoalListDto>();
            CreateMap<Goal, GoalDetailDto>();
            CreateMap<GoalCreateDto, Goal>();
            CreateMap<GoalUpdateDto, Goal>();

            CreateMap<PaginatedList<Card>, PaginatedList<CardListDto>>();
            CreateMap<Card, CardListDto>();
            CreateMap<Card, GoalDetailDto>();
            CreateMap<CardCreateDto, Card>();
            CreateMap<CardUpdateDto, Card>();

            CreateMap<TableItem, TableItemDto>();
        }
    }
}