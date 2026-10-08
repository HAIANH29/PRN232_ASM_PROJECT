# Longevity Diet Platform

PRN232 microservices scaffold for an educational diet planning and tracking platform based on approved knowledge from *The Longevity Diet*.

This repository is Phase 1 only. It creates the structure and infrastructure hooks, not full business CRUD.

## Containers

- `LongevityDiet.Web` - ASP.NET Core MVC web application.
- `LongevityDiet.ApiGateway` - YARP reverse proxy gateway.
- `LongevityDiet.Identity.Api` - REST API for users, roles, authentication, and JWT ownership.
- `LongevityDiet.DietKnowledge.Api` - REST API for guidelines, foods, recipes, and ingredients.
- `LongevityDiet.MealPlanning.Api` - REST API for meal plans and reminder publishing.
- `LongevityDiet.Tracking.Api` - REST API for daily and meal tracking.
- `LongevityDiet.Recommendation.Grpc` - internal gRPC recommendation skeleton.
- `LongevityDiet.ReminderWorker` - worker skeleton for RabbitMQ reminder messages and future email provider calls.

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
- RabbitMQ Management: `http://localhost:15672`

## Phase 1 TODO

- Add EF Core migrations per service.
- Implement real authentication and JWT token issuance in Identity Service.
- Implement CRUD and query endpoints for approved diet knowledge.
- Implement meal plan workflows and real RabbitMQ publish logic.
- Implement reminder consumption and email provider integration.
- Implement tracking workflows and progress summaries.
- Add tests around each implemented slice.
