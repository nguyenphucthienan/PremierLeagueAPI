using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PremierLeagueAPI.Dtos;
using PremierLeagueAPI.Models;

namespace PremierLeagueAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration configuration,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var registerUser = _mapper.Map<User>(registerUserDto);
            var result = await _userManager.CreateAsync(registerUser, registerUserDto.Password);
            var returnUser = _mapper.Map<DetailUserDto>(registerUser);

            if (result.Succeeded)
            {
                // return CreatedAtRoute();
                return Ok(returnUser);
            }

            return BadRequest(result.Errors);
        }
    }
}