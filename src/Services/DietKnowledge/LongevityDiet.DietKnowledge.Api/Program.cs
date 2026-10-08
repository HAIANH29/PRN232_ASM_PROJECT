using LongevityDiet.ApiDefaults.Extensions;
using LongevityDiet.DietKnowledge.Application;
using LongevityDiet.DietKnowledge.Infrastructure;
using LongevityDiet.DietKnowledge.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDietKnowledgeApplication();
builder.Services.AddDietKnowledgeInfrastructure(builder.Configuration);
builder.Services.AddLongevityPublicApiDefaults("Longevity Diet Knowledge Service", builder.Configuration);

var app = builder.Build();

await app.Services.SeedDietKnowledgeAsync();

app.UseLongevityPublicApiDefaults();

app.Run();
