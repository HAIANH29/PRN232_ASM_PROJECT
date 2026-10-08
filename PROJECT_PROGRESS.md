# Project Progress — Longevity Diet Platform

This file is the shared progress tracker for the project. Every human or AI agent must read it before changing the repository and update it after completing any work.

## Current Status

- Phase: Milestone 1 Identity Service complete; continuing toward feature-complete microservices.
- Branch: `HA/phase-1-microservices-scaffold`.
- Last baseline: Milestone 0 shared foundation after commit `a8dd8e2`.
- Verification baseline: `dotnet restore LongevityDietPlatform.sln`, `dotnet build LongevityDietPlatform.sln --no-restore -m:1 /p:UseSharedCompilation=false --verbosity minimal`, `docker compose config`, and Identity Docker smoke tests passed on 2026-10-08.
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
- Updated Recommendation Service to use RabbitMQ request/result messages with Google Gemini placeholder configuration.
- Added Notification Service as the required internal gRPC service for `Tracking Service -> Notification Service`.
- Replaced the old Reminder Worker role with `NotificationWorker`.
- Added Notification Worker placeholder for notification/reminder messages and future Resend delivery.
- Updated C4, ERD, physical database docs, README, `.env.example`, and instruction files to match the updated architecture.
- Added `PROJECT_PROGRESS.md` and `PROJECT_SCHEDULE.md` to track current status and the path to a submission-ready product.
- Implemented Identity Service register/login/profile/admin-check APIs with DTO validation, password hashing, JWT issuing, role seed data, demo admin seed, authorization policy, repository persistence, and the initial Identity EF Core migration.

## Incomplete / Remaining Work

- Add business DTOs, endpoint-specific validation rules, and correct HTTP status handling to each public REST API.
- Implement Diet Knowledge admin CRUD, user browse/search/filter/sort/pagination, and activation/deactivation.
- Implement Meal Planning create/update/delete workflows, scheduling rules, ownership checks, and real reminder publishing behavior.
- Implement Meal Planning consumption of `RecommendationResult` and the user accept/edit flow for AI recommendations.
- Implement Recommendation Service prompt/context creation from approved knowledge and optional real Google Gemini calls.
- Implement Tracking APIs for meal completion, daily tracking, history, and progress summaries.
- Wire real Tracking use cases to call Notification Service via gRPC when notifications are required.
- Implement durable RabbitMQ topology, retries, error handling, idempotency, and dead-letter behavior where needed.
- Implement Notification Worker email delivery through Resend, including retry/logging behavior.
- Add EF Core migrations for Diet Knowledge, Meal Planning, and Tracking.
- Add automated tests for implemented slices.
- Build the real Web Application screens and API Gateway integration.
- Run an end-to-end Docker Compose smoke test once business flows exist.

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
