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

        var normalizedAdminEmail = Normalize(seedOptions.AdminEmail);
        var admin = await dbContext.Users
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.NormalizedEmail == normalizedAdminEmail, cancellationToken);

        if (admin is null)
        {
            admin = new User
            {
                Id = Guid.NewGuid(),
                Email = seedOptions.AdminEmail.Trim(),
                NormalizedEmail = normalizedAdminEmail,
                DisplayName = seedOptions.AdminDisplayName.Trim(),
                PasswordHash = passwordHasher.Hash(seedOptions.AdminPassword),
                IsActive = true,
                CreatedAtUtc = DateTimeOffset.UtcNow
            };

            admin.Roles.Add(userRole);
            admin.Roles.Add(adminRole);
            await dbContext.Users.AddAsync(admin, cancellationToken);
        }
        else
        {
            EnsureUserHasRole(admin, userRole);
            EnsureUserHasRole(admin, adminRole);
        }

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
