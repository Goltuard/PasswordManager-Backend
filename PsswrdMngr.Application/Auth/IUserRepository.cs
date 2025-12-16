using System.Threading;
using System.Threading.Tasks;
using PsswrdMngr.Domain;

namespace PsswrdMngr.Application.Auth
{
    public interface IUserRepository
    {
        Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
        Task<User> AddAsync(User user, CancellationToken cancellationToken = default);
    }
}
