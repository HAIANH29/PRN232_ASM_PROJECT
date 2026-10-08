using LongevityDiet.NotificationWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<NotificationMessageWorker>();

var host = builder.Build();
host.Run();
