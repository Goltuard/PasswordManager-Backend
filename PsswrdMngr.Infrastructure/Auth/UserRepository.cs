using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PsswrdMngr.Application.Auth; 
using PsswrdMngr.Domain;

namespace PsswrdMngr.Infrastructure.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return _context.Users.AnyAsync(u => u.Name == name, cancellationToken);
        }

        public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return user;
        }
    }
}
