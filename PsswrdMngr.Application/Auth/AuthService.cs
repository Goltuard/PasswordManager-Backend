using System.Security.Cryptography;
using PsswrdMngr.Domain;

namespace PsswrdMngr.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Name is required", nameof(request.Name));

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
            throw new ArgumentException("Password must have at least 8 characters", nameof(request.Password));

        var normalizedName = request.Name.Trim();

        if (await _userRepository.NameExistsAsync(normalizedName, cancellationToken))
            throw new InvalidOperationException("User with this name already exists");

        var passwordHash = HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = normalizedName,
            PasswordHash = passwordHash,
            PublicKey = string.Empty
        };

        var created = await _userRepository.AddAsync(user, cancellationToken);

        return new RegisterResponse
        {
            Id = created.Id,
            Name = created.Name
        };
    }

    private string HashPassword(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[16];
        rng.GetBytes(salt);

        const int iterations = 100_000;
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(32);

        return $"{iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }
}
