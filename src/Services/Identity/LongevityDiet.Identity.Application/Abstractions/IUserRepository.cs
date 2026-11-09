using LongevityDiet.Identity.Domain.Entities;

namespace LongevityDiet.Identity.Application.Abstractions;

public interface IUserRepository
{
    IQueryable<User> Query();

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

    Task<Role?> GetRoleByNameAsync(string normalizedName, CancellationToken cancellationToken = default);

    Task AddAsync(User user, CancellationToken cancellationToken = default);

    // Marks a new preference profile as added; assigning it to User.Profile alone makes EF treat it as an update.
    void AddProfile(UserPreferenceProfile profile);

    Task<bool> EmailExistsAsync(string normalizedEmail, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
