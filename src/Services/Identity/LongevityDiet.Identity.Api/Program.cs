using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.Identity.Application;
using LongevityDiet.Identity.Application.Common;
using LongevityDiet.Identity.Infrastructure;
using LongevityDiet.Identity.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddLongevityPublicApiDefaults("Longevity Diet Identity Service", builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(IdentityRoleNames.Admin));
});

var app = builder.Build();

await app.Services.SeedIdentityAsync();

app.UseLongevityPublicApiDefaults();

app.Run();
