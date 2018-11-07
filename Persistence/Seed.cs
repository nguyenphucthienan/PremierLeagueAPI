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
        private readonly IClubRepository _clubRepository;
        private readonly IPlayerRepository _playerRepository;

        public Seed(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork,
            IClubRepository clubRepository,
            IPlayerRepository playerRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _clubRepository = clubRepository;
            _playerRepository = playerRepository;
        }

        public void SeedData()
        {
            SeedRolesAndAdminUser();
            SeedClubs();
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
                LastActive =  DateTime.Now
            };

            var result = _userManager.CreateAsync(adminUser, "password").Result;

            if (!result.Succeeded)
                return;

            var admin = _userManager.FindByNameAsync("admin").Result;
            _userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"}).Wait();
        }

        private void SeedClubs()
        {
            var clubsData = File.ReadAllText("Persistence/Data/Clubs.json");
            var clubs = JsonConvert.DeserializeObject<List<Club>>(clubsData);

            _clubRepository.AddRange(clubs);
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
                var club = _clubRepository.SingleOrDefaultAsync(c => c.Name == clubName);

                if (club == null)
                    continue;

                var player = JsonConvert.DeserializeObject<Player>(playerToken.ToString(), settings);
                player.ClubId = club.Id;
                player.Birthdate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                    .AddSeconds(birthdate); ;

                _playerRepository.Add(player);
            }

            _unitOfWork.CompleteAsync().Wait();
        }
    }
}