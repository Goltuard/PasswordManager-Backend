using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Domain;

namespace PsswrdMngr.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<CredentialContainer> CredentialContainers { get; set; }
    }
}
