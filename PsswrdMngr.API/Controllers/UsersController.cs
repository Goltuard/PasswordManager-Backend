using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.API.Dto;
using PsswrdMngr.API.Services;
using PsswrdMngr.Domain;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public UsersController(UserManager<User> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (result)
            {
                Console.WriteLine(user.Id);
                return new UserDto
                {
                    Id = user.UserId,
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                return BadRequest("UserName taken.");
            }

            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = registerDto.Password,
                PublicKey = registerDto.PublicKey,
                UserId = Guid.NewGuid()
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    Id = user.UserId,
                    UserName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                };
            }
            else
            {
                foreach (var err in result.Errors)
                {
                    Console.WriteLine(err);
                    Console.WriteLine(err.Description);
                }
            }

            return BadRequest("Problem registering");
        }
    }
}
