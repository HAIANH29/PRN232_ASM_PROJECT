using LongevityDiet.Identity.Domain.Entities;

namespace LongevityDiet.Identity.Application.Abstractions;

public interface IUserRepository
{
    IQueryable<User> Query();
}
