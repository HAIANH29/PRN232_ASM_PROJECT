using LongevityDiet.Identity.Application.Models;

namespace LongevityDiet.Identity.Application.Services;

public interface IIdentityService
{
    IdentityServiceStatus GetStatus();

    Task<AuthResult> RegisterAsync(RegisterUserCommand command, CancellationToken cancellationToken = default);

    Task<AuthResult> LoginAsync(LoginCommand command, CancellationToken cancellationToken = default);

    Task<UserProfile> GetProfileAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserProfile> UpdatePreferencesAsync(
        Guid userId,
        UpdatePreferencesCommand command,
        CancellationToken cancellationToken = default);
}
