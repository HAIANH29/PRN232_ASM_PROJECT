using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.MealPlanning.Application;
using LongevityDiet.MealPlanning.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMealPlanningApplication();
builder.Services.AddMealPlanningInfrastructure(builder.Configuration);
builder.Services.AddLongevityPublicApiDefaults("Longevity Diet Meal Planning Service", builder.Configuration);

var app = builder.Build();

app.UseLongevityPublicApiDefaults();

app.Run();
