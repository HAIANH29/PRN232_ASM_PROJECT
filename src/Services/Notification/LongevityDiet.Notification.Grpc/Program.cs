using LongevityDiet.Notification.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<INotificationPublisher, RabbitMqNotificationPublisher>();
builder.Services.AddGrpc();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGrpcService<NotificationGrpcService>();
app.MapHealthChecks("/health");
app.MapGet("/", () => "Notification Service uses gRPC. Tracking Service calls RequestProgressNotification.");

app.Run();
