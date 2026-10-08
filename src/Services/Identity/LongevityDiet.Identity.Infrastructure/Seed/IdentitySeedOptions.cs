namespace LongevityDiet.Identity.Infrastructure.Seed;

public sealed class IdentitySeedOptions
{
    public string AdminEmail { get; set; } = "admin@longevity.local";

    public string AdminPassword { get; set; } = "Admin@123456";

    public string AdminDisplayName { get; set; } = "Demo Admin";
}
