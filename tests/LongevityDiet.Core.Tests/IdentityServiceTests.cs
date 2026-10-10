using System.Security.Authentication;
using LongevityDiet.Identity.Application.Models;
using LongevityDiet.Identity.Application.Services;
using Xunit;

namespace LongevityDiet.Core.Tests;

public sealed class IdentityServiceTests
{
    [Fact]
    public async Task RegisterAsync_creates_user_with_normalized_email_user_role_and_token()
    {
        var repository = new InMemoryUserRepository();
        var service = new IdentityService(
            repository,
            new FakePasswordHasher(),
            new FakeJwtTokenGenerator());

        var result = await service.RegisterAsync(new RegisterUserCommand(
            "  new.user@example.com  ",
            "Pass@123456",
            "  New User  "));

        Assert.Single(repository.Users);
        var created = repository.Users.Single();
        Assert.Equal("new.user@example.com", created.Email);
        Assert.Equal("NEW.USER@EXAMPLE.COM", created.NormalizedEmail);
        Assert.Equal("New User", created.DisplayName);
        Assert.Equal("hashed:Pass@123456", created.PasswordHash);
        Assert.Contains(created.Roles, role => role.Name == "User");
        Assert.StartsWith("token:", result.AccessToken, StringComparison.Ordinal);
        Assert.Equal(created.Id, result.Profile.Id);
        Assert.Equal(1, repository.SaveChangesCount);
    }

    [Fact]
    public async Task LoginAsync_rejects_wrong_password()
    {
        var repository = new InMemoryUserRepository();
        repository.Users.Add(new()
        {
            Id = Guid.NewGuid(),
            Email = "demo@example.com",
            NormalizedEmail = "DEMO@EXAMPLE.COM",
            DisplayName = "Demo User",
            PasswordHash = "hashed:correct-password",
            IsActive = true
        });

        var service = new IdentityService(
            repository,
            new FakePasswordHasher(),
            new FakeJwtTokenGenerator());

        await Assert.ThrowsAsync<AuthenticationException>(() =>
            service.LoginAsync(new LoginCommand("demo@example.com", "wrong-password")));
    }
}
