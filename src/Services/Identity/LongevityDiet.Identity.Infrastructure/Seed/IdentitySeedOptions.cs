namespace LongevityDiet.Identity.Infrastructure.Seed;

public sealed class IdentitySeedOptions
{
    public string AdminEmail { get; set; } = "admin@longevity.local";

    public string AdminPassword { get; set; } = "Admin@123456";

    public string AdminDisplayName { get; set; } = "Demo Admin";

    public string UserEmail { get; set; } = "user@longevity.local";

    public string UserPassword { get; set; } = "User@123456";

    public string UserDisplayName { get; set; } = "Demo User";
}
