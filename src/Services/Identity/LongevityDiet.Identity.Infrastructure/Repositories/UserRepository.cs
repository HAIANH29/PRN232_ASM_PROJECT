using LongevityDiet.Identity.Application.Abstractions;
using LongevityDiet.Identity.Domain.Entities;
using LongevityDiet.Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.Identity.Infrastructure.Repositories;

public sealed class UserRepository(IdentityDbContext dbContext) : IUserRepository
{
    public IQueryable<User> Query()
    {
        return dbContext.Users.Include(user => user.Roles);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    public Task<Role?> GetRoleByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
    {
        return dbContext.Roles
            .FirstOrDefaultAsync(role => role.NormalizedName == normalizedName, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AnyAsync(user => user.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
