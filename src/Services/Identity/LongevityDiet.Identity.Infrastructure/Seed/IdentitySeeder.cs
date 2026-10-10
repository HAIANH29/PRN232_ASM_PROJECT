using LongevityDiet.Identity.Application.Abstractions;
using LongevityDiet.Identity.Application.Common;
using LongevityDiet.Identity.Domain.Entities;
using LongevityDiet.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LongevityDiet.Identity.Infrastructure.Seed;

public sealed class IdentitySeeder(
    IdentityDbContext dbContext,
    IPasswordHasher passwordHasher,
    IOptions<IdentitySeedOptions> options) : IIdentitySeeder
{
    private readonly IdentitySeedOptions seedOptions = options.Value;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);

        var userRole = await EnsureRoleAsync(IdentityRoleNames.User, cancellationToken);
        var adminRole = await EnsureRoleAsync(IdentityRoleNames.Admin, cancellationToken);

        await EnsureUserAsync(
            seedOptions.AdminEmail,
            seedOptions.AdminPassword,
            seedOptions.AdminDisplayName,
            [userRole, adminRole],
            cancellationToken);

        await EnsureUserAsync(
            seedOptions.UserEmail,
            seedOptions.UserPassword,
            seedOptions.UserDisplayName,
            [userRole],
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Role> EnsureRoleAsync(string roleName, CancellationToken cancellationToken)
    {
        var normalizedName = IdentityRoleNames.Normalize(roleName);
        var role = await dbContext.Roles
            .FirstOrDefaultAsync(existing => existing.NormalizedName == normalizedName, cancellationToken);

        if (role is not null)
        {
            return role;
        }

        role = new Role
        {
            Id = Guid.NewGuid(),
            Name = roleName,
            NormalizedName = normalizedName
        };

        await dbContext.Roles.AddAsync(role, cancellationToken);
        return role;
    }

    private async Task EnsureUserAsync(
        string email,
        string password,
        string displayName,
        IReadOnlyCollection<Role> roles,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = Normalize(email);
        var user = await dbContext.Users
            .Include(existing => existing.Roles)
            .FirstOrDefaultAsync(existing => existing.NormalizedEmail == normalizedEmail, cancellationToken);

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = email.Trim(),
                NormalizedEmail = normalizedEmail,
                DisplayName = displayName.Trim(),
                PasswordHash = passwordHasher.Hash(password),
                IsActive = true,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            await dbContext.Users.AddAsync(user, cancellationToken);
        }
        else
        {
            user.DisplayName = displayName.Trim();
            user.IsActive = true;
        }

        foreach (var role in roles)
        {
            EnsureUserHasRole(user, role);
        }
    }

    private static void EnsureUserHasRole(User user, Role role)
    {
        if (user.Roles.All(existing => existing.NormalizedName != role.NormalizedName))
        {
            user.Roles.Add(role);
        }
    }

    private static string Normalize(string value)
    {
        return value.Trim().ToUpperInvariant();
    }
}
