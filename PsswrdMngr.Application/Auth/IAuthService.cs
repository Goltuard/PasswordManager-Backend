using System.Threading;
using System.Threading.Tasks;

namespace PsswrdMngr.Application.Auth
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    }
}
