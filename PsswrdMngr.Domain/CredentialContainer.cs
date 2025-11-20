using System;
using System.Collections.Generic;
using System.Text;

namespace PsswrdMngr.Domain
{
    public class CredentialContainer
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ContainerHash { get; set; }
        public string ContainerString { get; set; }
    }
}
