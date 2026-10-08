using LongevityDiet.Identity.Domain.Entities;

namespace LongevityDiet.Identity.Application.Abstractions;

public interface IUserRepository
{
    IQueryable<User> Query();

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

    Task<Role?> GetRoleByNameAsync(string normalizedName, CancellationToken cancellationToken = default);

    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
