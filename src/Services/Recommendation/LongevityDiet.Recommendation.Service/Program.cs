using LongevityDiet.Recommendation.Service;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<RecommendationMessageWorker>();

var host = builder.Build();
host.Run();
