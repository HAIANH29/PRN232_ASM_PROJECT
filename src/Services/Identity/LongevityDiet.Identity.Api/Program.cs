using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.Identity.Application;
using LongevityDiet.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddLongevityPublicApiDefaults("Longevity Diet Identity Service");

var app = builder.Build();

app.UseLongevityPublicApiDefaults();

app.Run();
