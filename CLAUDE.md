# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

The repo root is `PRN232_ASM_PROJECT/` (the git repo). `AGENTS.md` is the authoritative architecture/scope document (a variant exists in `AGENTS_UPDATED.md`); `PROJECT_PROGRESS.md` is the shared progress tracker. Read both before changing things, and **update `PROJECT_PROGRESS.md` in the same change** (completed work, verification, TODOs). If a request conflicts with `AGENTS.md`, ask before making a major architecture change.

## Commands

.NET 9 SDK (`global.json` pins 9.0.318, rolls forward). Package versions are centrally managed in `Directory.Packages.props` — add `<PackageReference>` without a version and put the version there.

```powershell
$env:DOTNET_CLI_HOME = "$PWD\.dotnet"     # README convention for a repo-local dotnet home
dotnet restore LongevityDietPlatform.sln
dotnet build LongevityDietPlatform.sln
dotnet test LongevityDietPlatform.sln
dotnet test tests/LongevityDiet.Core.Tests --filter "FullyQualifiedName~TrackingServiceTests"   # single class / test
copy .env.example .env; docker compose up -d --build   # full stack (Postgres x4, RabbitMQ, all services)
```

Local ports: Web 5000, Gateway 5001 (`/routes` summary), Identity 5101, DietKnowledge 5102, MealPlanning 5103, Tracking 5104, Notification 5105, RabbitMQ UI 15672. Demo logins (from `.env`): `admin@longevity.local` / `Admin@123456`, `user@longevity.local` / `User@123456`.

All tests live in the single project `tests/LongevityDiet.Core.Tests` (xunit, EF InMemory, hand-written fakes in `TestDoubles.cs`).

## Architecture

Educational diet planning/tracking microservices app based on the book *The Longevity Diet* (PRN232 assignment). Not a medical product: never add diagnosis, treatment, disease prediction, or lifespan prediction.

- **Layout**: `src/Services/<Name>/LongevityDiet.<Name>.{Api,Application,Infrastructure,Domain}` for the four REST services (Identity, DietKnowledge, MealPlanning, Tracking). Dependencies are one-directional; controllers must not touch DbContext — go through Application services and Infrastructure repositories. Never expose EF entities; use DTOs.
- **Shared libs** (`src/BuildingBlocks`): `LongevityDiet.ApiDefaults` (response envelope, paged model, validation response factory, global exception middleware, Swagger Bearer setup, health/controller pipeline) and `LongevityDiet.Contracts` (RabbitMQ message contracts: reminder, notification, recommendation request/result). New REST APIs should reuse ApiDefaults (see `docs/development/Api-Conventions.md`).
- **Non-REST pieces**: `Recommendation.Service` (RabbitMQ worker, no DB, optional Gemini), `Notification.Grpc` (gRPC, no DB), `NotificationWorker` (RabbitMQ consumer → Resend email, falls back to logging), `ApiGateway` (YARP; prefixes `/identity`, `/diet-knowledge`, `/meal-planning`, `/tracking` — Recommendation/Notification/Worker are deliberately not routed), `Web` (ASP.NET Core MVC calling only the gateway).
- **Database per service** (PostgreSQL: IdentityDb, DietKnowledgeDb, MealPlanningDb, TrackingDb). No cross-DB reads or FKs; cross-service references are bare IDs (e.g. Tracking stores `MealPlanItemId`). Each service applies its own EF migrations at startup. Recommendation and Notification stay stateless.
- **Auth**: JWT Bearer issued by Identity, shared `Jwt__SigningKey` across services; roles `User`/`Admin`. Users only see their own meal plans/tracking.

- **Web i18n**: the MVC UI has an English/Vietnamese switch (`LanguageController`, `Services/Translations.cs`, `Translator.cs`, `ContentTranslations.cs`). New user-facing strings need entries in both languages; the selected language is also passed to Recommendation so Gemini answers in that language.
- **Repo-local caches**: `.dotnet/` and `.nuget/` (from `DOTNET_CLI_HOME`) are gitignored — ignore them when searching.

### Cross-service flows (span multiple projects)
- **AI recommendation**: MealPlanning publishes `RecommendationRequest` → Recommendation.Service (builds context from DietKnowledge *public* APIs, i.e. active `Approved` content only; Gemini or safe local fallback) → publishes `RecommendationResult` → MealPlanning consumes/stores it; user must accept before it becomes a plan.
- **Progress notification**: Tracking → gRPC → Notification.Grpc → RabbitMQ `notification.messages` → NotificationWorker → Resend.
- **Meal reminders**: MealPlanning publishes to `reminder.requests` → NotificationWorker. There is no `MealSchedule` table; schedule = `MealPlanItem.PlannedDate/MealSlot/ReminderAtUtc`. Worker uses durable queues with dead-letter queues and retry settings (`RabbitMq__MaxDeliveryAttempts`, `RabbitMq__RetryDelaySeconds`).
- **Book knowledge**: Admin uploads a PDF to DietKnowledge → chunks + `KnowledgeCandidate`s (Gemini-summarized or placeholders) → Admin approves/rejects → approval creates Approved guideline/food/recipe. Raw chunks/PDFs are Admin-only and must never reach public endpoints or Recommendation context. Do not commit PDFs. Seed data (`book-knowledge-seed.json` as `NeedsReview`, `demo-approved-knowledge-seed.json` gated by `DietKnowledgeSeed__IncludeDemoApprovedContent`) is described in `docs/development/Seed-Data-Strategy.md`.

## Docs to keep in sync
Architecture changes require updating `docs/c4/` (C0 context shows only users/platform/Gemini/Resend; C1 internal), `docs/erd/Conceptual-ERD.md`, `docs/database/Physical-Database.md`, and `docs/development/Gateway-Routes.md` when routes change. Gemini and Resend are optional in dev (`GEMINI_ENABLED=false`, `RESEND_API_KEY` empty/`development-only` → fallbacks).
