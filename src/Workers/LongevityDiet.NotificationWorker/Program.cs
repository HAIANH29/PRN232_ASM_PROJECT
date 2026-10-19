using Microsoft.Extensions.DependencyInjection;
using LongevityDiet.NotificationWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<BrevoOptions>(
    builder.Configuration.GetSection(BrevoOptions.SectionName));
builder.Services.AddHttpClient<IEmailSender, BrevoEmailSender>();
builder.Services.AddHostedService<NotificationMessageWorker>();

var host = builder.Build();
host.Run();
