namespace LongevityDiet.Identity.Application.Common;

public static class IdentityRoleNames
{
    public const string User = "User";

    public const string Admin = "Admin";

    public static string Normalize(string roleName)
    {
        return roleName.Trim().ToUpperInvariant();
    }
}
