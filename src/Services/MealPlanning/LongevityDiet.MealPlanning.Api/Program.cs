using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.MealPlanning.Application;
using LongevityDiet.MealPlanning.Infrastructure;
using LongevityDiet.MealPlanning.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMealPlanningApplication();
builder.Services.AddMealPlanningInfrastructure(builder.Configuration);
builder.Services.AddLongevityPublicApiDefaults("Longevity Diet Meal Planning Service", builder.Configuration);

var app = builder.Build();

await app.Services.InitializeMealPlanningAsync();

app.UseLongevityPublicApiDefaults();

app.Run();
