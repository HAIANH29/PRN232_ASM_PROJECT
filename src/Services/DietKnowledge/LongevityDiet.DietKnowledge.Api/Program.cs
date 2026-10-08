using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.DietKnowledge.Application;
using LongevityDiet.DietKnowledge.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDietKnowledgeApplication();
builder.Services.AddDietKnowledgeInfrastructure(builder.Configuration);
builder.Services.AddLongevityPublicApiDefaults("Longevity Diet Knowledge Service");

var app = builder.Build();

app.UseLongevityPublicApiDefaults();

app.Run();
