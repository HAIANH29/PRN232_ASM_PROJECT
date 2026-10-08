namespace LongevityDiet.Identity.Application.Services;

public sealed class IdentityService : IIdentityService
{
    public IdentityServiceStatus GetStatus()
    {
        return new IdentityServiceStatus(
            "Identity Service",
            "IdentityDb",
            ["User", "Role", "Authentication", "JWT token generation", "Basic user profile"]);
    }
}
