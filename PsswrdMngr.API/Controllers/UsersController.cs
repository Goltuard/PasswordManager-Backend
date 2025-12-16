using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Infrastructure;

namespace PsswrdMngr.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // DTO – to, co wystawiasz na zewnątrz (bez PasswordHash)
        public class UserDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = null!;
            public string PublicKey { get; set; } = null!;
        }

        // GET: /api/Users
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            var users = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            var result = users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                PublicKey = u.PublicKey
            }).ToList();

            return Ok(result);
        }

        // GET: /api/Users/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            var dto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                PublicKey = user.PublicKey
            };

            return Ok(dto);
        }
    }
}
