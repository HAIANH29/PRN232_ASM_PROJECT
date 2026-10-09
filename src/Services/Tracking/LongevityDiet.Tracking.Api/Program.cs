using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.Tracking.Application;
using LongevityDiet.Tracking.Infrastructure;
using LongevityDiet.Tracking.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTrackingApplication();
builder.Services.AddTrackingInfrastructure(builder.Configuration);
builder.Services.AddLongevityPublicApiDefaults("Longevity Diet Tracking Service", builder.Configuration);

var app = builder.Build();

await app.Services.InitializeTrackingAsync();

app.UseLongevityPublicApiDefaults();

app.Run();
