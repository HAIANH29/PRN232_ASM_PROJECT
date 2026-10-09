# Longevity Diet Platform

PRN232 microservices scaffold for an educational diet planning and tracking platform based on approved knowledge from *The Longevity Diet*.

This repository has the Phase 1 microservice scaffold plus the Milestone 1 Identity, Milestone 2 Diet Knowledge, Milestone 3 Meal Planning, Milestone 4 Tracking/gRPC Notification, Milestone 5 RabbitMQ notification delivery, Milestone 6 Recommendation Service, and Milestone 7 API Gateway implementations. The remaining web UI, tests, and final demo package are still being implemented incrementally.

Project status and remaining work are tracked in `PROJECT_PROGRESS.md`. The completion roadmap is tracked in `PROJECT_SCHEDULE.md`. Update the progress file after each completed project change.

Development conventions:

- API conventions: `docs/development/Api-Conventions.md`
- API Gateway routes: `docs/development/Gateway-Routes.md`
- Seed data strategy: `docs/development/Seed-Data-Strategy.md`
- Book knowledge workflow: `docs/book-knowledge/Book-Knowledge-Integration.md`

## Containers

- `LongevityDiet.Web` - ASP.NET Core MVC web application.
- `LongevityDiet.ApiGateway` - YARP reverse proxy gateway.
- `LongevityDiet.Identity.Api` - REST API for users, roles, authentication, and JWT ownership.
- `LongevityDiet.DietKnowledge.Api` - REST API for guidelines, foods, recipes, and ingredients.
- `LongevityDiet.MealPlanning.Api` - REST API for meal plans, reminder publishing, and AI recommendation request publishing.
- `LongevityDiet.Tracking.Api` - REST API for daily and meal tracking.
- `LongevityDiet.Recommendation.Service` - internal RabbitMQ recommendation worker with approved-knowledge context, safe fallback behavior, and optional Google Gemini integration.
- `LongevityDiet.Notification.Grpc` - internal gRPC notification service called by Tracking.
- `LongevityDiet.NotificationWorker` - worker service for RabbitMQ notification/reminder messages and Resend/logged email delivery.

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
- API Gateway route summary: `http://localhost:5001/routes`
- Identity Swagger: `http://localhost:5101/swagger`
- Diet Knowledge Swagger: `http://localhost:5102/swagger`
- Meal Planning Swagger: `http://localhost:5103/swagger`
- Tracking Swagger: `http://localhost:5104/swagger`
- Notification Service health: `http://localhost:5105/health`
- RabbitMQ Management: `http://localhost:15672`

API Gateway route prefixes:

- Identity: `http://localhost:5001/identity/api/auth/login`
- Diet Knowledge: `http://localhost:5001/diet-knowledge/api/foods`
- Meal Planning: `http://localhost:5001/meal-planning/api/meal-plans`
- Tracking: `http://localhost:5001/tracking/api/daily-trackings`

Send the same `Authorization: Bearer <token>` header through the gateway for protected endpoints. Recommendation Service, Notification Service, Notification Worker, RabbitMQ, and databases are intentionally not exposed through gateway routes.

Local demo Identity credentials are configured through `.env`:

- Admin email: `IDENTITY_ADMIN_EMAIL` defaults to `admin@longevity.local`
- Admin password: `IDENTITY_ADMIN_PASSWORD` defaults to `Admin@123456`

Diet Knowledge endpoints:

- Public reads:
  - `GET /api/diet-guidelines`
  - `GET /api/foods`
  - `GET /api/recipes`
- Public reads return active `Approved` knowledge only.
- Admin mutations:
  - `POST /api/admin/diet-guidelines`, `PUT|DELETE /api/admin/diet-guidelines/{id}`
  - `POST /api/admin/foods`, `PUT|DELETE /api/admin/foods/{id}`
  - `POST /api/admin/recipes`, `PUT|DELETE /api/admin/recipes/{id}`
  - `PATCH /api/admin/{diet-guidelines|foods|recipes}/{id}/activation`
- Admin list endpoints can filter by `reviewStatus=NeedsReview|Approved|Rejected`.
- Seeded book-knowledge items are `NeedsReview` until the team fills source metadata and approves them.

Meal Planning endpoints:

- All Meal Planning endpoints require a JWT Bearer token.
- Meal plan workflow:
  - `GET /api/meal-plans`
  - `GET /api/meal-plans/{id}`
  - `POST /api/meal-plans`
  - `PUT /api/meal-plans/{id}`
  - `DELETE /api/meal-plans/{id}`
  - `POST /api/meal-plans/{mealPlanId}/items`
  - `PUT /api/meal-plans/{mealPlanId}/items/{itemId}`
  - `DELETE /api/meal-plans/{mealPlanId}/items/{itemId}`
- Recommendation review workflow:
  - `POST /api/meal-recommendations`
  - `GET /api/meal-recommendations`
  - `GET /api/meal-recommendations/{id}`
  - `POST /api/meal-recommendations/{id}/accept`
- Meal Planning validates `FoodId`/`RecipeId` through Diet Knowledge public APIs, so only active `Approved` content can be scheduled.
- Scheduled meal items publish reminder messages to RabbitMQ.
- Recommendation requests are published to RabbitMQ; completed recommendation results are consumed and stored for user review.
- A separate `MealSchedule` table is not used in this milestone; schedule data is represented by `MealPlanItem.PlannedDate`, `MealSlot`, and `ReminderAtUtc`.

Tracking endpoints:

- All Tracking endpoints require a JWT Bearer token.
- Daily tracking workflow:
  - `GET /api/daily-trackings`
  - `GET /api/daily-trackings/{trackingDate}`
  - `PUT /api/daily-trackings/{trackingDate}`
  - `DELETE /api/daily-trackings/{trackingDate}`
  - `PUT /api/daily-trackings/{trackingDate}/meals/{mealPlanItemId}`
- Progress summary workflow:
  - `GET /api/progress-summaries?periodStartDate=YYYY-MM-DD&periodEndDate=YYYY-MM-DD`
- Tracking stores only `MealPlanItemId` as a cross-service ID; it does not read `MealPlanningDb`.
- When a daily progress summary reaches all tracked meals completed, Tracking calls Notification Service through gRPC.
- Notification Service publishes a `NotificationRequested` message to RabbitMQ for later worker delivery.

Notification Worker:

- Consumes scheduled meal reminders from `reminder.requests`.
- Consumes progress notifications from `notification.messages`.
- Declares durable RabbitMQ exchanges/queues plus dead-letter exchanges/queues for both flows.
- Retries failed messages using `RabbitMq__MaxDeliveryAttempts` and `RabbitMq__RetryDelaySeconds`.
- Sends through Resend when `RESEND_API_KEY` is configured.
- Logs the expected email payload when `RESEND_API_KEY` is empty or `development-only`, which is the default local fallback.

Recommendation Service:

- Consumes `RecommendationRequest` messages from `recommendation.requests`.
- Calls Diet Knowledge public APIs to build context only from active `Approved` knowledge.
- Filters unsafe user preference tags before they are used in recommendation titles.
- Uses local safe fallback recommendations when `GEMINI_ENABLED=false` or `GEMINI_API_KEY` is empty/`development-only`.
- Can call Gemini through a configurable HTTP client when `GEMINI_ENABLED=true`, `GEMINI_API_KEY` is configured, and `GEMINI_MODEL` is set.
- Publishes `RecommendationResult` messages to `recommendation.results`, where Meal Planning consumes and stores them for user review.
- Keeps the required safety disclaimer and avoids diagnosis, treatment advice, disease prediction, and lifespan prediction.

## Remaining TODO

- Build the real Web Application screens.
- Add tests around each implemented slice.
