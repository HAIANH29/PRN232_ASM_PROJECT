using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using LongevityDiet.Identity.Application.Abstractions;
using LongevityDiet.Identity.Application.Common;
using LongevityDiet.Identity.Application.Models;
using LongevityDiet.Identity.Domain.Entities;

namespace LongevityDiet.Identity.Application.Services;

public sealed class IdentityService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IIdentityService
{
    public IdentityServiceStatus GetStatus()
    {
        return new IdentityServiceStatus(
            "Identity Service",
            "IdentityDb",
            ["User", "Role", "Authentication", "JWT token generation", "Basic user profile"]);
    }

    public async Task<AuthResult> RegisterAsync(
        RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(command.Email);

        if (await userRepository.EmailExistsAsync(normalizedEmail, cancellationToken))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var userRole = await userRepository.GetRoleByNameAsync(
            IdentityRoleNames.Normalize(IdentityRoleNames.User),
            cancellationToken);

        if (userRole is null)
        {
            throw new InvalidOperationException("The User role has not been seeded.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = command.Email.Trim(),
            NormalizedEmail = normalizedEmail,
            DisplayName = command.DisplayName.Trim(),
            PasswordHash = passwordHasher.Hash(command.Password),
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        user.Roles.Add(userRole);

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return CreateAuthResult(user);
    }

    public async Task<AuthResult> LoginAsync(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = NormalizeEmail(command.Email);
        var user = await userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

        if (user is null || !passwordHasher.Verify(command.Password, user.PasswordHash))
        {
            throw new AuthenticationException("Email or password is incorrect.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("This account is inactive.");
        }

        return CreateAuthResult(user);
    }

    public async Task<UserProfile> GetProfileAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User profile was not found.");
        }

        return MapProfile(user);
    }

    public async Task<UserProfile> UpdatePreferencesAsync(
        Guid userId,
        UpdatePreferencesCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new KeyNotFoundException("User profile was not found.");

        var tags = NormalizeTags(command.PreferenceTags);
        var notes = command.Notes?.Trim() ?? string.Empty;
        if (notes.Length > 1000)
        {
            throw new ValidationException("Notes can be at most 1000 characters.");
        }

        if (user.Profile is null)
        {
            user.Profile = new UserPreferenceProfile { Id = Guid.NewGuid(), UserId = user.Id };
            userRepository.AddProfile(user.Profile);
        }

        user.Profile.PreferenceTags = string.Join(',', tags);
        user.Profile.Notes = notes;
        user.Profile.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await userRepository.SaveChangesAsync(cancellationToken);
        return MapProfile(user);
    }

    private static string[] NormalizeTags(IReadOnlyCollection<string> tags)
    {
        var cleaned = tags
            .Select(tag => tag?.Trim() ?? string.Empty)
            .Where(tag => tag.Length > 0)
            .ToArray();

        if (cleaned.Any(tag => tag.Length > 40 || tag.Contains(',')))
        {
            throw new ValidationException("Each preference tag must be at most 40 characters and cannot contain a comma.");
        }

        var distinct = cleaned.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        if (distinct.Length > 20)
        {
            throw new ValidationException("At most 20 preference tags are allowed.");
        }

        return distinct;
    }

    private AuthResult CreateAuthResult(User user)
    {
        var token = jwtTokenGenerator.Generate(user);

        return new AuthResult(
            token.AccessToken,
            token.ExpiresAtUtc,
            MapProfile(user));
    }

    private static UserProfile MapProfile(User user)
    {
        return new UserProfile(
            user.Id,
            user.Email,
            user.DisplayName,
            user.IsActive,
            user.Roles.Select(role => role.Name).OrderBy(role => role).ToArray(),
            user.CreatedAtUtc,
            SplitTags(user.Profile?.PreferenceTags),
            user.Profile?.Notes ?? string.Empty);
    }

    private static string[] SplitTags(string? tags)
    {
        return string.IsNullOrWhiteSpace(tags)
            ? []
            : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static string NormalizeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ValidationException("Email is required.");
        }

        return email.Trim().ToUpperInvariant();
    }
}
