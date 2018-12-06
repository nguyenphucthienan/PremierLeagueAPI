using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PremierLeagueAPI.Core;
using PremierLeagueAPI.Core.Models;
using PremierLeagueAPI.Core.Repositories;

namespace PremierLeagueAPI.Persistence
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IStadiumRepository _stadiumRepository;
        private readonly IClubRepository _clubRepository;
        private readonly ISquadRepository _squadRepository;
        private readonly IKitRepository _kitRepository;
        private readonly IManagerRepository _managerRepository;
        private readonly IPlayerRepository _playerRepository;

        public Seed(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork,
            ISeasonRepository seasonRepository,
            IStadiumRepository stadiumRepository,
            IClubRepository clubRepository,
            ISquadRepository squadRepository,
            IKitRepository kitRepository,
            IManagerRepository managerRepository,
            IPlayerRepository playerRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _seasonRepository = seasonRepository;
            _stadiumRepository = stadiumRepository;
            _clubRepository = clubRepository;
            _squadRepository = squadRepository;
            _kitRepository = kitRepository;
            _managerRepository = managerRepository;
            _playerRepository = playerRepository;
        }

        public void SeedData()
        {
            SeedRolesAndAdminUser();
            SeedSeasons();
            SeedStadiums();
            SeedClubs();
            SeedSeasonClubs();
            SeedSquads();
            SeedKits();
            SeedManagers();
            SeedPlayers();
        }

        private void SeedRolesAndAdminUser()
        {
            if (_userManager.Users.Any())
                return;

            var roles = new List<Role>
            {
                new Role {Name = "Admin"},
                new Role {Name = "Moderator"},
                new Role {Name = "Member"}
            };

            foreach (var role in roles)
            {
                _roleManager.CreateAsync(role).Wait();
            }

            // Create admin user
            var adminUser = new User
            {
                UserName = "admin",
                Created = DateTime.Now,
                LastActive = DateTime.Now
            };

            var result = _userManager.CreateAsync(adminUser, "password").Result;

            if (!result.Succeeded)
                return;

            var admin = _userManager.FindByNameAsync("admin").Result;
            _userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"}).Wait();
        }

        private void SeedSeasons()
        {
            var season = new Season {Name = "2018/2019"};

            _seasonRepository.Add(season);
            _unitOfWork.CompleteAsync().Wait();
        }

        private void SeedStadiums()
        {
            var stadiumsData = File.ReadAllText("Persistence/Data/Stadiums.json");
            var stadiums = JsonConvert.DeserializeObject<List<Stadium>>(stadiumsData);

            _stadiumRepository.AddRange(stadiums);
            _unitOfWork.CompleteAsync().Wait();
        }

        private void SeedClubs()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var clubsData = File.ReadAllText("Persistence/Data/Clubs.json");
            var clubs = JsonConvert.DeserializeObject<JArray>(clubsData);

            foreach (var clubToken in clubs)
            {
                var homeField = clubToken["homeField"].ToString();

                var stadiumTask = _stadiumRepository.SingleOrDefaultAsync(s => s.Name == homeField);
                stadiumTask.Wait();

                var stadium = stadiumTask.Result;
                if (stadium == null)
                    continue;

                var club = JsonConvert.DeserializeObject<Club>(clubToken.ToString(), settings);
                club.Stadium = stadium;

                _clubRepository.Add(club);
            }

            _unitOfWork.CompleteAsync().Wait();
        }

        private void SeedSeasonClubs()
        {
            var seasonTask = _seasonRepository.SingleOrDefaultAsync(s => s.Name == "2018/2019");
            seasonTask.Wait();
            var season = seasonTask.Result;

            var clubsTask = _clubRepository.GetAllAsync();
            clubsTask.Wait();

            foreach (var club in clubsTask.Result)
            {
                season.SeasonClubs.Add(new SeasonClub
                {
                    Season = season,
                    Club = club
                });
            }

            _unitOfWork.CompleteAsync().Wait();
        }

        private void SeedSquads()
        {
            var seasonTask = _seasonRepository.SingleOrDefaultAsync(s => s.Name == "2018/2019");
            seasonTask.Wait();
            var season = seasonTask.Result;

            var clubsTask = _clubRepository.GetAllAsync();
            clubsTask.Wait();

            foreach (var club in clubsTask.Result)
            {
                var squad = new Squad
                {
                    Season = season,
                    Club = club
                };

                _squadRepository.Add(squad);
            }

            _unitOfWork.CompleteAsync().Wait();
        }

        private void SeedKits()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var seasonTask = _seasonRepository.SingleOrDefaultAsync(s => s.Name == "2018/2019");
            seasonTask.Wait();
            var season = seasonTask.Result;

            var kitsData = File.ReadAllText("Persistence/Data/Kits.json");
            var kits = JsonConvert.DeserializeObject<JArray>(kitsData);

            foreach (var kitToken in kits)
            {
                var clubName = kitToken["clubName"].ToString();

                var clubTask = _clubRepository.SingleOrDefaultAsync(c => c.Name == clubName);
                clubTask.Wait();

                var club = clubTask.Result;
                if (club == null)
                    continue;

                var squadTask = _squadRepository
                    .SingleOrDefaultAsync(s => s.SeasonId == season.Id && s.ClubId == club.Id);
                squadTask.Wait();

                var squad = squadTask.Result;
                if (squad == null)
                    continue;

                var kit = JsonConvert.DeserializeObject<Kit>(kitToken.ToString(), settings);
                kit.Squad = squad;

                _kitRepository.Add(kit);
            }

            _unitOfWork.CompleteAsync().Wait();
        }

        private void SeedManagers()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var managersData = File.ReadAllText("Persistence/Data/Managers.json");
            var managers = JsonConvert.DeserializeObject<JArray>(managersData);

            var seasonTask = _seasonRepository.SingleOrDefaultAsync(s => s.Name == "2018/2019");
            seasonTask.Wait();
            var season = seasonTask.Result;

            foreach (var managerToken in managers)
            {
                var clubName = managerToken["clubName"].ToString();
                var birthdateString = managerToken["birthdateString"].ToString();

                var clubsTask = _clubRepository.SingleOrDefaultAsync(c => c.Name == clubName);
                clubsTask.Wait();

                var club = clubsTask.Result;
                if (club == null)
                    continue;

                var manager = JsonConvert.DeserializeObject<Manager>(managerToken.ToString(), settings);
                manager.Birthdate = DateTime.ParseExact(birthdateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                _managerRepository.Add(manager);

                var squadTask = _squadRepository
                    .SingleOrDefaultAsync(s => s.SeasonId == season.Id && s.ClubId == club.Id);
                squadTask.Wait();

                var squad = squadTask.Result;
                if (squad == null)
                    continue;

                manager.SquadManagers.Add(new SquadManager
                {
                    Squad = squad,
                    Manager = manager,
                    StartDate = DateTime.Now
                });
            }

            _unitOfWork.CompleteAsync().Wait();
        }

        private void SeedPlayers()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var playersData = File.ReadAllText("Persistence/Data/Players.json");
            var players = JsonConvert.DeserializeObject<JArray>(playersData);

            var seasonTask = _seasonRepository.SingleOrDefaultAsync(s => s.Name == "2018/2019");
            seasonTask.Wait();
            var season = seasonTask.Result;

            foreach (var playerToken in players)
            {
                var clubName = playerToken["clubName"].ToString();
                var position = playerToken["position"].ToString();
                var number = (int) playerToken["number"];
                var birthdate = (long) playerToken["birthdateTimestamp"];

                var positionType = PositionType.Goalkeeper;
                switch (position)
                {
                    case "Goalkeeper":
                        positionType = PositionType.Goalkeeper;
                        break;
                    case "Defender":
                        positionType = PositionType.Defender;
                        break;
                    case "Midfielder":
                        positionType = PositionType.Midfielder;
                        break;
                    case "Forward":
                        positionType = PositionType.Forward;
                        break;
                }

                var clubsTask = _clubRepository.SingleOrDefaultAsync(c => c.Name == clubName);
                clubsTask.Wait();

                var club = clubsTask.Result;
                if (club == null)
                    continue;

                var player = JsonConvert.DeserializeObject<Player>(playerToken.ToString(), settings);
                player.Birthdate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(birthdate);
                player.PositionType = positionType;

                _playerRepository.Add(player);

                var squadTask = _squadRepository
                    .SingleOrDefaultAsync(s => s.SeasonId == season.Id && s.ClubId == club.Id);
                squadTask.Wait();

                var squad = squadTask.Result;
                if (squad == null)
                    continue;

                player.SquadPlayers.Add(new SquadPlayer
                {
                    Squad = squad,
                    Player = player,
                    Number = number,
                    StartDate = DateTime.Now
                });
            }

            _unitOfWork.CompleteAsync().Wait();
        }
    }
}