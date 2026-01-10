using Microsoft.AspNetCore.Identity;

namespace PsswrdMngr.Domain
{
    public class User : IdentityUser
    {
        public string PublicKey { get; set; }
    }
}
