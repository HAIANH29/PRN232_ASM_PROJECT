using LongevityDiet.Identity.Application.Abstractions;
using LongevityDiet.Identity.Domain.Entities;
using LongevityDiet.Identity.Infrastructure.Persistence;

namespace LongevityDiet.Identity.Infrastructure.Repositories;

public sealed class UserRepository(IdentityDbContext dbContext) : IUserRepository
{
    public IQueryable<User> Query()
    {
        return dbContext.Users;
    }
}
