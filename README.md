# Longevity Diet Platform

PRN232 microservices application for an educational diet planning and tracking platform based on approved knowledge from *The Longevity Diet*.

This repository contains the implemented PRN232 microservice project through Milestone 11: core services, Web Application, Docker/database setup, Admin book-source ingestion, automated tests, and the submission documentation package.

Project status and remaining work are tracked in `PROJECT_PROGRESS.md`. The completion roadmap is tracked in `PROJECT_SCHEDULE.md`. Update the progress file after each completed project change.

Development conventions:

- API conventions: `docs/development/Api-Conventions.md`
- API Gateway routes: `docs/development/Gateway-Routes.md`
- Clean database/Docker/demo setup: `docs/development/Database-Docker-Demo-Setup.md`
- Seed data strategy: `docs/development/Seed-Data-Strategy.md`
- Book knowledge workflow: `docs/book-knowledge/Book-Knowledge-Integration.md`
- Demo script: `docs/submission/Demo-Script.md`
- Final submission checklist: `docs/submission/Submission-Checklist.md`

## Containers

- `LongevityDiet.Web` - ASP.NET Core MVC web application.
- `LongevityDiet.ApiGateway` - YARP reverse proxy gateway.
- `LongevityDiet.Identity.Api` - REST API for users, roles, authentication, JWT ownership, and eating-preference profiles.
- `LongevityDiet.DietKnowledge.Api` - REST API for categories, guidelines, foods (ingredients with nutrition facts), recipes (with steps), diet regimens, semantic search over approved knowledge, and Admin book-source ingestion. It owns the separate pgvector `VectorDb`.
- `LongevityDiet.MealPlanning.Api` - REST API for meal plans, reminder publishing, and AI recommendation request publishing.
- `LongevityDiet.Tracking.Api` - REST API for diet tracking (following a regimen and/or meal plan), daily and meal tracking, and progress.
- `LongevityDiet.Recommendation.Service` - internal RabbitMQ recommendation worker with approved-knowledge context, safe fallback behavior, and optional Google Gemini integration.
- `LongevityDiet.Notification.Grpc` - internal gRPC notification service called by Tracking.
- `LongevityDiet.NotificationWorker` - worker service for RabbitMQ notification/reminder messages and Brevo/logged email delivery.

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

For detached local demo startup:

```powershell
docker compose up -d --build
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

- Identity: `http://localhost:5001/identity/api/auth/login`  (also `PUT /api/auth/profile/preferences`)
- Diet Knowledge: `http://localhost:5001/diet-knowledge/api/foods`
- Meal Planning: `http://localhost:5001/meal-planning/api/meal-plans`
- Tracking: `http://localhost:5001/tracking/api/daily-trackings`

Send the same `Authorization: Bearer <token>` header through the gateway for protected endpoints. Recommendation Service, Notification Service, Notification Worker, RabbitMQ, and databases are intentionally not exposed through gateway routes.

The services refuse to start with a missing, short or public JWT signing key unless `JWT_ALLOW_DEVELOPMENT_KEY=true` (the default in `.env.example`, for the local demo only). Set `JWT_SIGNING_KEY` to a private 32+ character secret and `JWT_ALLOW_DEVELOPMENT_KEY=false` for any real deployment.

Local demo Identity credentials are configured through `.env`:

- Admin email: `IDENTITY_ADMIN_EMAIL` defaults to `admin@longevity.local`
- Admin password: `IDENTITY_ADMIN_PASSWORD` defaults to `Admin@123456`
- User email: `IDENTITY_USER_EMAIL` defaults to `user@longevity.local`
- User password: `IDENTITY_USER_PASSWORD` defaults to `User@123456`

Database initialization and demo data:

- Identity, Diet Knowledge, Meal Planning, and Tracking apply their own EF Core migrations at service startup.
- Docker Compose waits for PostgreSQL and RabbitMQ health checks before starting dependent services.
- Diet Knowledge loads `book-knowledge-seed.json` as `NeedsReview` review data.
- Diet Knowledge also loads approved local demo knowledge from `demo-approved-knowledge-seed.json` when `DIET_KNOWLEDGE_SEED_INCLUDE_DEMO_APPROVED_CONTENT=true`, which is the default for local teacher demos.
- Admin PDF uploads are stored in the Docker volume `diet-knowledge-book-sources` at `/app/storage/book-sources`.
- Full setup details and environment variables are documented in `docs/development/Database-Docker-Demo-Setup.md`.

## Tests

Automated core tests are in `tests/LongevityDiet.Core.Tests`.

Run the full test suite:

```powershell
dotnet test LongevityDietPlatform.sln
```

The current suite covers:

- Identity register/login behavior.
- Diet Knowledge validation and search/filter/pagination behavior.
- Admin book-source ingestion candidate generation and approval.
- Meal Planning validation, reminder publishing, and recommendation request publishing.
- Tracking progress summary calculation and notification trigger behavior.
- Recommendation safety filtering and approved-knowledge context behavior.
- Notification Worker Brevo/log fallback behavior.
- Reliability: reminder staleness checks, Meal Planning outbox dispatch and back-off, planned-meal progress, JWT signing key rules.
- Semantic search index (VectorDb), diet regimens/categories/nutrition/steps, user profile preferences and diet tracking.
- API authorization boundaries for public, authenticated, and Admin-only endpoints.

Diet Knowledge endpoints:

- Public reads:
  - `GET /api/diet-guidelines`
  - `GET /api/foods`
  - `GET /api/categories`, `GET /api/diet-regimens[/{id}]`, `GET /api/knowledge/search?q=&type=&limit=` (semantic search)
  - `GET /api/recipes`
- Public reads return active `Approved` knowledge only.
- Admin mutations:
  - `POST /api/admin/diet-guidelines`, `PUT|DELETE /api/admin/diet-guidelines/{id}`
  - `POST /api/admin/foods`, `PUT|DELETE /api/admin/foods/{id}`
  - `POST /api/admin/recipes`, `PUT|DELETE /api/admin/recipes/{id}`
  - `PATCH /api/admin/{diet-guidelines|foods|recipes}/{id}/activation`
  - `GET|POST|PUT /api/admin/diet-regimens`, `PATCH /api/admin/diet-regimens/{id}/activation`
  - `POST /api/admin/knowledge-index/rebuild`, `GET /api/admin/knowledge-index/status`
  - `DELETE /api/admin/knowledge?confirm=true` deletes all managed knowledge (candidates go back to `NeedsReview`)
- Admin book-source ingestion:
  - `POST /api/admin/book-sources` multipart PDF or TXT upload
  - `GET /api/admin/book-sources`
  - `GET /api/admin/book-sources/{id}/chunks`
  - `DELETE /api/admin/book-sources/{id}` removes the PDF, its chunks and candidates (approved knowledge is kept)
  - `POST /api/admin/book-sources/{id}/candidates/generate` analyzes the next batch of not-yet-analyzed chunks (call repeatedly until done)
  - `GET /api/admin/book-sources/{id}/candidates/progress` chunks analyzed / remaining / skipped
  - `GET /api/admin/book-sources/{id}/candidates/generate/status` live progress of a running generation
  - `POST /api/admin/book-sources/{id}/candidates/approve-all` approves every pending candidate except placeholders and items with disease/treatment wording
  - `POST /api/admin/book-sources/{id}/candidates/reject-placeholders` rejects the pending local placeholder drafts
  - `GET /api/admin/book-sources/{id}/candidates`
  - `POST /api/admin/knowledge-candidates/{id}/approve`
  - `POST /api/admin/knowledge-candidates/{id}/reject`
- Admin list endpoints can filter by `reviewStatus=NeedsReview|Approved|Rejected`.
- Seeded book-knowledge items are `NeedsReview` until the team fills source metadata and approves them.
- Uploaded PDF chunks and candidates are Admin-only. Public endpoints and Recommendation context continue to use only active `Approved` guidelines, foods, and recipes.

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
- Reminder and recommendation messages are saved to the `OutboxMessages` table in the same transaction as the plan, item or request; a background dispatcher publishes them to RabbitMQ with retries (at-least-once delivery).
- Changing an item publishes a new reminder only when its reminder time changes. The Notification Worker asks the internal endpoint `GET /internal/reminders/{itemId}` (shared key `INTERNAL_API_KEY`, blocked by the gateway) before sending a delayed reminder, so reminders for deleted or rescheduled meals are dropped.
- Recommendation requests are published through the same outbox; completed recommendation results are consumed and stored for user review.
- A separate `MealSchedule` table is not used in this milestone; schedule data is represented by `MealPlanItem.PlannedDate`, `MealSlot`, and `ReminderAtUtc`.

Tracking endpoints:

- All Tracking endpoints require a JWT Bearer token.
- Daily tracking workflow:
  - `GET /api/daily-trackings`
  - `GET /api/daily-trackings/{trackingDate}`
  - `PUT /api/daily-trackings/{trackingDate}`
  - `DELETE /api/daily-trackings/{trackingDate}`
  - `PUT /api/daily-trackings/{trackingDate}/meals/{mealPlanItemId}`  (body `isCompleted`, optional `plannedMealCount`)
- Progress summary workflow:
  - `GET /api/progress-summaries?periodStartDate=YYYY-MM-DD&periodEndDate=YYYY-MM-DD`
- Tracking stores only `MealPlanItemId` as a cross-service ID; it does not read `MealPlanningDb`.
- A day counts the larger of `plannedMealCount` (meals planned that day, sent by the Web check-in page) and the tracked meals. When every counted meal of the day is completed, Tracking calls Notification Service through gRPC.
- Diet tracking (follow a regimen and/or meal plan; one Active per user): `POST|GET /api/diet-trackings`, `GET /api/diet-trackings/{id}`, `PATCH /api/diet-trackings/{id}/status`.
- Notification Service publishes a `NotificationRequested` message to RabbitMQ for later worker delivery.

Notification Worker:

- Consumes scheduled meal reminders from `reminder.requests`.
- Drops a due reminder when Meal Planning says its meal plan item was deleted or rescheduled (fails open if Meal Planning is unreachable).
- Consumes progress notifications from `notification.messages`.
- Declares durable RabbitMQ exchanges/queues plus dead-letter exchanges/queues for both flows.
- Retries failed messages using `RabbitMq__MaxDeliveryAttempts` and `RabbitMq__RetryDelaySeconds`.
- Sends through Brevo (`POST /v3/smtp/email`, `api-key` header) when both `BREVO_API_KEY` and `BREVO_FROM_EMAIL` are configured. The sender must be verified in Brevo (Senders, Domains & Dedicated IPs).
- Logs the expected email payload when `BREVO_API_KEY` is empty or `development-only`, or `BREVO_FROM_EMAIL` is empty, which is the default local fallback.

Recommendation Service:

- Consumes `RecommendationRequest` messages from `recommendation.requests`.
- Calls Diet Knowledge public APIs to build context only from active `Approved` knowledge.
- Filters unsafe user preference tags before they are used in recommendation titles.
- Uses local safe fallback recommendations when `GEMINI_ENABLED=false` or `GEMINI_API_KEY` is empty/`development-only`.
- Can call Gemini through a configurable HTTP client when `GEMINI_ENABLED=true`, `GEMINI_API_KEY` is configured, and `GEMINI_MODEL` is set.
- Publishes `RecommendationResult` messages to `recommendation.results`, where Meal Planning consumes and stores them for user review.
- Keeps the required safety disclaimer and avoids diagnosis, treatment advice, disease prediction, and lifespan prediction.

Admin Book Sources:

- Sign in as Admin and open `Book Sources` in the Web navigation.
- Upload a legally obtained PDF copy for private project research.
- The Diet Knowledge Service extracts chunks, stores them in `DietKnowledgeDb`, and can use Gemini to generate candidate summaries.
- If Gemini is disabled, safe placeholder candidates are created so the workflow can still be demonstrated.
- Approving a candidate creates managed Diet Knowledge content; raw chunks are never user-facing content.

## Database Design (five modules + separate vector database)

The diagram is `docs/database/LongevityDiet-Database.drawio` (explained in `Database-Diagram-Explanation.docx`): Module 1 User (`IdentityDb`), Module 2 Diet and Module 3 Diet Regimen (`DietKnowledgeDb`), Module 4 Meal Plan (`MealPlanningDb`), Module 5 UserDietTracking (`TrackingDb`), and the separate pgvector `VectorDb` (port 5437) used for semantic search over approved knowledge. Details: `docs/database/Physical-Database.md` and `docs/development/Database-Docker-Demo-Setup.md`.

## Demo Path

Use the full step-by-step script in `docs/submission/Demo-Script.md`.

Minimum browser demo:

1. Start Docker Compose.
2. Open `http://localhost:5000`.
3. Login as Admin with `admin@longevity.local` / `Admin@123456`.
4. Show Admin knowledge management and `Book Sources`.
5. Logout or open another session and login as User with `user@longevity.local` / `User@123456`.
6. Browse diet knowledge (try Smart search) and the Diet Regimens, save eating preferences on the Profile, create a meal plan, request a recommendation, accept it, start a diet tracking, mark meals complete, and review progress.
7. Show RabbitMQ Management at `http://localhost:15672` for queues/exchanges.
8. Run `dotnet test LongevityDietPlatform.sln` to show automated coverage.

## Remaining TODO

- Complete team source verification and approval for real book-derived seed/candidate data.
- Run final QA before submission using `docs/submission/Submission-Checklist.md`.
