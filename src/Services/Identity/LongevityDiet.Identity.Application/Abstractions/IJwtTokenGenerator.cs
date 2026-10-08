using LongevityDiet.Identity.Domain.Entities;

namespace LongevityDiet.Identity.Application.Abstractions;

public interface IJwtTokenGenerator
{
    TokenResult Generate(User user);
}
