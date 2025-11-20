using System;
using System.Collections.Generic;
using System.Text;

namespace PsswrdMngr.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string PublicKey { get; set; }

    }
}
