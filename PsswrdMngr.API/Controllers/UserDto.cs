using System;

namespace PsswrdMngr.API.Controllers
{
    // Prosty DTO pasujący do encji User
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PublicKey { get; set; } = string.Empty;
    }
}
