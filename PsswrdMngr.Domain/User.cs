using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace PsswrdMngr.Domain
{
    public class User : IdentityUser
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string PublicKey { get; set; }
    }
}
