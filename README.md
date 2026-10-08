# Longevity Diet Platform

PRN232 microservices scaffold for an educational diet planning and tracking platform based on approved knowledge from *The Longevity Diet*.

This repository has the Phase 1 microservice scaffold plus the Milestone 1 Identity implementation and Milestone 2 Diet Knowledge implementation. The remaining business services are still being implemented incrementally.

Project status and remaining work are tracked in `PROJECT_PROGRESS.md`. The completion roadmap is tracked in `PROJECT_SCHEDULE.md`. Update the progress file after each completed project change.

Development conventions:

- API conventions: `docs/development/Api-Conventions.md`
- Seed data strategy: `docs/development/Seed-Data-Strategy.md`

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

Local demo Identity credentials are configured through `.env`:

- Admin email: `IDENTITY_ADMIN_EMAIL` defaults to `admin@longevity.local`
- Admin password: `IDENTITY_ADMIN_PASSWORD` defaults to `Admin@123456`

Diet Knowledge endpoints:

- Public reads:
  - `GET /api/diet-guidelines`
  - `GET /api/foods`
  - `GET /api/recipes`
- Admin mutations:
  - `POST /api/admin/diet-guidelines`, `PUT|DELETE /api/admin/diet-guidelines/{id}`
  - `POST /api/admin/foods`, `PUT|DELETE /api/admin/foods/{id}`
  - `POST /api/admin/recipes`, `PUT|DELETE /api/admin/recipes/{id}`
  - `PATCH /api/admin/{diet-guidelines|foods|recipes}/{id}/activation`

## Remaining TODO

- Add EF Core migrations for Meal Planning and Tracking.
- Implement meal plan workflows and real RabbitMQ publish/consume logic.
- Implement Recommendation Service RabbitMQ consumption and Google Gemini integration.
- Implement Tracking -> Notification gRPC use cases.
- Implement Notification Worker consumption and Resend integration.
- Implement tracking workflows and progress summaries.
- Add tests around each implemented slice.
