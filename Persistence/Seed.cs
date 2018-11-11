using System;
using System.Collections.Generic;
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
        private readonly IClubRepository _clubRepository;
        private readonly ISquadRepository _squadRepository;
        private readonly IPlayerRepository _playerRepository;

        public Seed(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork,
            ISeasonRepository seasonRepository,
            IClubRepository clubRepository,
            ISquadRepository squadRepository,
            IPlayerRepository playerRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _seasonRepository = seasonRepository;
            _clubRepository = clubRepository;
            _squadRepository = squadRepository;
            _playerRepository = playerRepository;
        }

        public void SeedData()
        {
            SeedRolesAndAdminUser();
            SeedSeasons();
            SeedClubs();
            SeedSeasonClubs();
            SeedSquads();
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

        private void SeedClubs()
        {
            var clubsData = File.ReadAllText("Persistence/Data/Clubs.json");
            var clubs = JsonConvert.DeserializeObject<List<Club>>(clubsData);

            _clubRepository.AddRange(clubs);
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

        private void SeedPlayers()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var playersData = File.ReadAllText("Persistence/Data/Players.json");
            var players = JsonConvert.DeserializeObject<JArray>(playersData);

            foreach (var playerToken in players)
            {
                var clubName = playerToken["clubName"].ToString();
                var birthdate = (long) playerToken["birthdateTimestamp"];

                var clubsTask = _clubRepository.SingleOrDefaultAsync(c => c.Name == clubName);
                clubsTask.Wait();

                var club = clubsTask.Result;
                if (club == null)
                    continue;

                var player = JsonConvert.DeserializeObject<Player>(playerToken.ToString(), settings);
                player.Birthdate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(birthdate);

                _playerRepository.Add(player);

                var squadTask = _squadRepository.SingleOrDefaultAsync(s => s.ClubId == club.Id);
                squadTask.Wait();

                var squad = squadTask.Result;
                if (squad == null)
                    continue;

                player.SquadPlayers.Add(new SquadPlayer
                {
                    Squad = squad,
                    Player = player
                });
            }

            _unitOfWork.CompleteAsync().Wait();
        }
    }
}