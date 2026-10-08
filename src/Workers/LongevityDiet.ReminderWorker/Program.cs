using LongevityDiet.ReminderWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ReminderMessageWorker>();

var host = builder.Build();
host.Run();
