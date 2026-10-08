using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.Tracking.Application;
using LongevityDiet.Tracking.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTrackingApplication();
builder.Services.AddTrackingInfrastructure(builder.Configuration);
builder.Services.AddLongevityPublicApiDefaults("Longevity Diet Tracking Service", builder.Configuration);

var app = builder.Build();

app.UseLongevityPublicApiDefaults();

app.Run();
