using Microsoft.Extensions.DependencyInjection;
using LongevityDiet.NotificationWorker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<ResendOptions>(
    builder.Configuration.GetSection(ResendOptions.SectionName));
builder.Services.AddHttpClient<IEmailSender, ResendEmailSender>();
builder.Services.AddHostedService<NotificationMessageWorker>();

var host = builder.Build();
host.Run();
