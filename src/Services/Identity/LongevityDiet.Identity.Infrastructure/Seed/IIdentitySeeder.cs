namespace LongevityDiet.Identity.Infrastructure.Seed;

public interface IIdentitySeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
