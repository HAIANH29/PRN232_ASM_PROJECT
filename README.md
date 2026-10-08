# Longevity Diet Platform

PRN232 microservices scaffold for an educational diet planning and tracking platform based on approved knowledge from *The Longevity Diet*.

This repository is Phase 1 only. It creates the structure and infrastructure hooks, not full business CRUD.

Project status and remaining work are tracked in `PROJECT_PROGRESS.md`. Update that file after each completed project change.

## Containers

- `LongevityDiet.Web` - ASP.NET Core MVC web application.
- `LongevityDiet.ApiGateway` - YARP reverse proxy gateway.
- `LongevityDiet.Identity.Api` - REST API for users, roles, authentication, and JWT ownership.
- `LongevityDiet.DietKnowledge.Api` - REST API for guidelines, foods, recipes, and ingredients.
- `LongevityDiet.MealPlanning.Api` - REST API for meal plans, reminder publishing, and AI recommendation request publishing.
- `LongevityDiet.Tracking.Api` - REST API for daily and meal tracking.
- `LongevityDiet.Recommendation.Service` - internal RabbitMQ recommendation worker with Google Gemini placeholder.
- `LongevityDiet.Notification.Grpc` - internal gRPC notification service called by Tracking.
- `LongevityDiet.NotificationWorker` - worker skeleton for RabbitMQ notification/reminder messages and future Resend calls.

Each REST service is split into:

- `.Api`
- `.Application`
- `.Infrastructure`
- `.Domain`

## Run Locally

Restore and build:

```powershell
$env:DOTNET_CLI_HOME = "$PWD\.dotnet"
dotnet restore LongevityDietPlatform.sln
dotnet build LongevityDietPlatform.sln
```

Run all containers:

```powershell
copy .env.example .env
docker compose up --build
```

Useful URLs:

- Web: `http://localhost:5000`
- API Gateway: `http://localhost:5001`
- Identity Swagger: `http://localhost:5101/swagger`
- Diet Knowledge Swagger: `http://localhost:5102/swagger`
- Meal Planning Swagger: `http://localhost:5103/swagger`
- Tracking Swagger: `http://localhost:5104/swagger`
- Notification Service health: `http://localhost:5105/health`
- RabbitMQ Management: `http://localhost:15672`

## Phase 1 TODO

- Add EF Core migrations per service.
- Implement real authentication and JWT token issuance in Identity Service.
- Implement CRUD and query endpoints for approved diet knowledge.
- Implement meal plan workflows and real RabbitMQ publish/consume logic.
- Implement Recommendation Service RabbitMQ consumption and Google Gemini integration.
- Implement Tracking -> Notification gRPC use cases.
- Implement Notification Worker consumption and Resend integration.
- Implement tracking workflows and progress summaries.
- Add tests around each implemented slice.
