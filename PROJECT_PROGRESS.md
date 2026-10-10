# Project Progress — Longevity Diet Platform

This file is the shared progress tracker for the project. Every human or AI agent must read it before changing the repository and update it after completing any work.

## Current Status

- Phase: Milestone 11 documentation and submission package complete; manual real book source review/approval and final QA remain.
- Branch: `HA/phase-1-microservices-scaffold`.
- Last pushed baseline before this update: Milestone 9 after commit `f0ec65a`.
- Verification baseline: `dotnet restore LongevityDietPlatform.sln`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config --quiet`, `docker compose up -d --build`, Admin PDF ingestion API smoke, 29 automated core tests, and documentation validation passed on 2026-10-10.
- Completion roadmap: `PROJECT_SCHEDULE.md`.

## Done

- Created the .NET solution and project layout for the required containers/services.
- Added Web Application, YARP API Gateway, Identity, Diet Knowledge, Meal Planning, Tracking, Recommendation, Notification, RabbitMQ, Notification Worker, and PostgreSQL database skeletons.
- Added layered structure for REST services: API, Application, Infrastructure, Domain.
- Added EF Core DbContext shells for `IdentityDb`, `DietKnowledgeDb`, `MealPlanningDb`, and `TrackingDb`.
- Added phase-1 domain entities:
  - Identity: `User`, `Role`
  - Diet Knowledge: `DietGuideline`, `Food`, `Recipe`, `RecipeIngredient`
  - Meal Planning: `MealPlan`, `MealPlanItem`
  - Tracking: `DailyTracking`, `MealTracking`, `ProgressSummary`
- Added Swagger and health endpoint scaffolding for public REST APIs.
- Added shared API defaults for public REST APIs: response envelope, paged response model, validation error response factory, global exception middleware, Swagger Bearer setup, and health/controller pipeline extension.
- Added Dockerfiles and `docker-compose.yml` skeleton for local development.
- Added RabbitMQ contract/configuration skeletons for reminder, notification, and recommendation flows.
- Updated Recommendation Service to use RabbitMQ request/result messages with approved Diet Knowledge context, Google Gemini client configuration, and safe disabled-mode fallback.
- Added Notification Service as the required internal gRPC service for `Tracking Service -> Notification Service`.
- Replaced the old Reminder Worker role with `NotificationWorker`.
- Added Notification Worker placeholder for notification/reminder messages and future Resend delivery.
- Updated C4, ERD, physical database docs, README, `.env.example`, and instruction files to match the updated architecture.
- Added `PROJECT_PROGRESS.md` and `PROJECT_SCHEDULE.md` to track current status and the path to a submission-ready product.
- Implemented Identity Service register/login/profile/admin-check APIs with DTO validation, password hashing, JWT issuing, role seed data, demo admin seed, authorization policy, repository persistence, and the initial Identity EF Core migration.
- Implemented Diet Knowledge Service admin CRUD, activation/deactivation, public read endpoints, search/filter/sort/pagination, managed seed content, and the initial Diet Knowledge EF Core migration.
- Added Diet Knowledge source/provenance and review metadata for book-derived guidelines, foods, and recipes.
- Moved Diet Knowledge seed data to a version-controlled JSON file with seed entries marked `NeedsReview` until team source verification.
- Restricted public Diet Knowledge reads and recipe ingredient validation to active `Approved` content.
- Implemented Meal Planning Service meal plan CRUD, meal plan item scheduling, ownership checks, approved Diet Knowledge validation through REST, RabbitMQ reminder publishing, RabbitMQ recommendation request publishing, recommendation result consumption/storage, recommendation accept flow, and the initial Meal Planning EF Core migration.
- Implemented Tracking Service daily tracking, meal completion/not-completion tracking, progress summary calculation, ownership checks, startup migration, and the initial Tracking EF Core migration.
- Implemented the real Tracking Service -> Notification Service gRPC progress notification flow.
- Updated Notification Service to publish accepted progress notification messages to RabbitMQ.
- Implemented Notification Worker reminder and notification consumers, durable RabbitMQ topology declarations, retry handling, dead-letter queues, Resend sender abstraction, and development log fallback delivery.
- Implemented Recommendation Service RabbitMQ consumer, approved-knowledge REST context loading, safe prompt builder, Gemini client abstraction, disabled-mode fallback recommendations, retry/dead-letter handling, and result publishing back to RabbitMQ.
- Finalized API Gateway YARP routes for Identity, Diet Knowledge, Meal Planning, and Tracking, with internal Recommendation/Notification services kept unrouted.
- Implemented Web Application screens for login/register/profile, diet knowledge browsing, admin content management, meal planning, tracking/progress, and recommendation request/review/accept flows.
- Wired the Web Application to call public backend services only through the API Gateway.
- Fixed Meal Planning item insertion for existing meal plans so Web meal-plan item creation stores a new `MealPlanItem` instead of attempting to update a missing row.
- Added clean Docker/database setup documentation, Docker health checks, demo admin/user seed accounts, and approved local demo Diet Knowledge seed data for browser demos.
- Added Admin-only book PDF ingestion in Diet Knowledge Service: upload, PDF text extraction, chunk storage, Gemini-assisted candidate generation with local fallback, candidate approve/reject, EF Core migration, and Web Admin Book Sources screens.
- Added automated core test coverage for Identity, Diet Knowledge, Admin book ingestion, Meal Planning, Tracking progress, Recommendation safety/context, Notification Worker email delivery, and API authorization boundaries.
- Added Milestone 11 submission documentation: updated README, C4 notes, ERD, physical database docs, demo setup docs, book workflow docs, demo script, and final submission checklist.

## Incomplete / Remaining Work

- Verify each real book-derived seed/candidate item against a legally obtained source copy and approve only reviewed items.
- Run final QA before submission.

## Update Rules

When completing any project work, update this file in the same change set.

Each update should include:
- Date.
- Branch and commit when known.
- What changed.
- Files or areas affected.
- Verification performed.
- Remaining TODOs, assumptions, or blockers.

Do not mark a feature as complete if it only has placeholders or configuration. Call it a scaffold until the real behavior exists and has been verified.

## Change Log

### 2026-10-10 — Complete Milestone 11 Documentation And Submission Package

- Updated README with current project status, test instructions, demo links, and a minimum browser demo path.
- Updated C0/C1 notes, conceptual ERD, physical database docs, database/demo setup docs, and book-knowledge workflow docs to match the implemented architecture through Admin PDF ingestion and automated tests.
- Added `docs/submission/Demo-Script.md` with exact presentation steps for architecture, Admin flow, User flow, Recommendation flow, Tracking/Notification flow, RabbitMQ evidence, and tests.
- Added `docs/submission/Submission-Checklist.md` with final verification, functional demo, architecture, documentation, book/AI safety, and git checks.
- Updated `PROJECT_SCHEDULE.md` to mark Milestone 11 complete.
- Verification: documentation review, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config --quiet`, and `git diff --check` passed.
- Remaining TODO: manually verify/approve real book-derived knowledge from a legally obtained copy and run final QA before submission.

### 2026-10-10 — Complete Milestone 10 Automated Tests

- Added `tests/LongevityDiet.Core.Tests` to the solution.
- Added centralized test package versions for xUnit, the .NET test SDK, and EF Core InMemory.
- Added unit tests for Identity registration/login behavior, Diet Knowledge validation, Admin book PDF ingestion candidate generation/approval, Meal Planning validation/reminder/recommendation publishing, Tracking progress summary notification behavior, Recommendation safety/context behavior, and Notification Worker Resend/log-fallback behavior.
- Added lightweight repository/integration tests for Diet Knowledge search/filter/pagination and Tracking progress calculation using EF Core InMemory.
- Added API authorization boundary tests for Admin-only Diet Knowledge endpoints, authenticated Meal Planning/Tracking endpoints, anonymous public browse endpoints, and Identity profile/admin-check endpoints.
- Verification: `dotnet restore LongevityDietPlatform.sln`, `dotnet test tests\LongevityDiet.Core.Tests\LongevityDiet.Core.Tests.csproj --no-restore -m:1 --verbosity minimal`, and `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal` passed with 29 tests.
- Remaining TODO: manually verify/approve real book-derived knowledge from a legally obtained copy, add final demo script/submission checklist, and run final QA before submission.

### 2026-10-10 — Complete Milestone 9.5 Admin Book PDF Ingestion And AI Chunking

- Updated project rules and documentation so PDF ingestion is Admin-only, owned by Diet Knowledge Service, and does not expose raw chunks to normal users or Recommendation context.
- Added Diet Knowledge domain/application/infrastructure/API support for `BookSourceDocument`, `BookSourceChunk`, and `KnowledgeCandidate`.
- Added PDF upload, PdfPig text extraction, chunking, Gemini-assisted candidate generation, safe local fallback candidate generation, candidate approval into managed guideline/food knowledge, and candidate rejection.
- Added the Diet Knowledge EF Core migration `AddBookSourceIngestion`.
- Added Web Admin Book Sources screens for upload, source details, chunk review, candidate generation, approval, and rejection.
- Added Docker storage volume/configuration for uploaded book PDFs and updated README, book-knowledge docs, database docs, ERD, C1 architecture docs, seed strategy, `.env.example`, and `PROJECT_SCHEDULE.md`.
- Verification: `dotnet restore LongevityDietPlatform.sln`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config --quiet`, `git diff --check`, `docker compose up -d --build`, targeted `docker compose up -d --build diet-knowledge-service`, Web home smoke, Diet Knowledge health smoke, gateway routes smoke, admin login through the gateway, temporary PDF upload through the gateway, chunk extraction, candidate generation, and candidate approval into managed Diet Knowledge.
- Smoke document: `f00ec797-44bc-464b-bb2b-377fdfbc449a`; approved knowledge: `d2099645-32ea-44a6-95da-ebaa2a72c051`.
- Remaining TODO: use the team's legally obtained real PDF/source copy, review generated candidates manually, approve only verified items, add automated tests, and add the final demo/submission checklist.

### 2026-10-10 — Complete Milestone 9 Database, Docker, And Demo Data

- Confirmed EF Core migrations exist for all four stateful service databases: Identity, Diet Knowledge, Meal Planning, and Tracking.
- Added configurable demo user seeding alongside the existing demo admin and role seed data.
- Added approved local demo Diet Knowledge seed data for public browser flows while keeping the source-review book seed data as `NeedsReview`.
- Added Docker Compose PostgreSQL/RabbitMQ health checks and startup dependency conditions so services wait for core infrastructure before starting.
- Added database initialization, environment variable, demo account, demo data, and local reset instructions in `docs/development/Database-Docker-Demo-Setup.md`.
- Updated README, seed strategy docs, book-knowledge docs, `.env.example`, and the schedule.
- Verification: `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config --quiet`, `docker compose up -d --build`, `docker compose up -d --force-recreate`, targeted `docker compose up -d --build diet-knowledge-service`, `docker compose ps`, gateway route smoke, Web home smoke, demo admin/user login through the gateway, and public Diet Knowledge smoke showing seeded guidelines, foods, recipes, plus targeted demo seed items.
- Remaining TODO: team still needs to replace demo source placeholders with exact book source metadata after reviewing a legally obtained copy; automated tests and final submission/demo checklist remain.

### 2026-10-10 — Fix Web Footer Layout

- Removed the MVC template footer absolute positioning that caused the footer to float over page content on longer Web screens.
- Changed the Web shell to a flex column layout so the footer sits at the bottom of short pages and after the content on long pages.
- Verification: `dotnet build src/Web/LongevityDiet.Web/LongevityDiet.Web.csproj --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `docker compose up -d --build web`, Web home returned `200`, served `LongevityDiet.Web.styles.css` no longer contains `position: absolute` for footer and includes `flex-shrink: 0`, and `git diff --check` passed.

### 2026-10-10 — Complete Milestone 8 Web Application

- Added MVC Web Application infrastructure for calling the API Gateway with `ApiGatewayClient`, storing the JWT in an HttpOnly cookie, and rendering signed-in/admin navigation from `UserSession`.
- Added browser screens for login, register, profile, diet guideline/food/recipe browsing, admin guideline/food/recipe management, meal plan creation/editing, meal item scheduling, tracking/progress, and recommendation request/review/accept.
- Added user-friendly TempData alerts, model validation messages, and responsive operational styling for the Web UI.
- Updated Web configuration so local development calls `http://localhost:5001` and Docker calls `http://api-gateway:8080`.
- Fixed Meal Planning Service add-item persistence by adding an explicit repository `AddMealPlanItemAsync` method and using it for existing meal plans.
- Verification: `dotnet build src/Web/LongevityDiet.Web/LongevityDiet.Web.csproj --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet build src/Services/MealPlanning/LongevityDiet.MealPlanning.Api/LongevityDiet.MealPlanning.Api.csproj --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `docker compose up -d --build web api-gateway identity-service diet-knowledge-service meal-planning-service tracking-service recommendation-service notification-service notification-worker rabbitmq`, Web and gateway health checks, admin login through Web, admin-created approved food/recipe through Web, user registration/profile through Web, public knowledge browse through Web, meal plan creation plus meal item scheduling through Web, tracking completion through Web, recommendation request completion through RabbitMQ, and recommendation accept into a new meal plan through Web.
- Smoke run: `1791610849`; user `m8.web.1791610849@longevity.local`; Web endpoint `http://localhost:5000`.
- Remaining TODO: add automated tests, add final demo script/submission checklist, and complete manual book-source verification/approval for seed data.

### 2026-10-09 — Complete Milestone 7 API Gateway Integration

- Kept the gateway as the single backend entry point for public REST services.
- Finalized YARP routes for:
  - `/identity/**` -> Identity Service
  - `/diet-knowledge/**` -> Diet Knowledge Service
  - `/meal-planning/**` -> Meal Planning Service
  - `/tracking/**` -> Tracking Service
- Added a `/routes` gateway endpoint that lists public route prefixes for local inspection.
- Added Development environment downstream addresses for running the gateway against services exposed on localhost ports `5101`-`5104`.
- Added `docs/development/Gateway-Routes.md` and README route examples.
- Confirmed Recommendation Service, Notification Service, Notification Worker, RabbitMQ, and PostgreSQL are not exposed through gateway routes.
- Verification: `dotnet build src/Gateways/LongevityDiet.ApiGateway/LongevityDiet.ApiGateway.csproj --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config --quiet`, `docker compose up -d --build api-gateway identity-service diet-knowledge-service meal-planning-service tracking-service`, API Gateway health check, route summary check, Diet Knowledge public read through gateway, Identity login/profile through gateway, Meal Planning protected route returned `401` without token and `200` with token, Tracking progress summary returned through gateway with token, and `/recommendation/health` plus `/notification/health` returned `404` through the gateway.
- Remaining TODO: build the real Web Application screens, add automated tests, add final demo script/checklist, and keep book source approval work explicit.

### 2026-10-09 — Complete Milestone 6 Recommendation Service and Gemini Placeholder/Integration

- Replaced the Recommendation Service placeholder loop with a real RabbitMQ consumer for `recommendation.requests`.
- Added durable recommendation request/result topology declarations plus a manual dead-letter exchange/queue for failed recommendation requests.
- Added retry handling through `RabbitMq__MaxDeliveryAttempts`, `RabbitMq__RetryDelaySeconds`, and persistent republished retry messages.
- Added `IApprovedKnowledgeClient` and HTTP Diet Knowledge reads for public active `Approved` guidelines, foods, and recipes.
- Added safe recommendation context/prompt construction from approved project knowledge plus sanitized user preference tags.
- Added safety guardrails so suggestion titles do not include diagnosis, treatment, disease prediction, lifespan prediction, or similar out-of-scope terms.
- Added `IGeminiClient` and configurable Gemini HTTP integration behind `Gemini__Enabled`, `Gemini__ApiKey`, and `Gemini__Model`.
- Added disabled-mode fallback recommendations for local/demo use when Gemini is not configured.
- Published `RecommendationResult` messages back to RabbitMQ for Meal Planning to consume and store as completed recommendation requests.
- Updated Docker Compose, `.env.example`, README, book-knowledge docs, and `PROJECT_SCHEDULE.md`.
- Verification: `dotnet restore src/Services/Recommendation/LongevityDiet.Recommendation.Service/LongevityDiet.Recommendation.Service.csproj`, `dotnet build src/Services/Recommendation/LongevityDiet.Recommendation.Service/LongevityDiet.Recommendation.Service.csproj --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config --quiet`, `docker compose up -d --build identity-service diet-knowledge-service meal-planning-service recommendation-service`, health checks for Identity/Diet Knowledge/Meal Planning, admin-created approved guideline/food/recipe through Diet Knowledge, user-created recommendation request through Meal Planning, confirmed the request became `Completed`, confirmed Gemini-disabled fallback returned safe suggestion titles and a non-medical disclaimer, confirmed RabbitMQ `recommendation.requests` and `recommendation.results` were drained to 0 messages with active consumers, and confirmed Recommendation Service logs showed request completion.
- Smoke run: `1791554427`; request `58abd464-a58f-4f59-a8c6-71c89b3b3451` completed with 3 suggestions.
- Remaining TODO: add automated tests, finalize API Gateway routes, build Web Application screens, and configure real Gemini/Resend keys only if needed for demo/production external calls.

### 2026-10-09 — Complete Milestone 5 RabbitMQ and Notification Delivery

- Replaced the Notification Worker placeholder loop with real RabbitMQ consumers for `reminder.requests` and `notification.messages`.
- Added durable exchange/queue declarations for reminder and notification flows, plus matching dead-letter exchanges and queues.
- Added retry handling through `RabbitMq__MaxDeliveryAttempts`, `RabbitMq__RetryDelaySeconds`, and persistent republished retry messages.
- Added failure logging and dead-letter routing after retry attempts are exhausted.
- Added `IEmailSender`, `EmailMessage`, and `ResendEmailSender`.
- Added Resend configuration for base URL, endpoint, API key, sender email, and sender name.
- Added development fallback behavior: when `RESEND_API_KEY` is empty or `development-only`, the worker logs the email payload instead of calling Resend.
- Updated Docker Compose, `.env.example`, README, and `PROJECT_SCHEDULE.md`.
- Verification: `dotnet build src/Workers/LongevityDiet.NotificationWorker/LongevityDiet.NotificationWorker.csproj --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal` with `TEMP/TMP` pointed at the repository `.tmp` folder on drive D, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config --quiet`, `git diff --check`, `docker compose up -d --build identity-service diet-knowledge-service meal-planning-service notification-service tracking-service notification-worker`, health checks for Identity/Diet Knowledge/Meal Planning/Tracking/Notification, created approved Diet Knowledge food, created Meal Planning plan with reminder, confirmed Notification Worker logged the reminder email payload, marked Tracking progress complete, confirmed Tracking -> Notification gRPC -> RabbitMQ -> Notification Worker logged the progress notification email payload, and confirmed RabbitMQ Management showed durable `longevity.reminders`, `longevity.notifications`, and matching dead-letter exchanges/queues with active consumers on `reminder.requests` and `notification.messages`.
- Smoke run: `1791553445`; progress summary reached `1/1`; Notification Worker saw both reminder and notification logs.
- Remaining TODO: add automated tests, implement Recommendation Service processing/Gemini-safe context, finalize API Gateway routes, and build Web Application screens.

### 2026-10-09 — Complete Milestone 4 Tracking Service and gRPC Notification

- Added authenticated Tracking API contracts and controllers for daily tracking history, daily tracking upsert/delete, meal completion/not-completion, and progress summary reads.
- Added application-layer ownership checks using the user id from JWT claims.
- Added Tracking Service progress summary calculation for daily/weekly-style date ranges.
- Added the real progress-notification business trigger: when a user completes all planned meals for a tracked day, Tracking Service calls Notification Service through gRPC.
- Added Tracking persistence for `DailyTracking`, `MealTracking`, and `ProgressSummary`, including internal-only `MealTracking -> DailyTracking` relationship and cross-service `MealPlanItemId` references by id only.
- Added startup migration initialization and the initial Tracking EF Core migration `InitialTrackingSchema`.
- Updated Notification Service to accept progress notification gRPC requests and publish `NotificationRequested` messages to RabbitMQ.
- Split Notification Service Kestrel endpoints so internal gRPC uses HTTP/2 on port `8080` and host health checks use HTTP/1 on port `8081`.
- Updated Docker Compose, README, ERD, physical database docs, seed strategy, and `PROJECT_SCHEDULE.md`.
- Verification: `dotnet restore LongevityDietPlatform.sln`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config`, `git diff --check`, `docker compose up -d --build notification-service tracking-service`, Tracking and Notification health checks, user daily tracking upsert, meal marked not completed then completed, progress summary updated from `0/1` to `1/1`, another user denied access with `404`, and RabbitMQ `notification.messages` increased after the gRPC notification flow.
- Remaining TODO: add automated tests for Tracking/Notification, implement Notification Worker consumption and Resend delivery, implement full Recommendation Service processing/Gemini-safe context, finalize API Gateway routes, and build Web Application screens.

### 2026-10-09 — Complete Milestone 3 Meal Planning Service

- Added Meal Planning API DTOs and authenticated controllers for meal plan CRUD, meal plan item CRUD, recommendation request review, and recommendation accept flow.
- Added application-layer ownership checks using the user id from JWT claims.
- Added validation for meal plan date ranges, item planned dates, meal slots, exactly one food/recipe reference per item, and recommendation request duration.
- Added an HTTP Diet Knowledge catalog client so Meal Planning validates `FoodId` and `RecipeId` through public Diet Knowledge REST APIs instead of reading another service database.
- Added RabbitMQ publishing for scheduled meal reminders and recommendation requests.
- Added RabbitMQ recommendation result consumer/hosted worker that stores completed recommendation result data for user review.
- Added `MealRecommendationRequest` persistence and the initial Meal Planning EF Core migration `InitialMealPlanningSchema`.
- Added startup migration initialization for `MealPlanningDb`.
- Updated Docker Compose with Meal Planning dependency on Diet Knowledge and RabbitMQ, plus Diet Knowledge base URL configuration.
- Updated README, ERD, physical database docs, seed strategy, and `PROJECT_SCHEDULE.md`.
- Implementation note: a separate `MealSchedule` table was not needed for the current workflow; schedule data is represented by `MealPlanItem.PlannedDate`, `MealSlot`, and `ReminderAtUtc`.
- Verification: `dotnet restore LongevityDietPlatform.sln`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config`, `docker compose up -d --build identity-service diet-knowledge-service meal-planning-service`, health checks for Identity/Diet Knowledge/Meal Planning, admin-created approved food through Diet Knowledge, user-created meal plan through Meal Planning, invalid food reference rejected with `400`, another user denied access to the first user's plan with `404`, recommendation request published to RabbitMQ, synthetic recommendation result consumed and stored as `Completed`, accepted recommendation saved as a new meal plan, RabbitMQ showed `reminder.requests` with 2 messages, `recommendation.requests` with 1 message, and `recommendation.results` consumed/acked to 0 messages.
- Remaining TODO: add automated tests for the Meal Planning slice, implement real Recommendation Service processing/Gemini context, implement Notification Worker consumption, and complete Tracking Service.

### 2026-10-09 — Add Book Knowledge integration foundation

- Added source metadata and review status fields to `DietGuideline`, `Food`, and `Recipe`.
- Added `NeedsReview`, `Approved`, and `Rejected` review workflow rules in the Diet Knowledge application layer.
- Added admin DTO support for source metadata and review status, including validation that `Approved` content has source chapter, source page/reference, and reviewer.
- Updated public Diet Knowledge reads to return only active `Approved` content.
- Updated recipe ingredient validation so recipes can only reference active approved foods.
- Replaced hard-coded Diet Knowledge seed content with `book-knowledge-seed.json`; seed entries remain `NeedsReview` until the team verifies source locations.
- Added Diet Knowledge EF Core migration `AddBookKnowledgeReviewMetadata`.
- Added book-knowledge documentation and updated README, seed strategy, ERD, physical database docs, and `PROJECT_SCHEDULE.md`.
- Verification: `dotnet restore LongevityDietPlatform.sln`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `dotnet test LongevityDietPlatform.sln --no-restore -m:1 --verbosity minimal`, `docker compose config`, `git diff --check`, `docker compose up -d --build identity-service diet-knowledge-service`, Identity/Diet Knowledge health checks, admin login, rejected invalid approved content with `400`, created approved guideline/food/recipe through admin APIs, confirmed public reads returned the approved smoke items, confirmed admin `reviewStatus=NeedsReview` saw seeded foods, and confirmed invalid `reviewStatus` filter returned `400`.
- Remaining TODO: the team must verify the legal source copy, fill exact source chapter/page/reference metadata for seed items, and mark verified items `Approved` before using them as demo/public knowledge.

### 2026-10-08 — Complete Milestone 2 Diet Knowledge Service

- Added public read endpoints for diet guidelines, foods, and recipes.
- Added admin endpoints for diet guideline, food, and recipe create/update/read/list plus activation/deactivation.
- Added API DTOs and Application models/commands so EF entities are not returned directly.
- Added food/recipe search, category/ingredient filtering, sort options, and paged list responses.
- Added recipe ingredient handling with validation that recipes reference active approved foods.
- Added approved demo seed data for guidelines, foods, recipes, and recipe ingredients.
- Added the initial Diet Knowledge EF Core migration and idempotent startup migration/seed with retry for Docker startup timing.
- Updated README, API conventions, seed/database docs, ERD, and `PROJECT_SCHEDULE.md`.
- Verification: `dotnet restore LongevityDietPlatform.sln`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `docker compose config`, `git diff --check`, `docker compose up -d --build identity-service diet-knowledge-service`, `GET /health`, public food/recipe search/filter/pagination, admin create/update/deactivate for guidelines/foods/recipes, admin endpoint without token returning `401`, normal user admin mutation returning `403`, public inactive resource lookup returning `404`, and admin inactive resource lookup returning `200`.

### 2026-10-08 — Complete Milestone 1 Identity Service

- Added Identity persistence for `User`, `Role`, and the EF-generated `UserRoles` join table, including normalized email/name uniqueness.
- Added register, login, profile, and admin authorization check endpoints under `/api/auth`.
- Added PBKDF2 password hashing, JWT access token generation, `User`/`Admin` role seed data, and a configurable demo admin account.
- Added `AdminOnly` authorization policy and shared JWT validation configuration for public APIs.
- Added the initial Identity EF Core migration and idempotent startup migration/seed with retry for Docker startup timing.
- Updated Docker Compose and `.env.example` with Identity seed and JWT settings.
- Updated README, seed/database docs, and `PROJECT_SCHEDULE.md`.
- Verification: `dotnet restore LongevityDietPlatform.sln`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `docker compose config`, `docker compose up -d --build identity-service`, `GET /health`, register/login smoke tests, profile without/with invalid token returning `401`, admin-check returning `200` for admin and `403` for a normal user.

### 2026-10-08 — Complete Milestone 0 baseline foundation

- Added `LongevityDiet.ApiDefaults` shared building block.
- Added shared `ApiResponse<T>`, `ApiError`, `PageRequest`, and `PagedResponse<T>` models.
- Added global exception middleware and shared model-validation error response behavior.
- Added shared public API startup extension for controllers, Swagger, JWT Bearer setup, health checks, authentication, and authorization.
- Applied the shared API defaults to Identity, Diet Knowledge, Meal Planning, and Tracking public APIs.
- Updated service-info endpoints to return the common API response envelope.
- Added API convention and seed-data strategy docs.
- Updated `PROJECT_SCHEDULE.md` to mark Milestone 0 complete.
- Verification: `dotnet restore LongevityDietPlatform.sln`, `git diff --check`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, and `docker compose config` passed.

### 2026-10-08 — Add completion schedule

- Added `PROJECT_SCHEDULE.md` with the current completion snapshot, definition of 100% complete, milestone schedule, minimum submission path, and per-service checklist.
- Updated `README.md` to link the schedule.
- Verification: `git diff --check` passed; `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal` passed with 0 warnings and 0 errors.

### 2026-10-08 — Add project progress tracking

- Added this progress tracker.
- Added rules to `AGENTS.md` and `AGENTS_UPDATED.md` requiring agents to read and update this file.
- Linked the tracker from `README.md`.
- Verification: `git diff --check` passed; `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal` passed with 0 warnings and 0 errors.

### 2026-10-08 — Align scaffold with updated architecture

- Replaced the previous direct Meal Planning -> Recommendation gRPC design with RabbitMQ-based recommendation request/result flow.
- Added Notification Service gRPC skeleton and Tracking gRPC client skeleton.
- Replaced Reminder Worker with Notification Worker.
- Updated compose, docs, README, environment sample, and contracts.
- Verification: restore/build/compose config passed before this tracker was created.
