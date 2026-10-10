# Project Completion Schedule — Longevity Diet Platform

This file describes what must be completed to turn the current scaffold into a submission-ready PRN232 project.

Read together with:
- `AGENTS.md` for architecture and coding rules.
- `PROJECT_PROGRESS.md` for the latest factual progress log.

## Current Snapshot

Current phase: Milestone 9 Database, Docker, and demo data is complete, including Docker startup and demo-data smoke; Book Knowledge manual source review remains pending.

Estimated product completion: about 92-94%.

What is already in code:
- .NET solution and project structure exist.
- Required services/containers are scaffolded.
- REST service layering exists: API, Application, Infrastructure, Domain.
- Swagger and health endpoint scaffolding exist for public REST APIs.
- Four PostgreSQL DbContext shells exist:
  - `IdentityDb`
  - `DietKnowledgeDb`
  - `MealPlanningDb`
  - `TrackingDb`
- Dockerfiles and Docker Compose skeleton exist.
- YARP API Gateway has finalized routes for public REST services.
- RabbitMQ message contract/configuration skeletons exist.
- Recommendation Service consumes RabbitMQ recommendation requests, builds context from active `Approved` Diet Knowledge data, applies safety guardrails, uses safe fallback recommendations when Gemini is disabled, and publishes recommendation results.
- Notification Service exists as a gRPC service skeleton.
- Notification Worker consumes RabbitMQ reminder/notification messages and sends through Resend or logs email payloads in development fallback mode.
- Architecture docs exist and match the updated microservice diagram.
- Identity Service now supports register/login/profile/admin-check, password hashing, JWT generation, role seeding, demo admin seeding, and the initial Identity EF Core migration.
- Diet Knowledge Service now supports book-knowledge review metadata, JSON seed data, public browsing/search/filter/sort/pagination for active approved content, admin CRUD, activation/deactivation, and Diet Knowledge EF Core migrations.
- Meal Planning Service now supports authenticated meal plan CRUD, meal plan item scheduling, ownership checks, Diet Knowledge validation through REST, RabbitMQ reminder publishing, recommendation request publishing, recommendation result consumption/storage, recommendation accept flow, and the initial Meal Planning EF Core migration.
- Tracking Service now supports authenticated daily tracking, meal completion/not-completion tracking, progress summary calculation, ownership checks, Tracking EF Core migration, and the real Tracking -> Notification Service gRPC flow.
- Notification Service now accepts progress notification gRPC requests and publishes notification messages to RabbitMQ.
- Notification Worker now declares durable reminder/notification queues, dead-letter queues, retry handling, Resend sender abstraction, and local log fallback delivery.
- Web Application now supports the main User and Admin browser flows through the API Gateway.
- Milestone 9 Docker/database demo setup now includes health-checked PostgreSQL/RabbitMQ startup, demo admin/user seed accounts, approved local demo Diet Knowledge seed data, and clean-machine setup documentation.

What is not yet product-ready:
- The team still needs to verify book-derived seed items against a legally obtained source copy before marking them `Approved`.
- Search/filter/sort/pagination are implemented for Diet Knowledge and Meal Planning; Tracking supports date-filtered paged history.
- RabbitMQ publish/consume behavior exists for Meal Planning reminders/recommendations, Recommendation Service processing/results, Notification Service notifications, and Notification Worker delivery.
- Gemini integration has a configurable HTTP client abstraction with safe disabled-mode fallback; real Gemini and Resend delivery need configured API keys for production/demo external calls.
- Automated tests and the final submission/demo package are not complete.

## Definition Of 100% Complete

The project is submission-ready only when all of these are true:

- Solution builds with `0` errors.
- Docker Compose starts all required services locally.
- Each public REST API has Swagger and health endpoint.
- Identity supports register, login, profile, JWT issuing, password hashing, User/Admin roles.
- Diet Knowledge supports admin CRUD and user browse/search/filter/sort/pagination.
- Meal Planning supports create/update/delete meal plans, meal items, scheduling, ownership checks, and reminder publishing.
- Tracking supports meal completion, daily tracking, progress summaries, ownership checks, and gRPC notification trigger.
- Recommendation flow works through RabbitMQ request/result messages and follows AI safety scope.
- Notification flow works through gRPC, RabbitMQ, Notification Worker, and Resend placeholder or configured email delivery.
- PostgreSQL database-per-service rule is preserved.
- EF Core migrations exist for all four persistent services.
- Web Application supports the main User and Admin flows through the API Gateway.
- Architecture docs, ERD, physical database docs, README, and progress tracker are updated.
- Tests cover the implemented business slices.
- A demo script or submission checklist exists for presenting the project to the teacher.

## Recommended Schedule

Assumption: one developer working in focused slices. If the deadline is shorter, follow the "Minimum Submission Path" section first.

### Milestone 0 — Baseline And Shared Foundation

Status: Completed on 2026-10-08.

Target: T+0.5 to T+1 day.

Code/work to do:
- [x] Add shared API response conventions where useful.
- [x] Add global exception handling per public API.
- [x] Add validation approach for request DTOs.
- [x] Confirm package versions and central package management.
- [x] Decide seed data strategy for demo accounts and approved diet content.
- [x] Keep `AGENTS.md`, `PROJECT_PROGRESS.md`, and docs synchronized.

Verification:
- `dotnet build LongevityDietPlatform.sln`
- `docker compose config`

Done when:
- All services still build.
- Future endpoints can follow the same response/error/validation pattern.

### Milestone 1 — Identity Service

Status: Completed on 2026-10-08.

Target: T+1 to T+2.5 days.

Code/work to do:
- [x] Implement `User` and `Role` persistence.
- [x] Add register endpoint.
- [x] Add login endpoint.
- [x] Add password hashing.
- [x] Add JWT token generation.
- [x] Add profile endpoint.
- [x] Add role seed data: `User`, `Admin`.
- [x] Add admin demo account seed.
- [x] Add authorization policies and role checks.
- [x] Add DTOs and validation.
- [x] Add Identity EF Core migration.

Verification:
- [x] Register/login through API.
- [x] Login returns a valid JWT.
- [x] Protected profile endpoint rejects missing/invalid token.
- [x] Admin-only test endpoint rejects normal users.

Done when:
- The rest of the system can rely on JWT Bearer authentication.

### Milestone 2 — Diet Knowledge Service

Status: Completed on 2026-10-08.

Target: T+2.5 to T+4 days.

Code/work to do:
- [x] Implement DTOs for DietGuideline, Food, Recipe, RecipeIngredient.
- [x] Implement admin CRUD endpoints.
- [x] Implement activation/deactivation.
- [x] Implement list endpoints with pagination.
- [x] Implement search/filter/sort for foods and recipes.
- [x] Implement read-only user endpoints.
- [x] Add managed seed data from the allowed Longevity Diet domain.
- [x] Add Diet Knowledge EF Core migration.
- [x] Add authorization: Admin mutations only; public/user reads allowed as required.

Verification:
- [x] Admin can create/update/deactivate content.
- [x] User can search/filter recipes and foods.
- [x] Pagination returns stable metadata.
- [x] No EF entities are exposed directly.

Done when:
- Meal Planning and Recommendation can depend on approved knowledge content.

### Milestone 2.5 — Book Knowledge Integration

Status: Technical foundation completed on 2026-10-09; source-copy verification is still a team responsibility.

Target: T+4 to T+4.5 days.

Code/work to do:
- [ ] Confirm the team has a legitimately obtained copy of *The Longevity Diet* for project research.
- [ ] Approve the scoped chapter/section list used for project knowledge.
- [x] Store source and review metadata for DietGuideline, Food, and Recipe.
- [x] Keep seeded summaries in a version-controlled JSON file.
- [x] Import JSON seed data through EF Core startup seeding.
- [x] Keep seed entries `NeedsReview` until exact source locations are verified.
- [x] Let admins create/update/deactivate managed knowledge.
- [x] Restrict public reads to active `Approved` knowledge.
- [x] Require source metadata and reviewer when admin marks content `Approved`.
- [x] Build Recommendation context only from active `Approved` knowledge during Milestone 6.

Verification:
- [x] `dotnet build LongevityDietPlatform.sln`
- [x] Diet Knowledge migration added for provenance/review metadata.
- [ ] Team review confirms source chapter/page/reference for each seed item.
- [x] Recommendation smoke test proves the context path uses public active `Approved` Diet Knowledge reads.

Done when:
- The app has technical support for book-derived knowledge, and the team has verified/approved the dataset it wants to expose in demos.

### Milestone 3 — Meal Planning Service

Status: Completed on 2026-10-09.

Target: T+4 to T+6 days.

Code/work to do:
- [x] Implement MealPlan and MealPlanItem DTOs.
- [x] Add MealSchedule if required by final workflow.
- [x] Implement create/update/delete meal plan endpoints.
- [x] Implement add/update/remove meal plan items.
- [x] Implement ownership checks using user id from JWT.
- [x] Implement validation for planned dates and meal slots.
- [x] Add explicit HTTP client/API call to Diet Knowledge Service when validating food/recipe ids.
- [x] Publish reminder messages to RabbitMQ when meals are scheduled.
- [x] Publish `RecommendationRequest` messages.
- [x] Consume/store `RecommendationResult` data in a way that supports user review.
- [x] Add accept/edit recommendation flow that saves final accepted plan.
- [x] Add Meal Planning EF Core migration.

Implementation note: a separate `MealSchedule` table was not needed for this workflow. Schedule information is stored on `MealPlanItem` with `PlannedDate`, `MealSlot`, and `ReminderAtUtc`.

Verification:
- [x] User can manage only their own meal plans.
- [x] Meal plan creation rejects invalid food/recipe references.
- [x] Reminder message is published for scheduled meals.
- [x] Recommendation request message is published.
- [x] Recommendation result message is consumed and stored as `Completed`.
- [x] Accepted recommendation saves the final meal plan.

Done when:
- Main meal planning workflow works without direct database access to other services.

### Milestone 4 — Tracking Service And gRPC Notification

Status: Completed on 2026-10-09.

Target: T+6 to T+7.5 days.

Code/work to do:
- [x] Implement DailyTracking endpoints.
- [x] Implement MealTracking endpoints for completed/not completed meals.
- [x] Implement ProgressSummary calculation.
- [x] Add ownership checks using JWT.
- [x] Call Notification Service through gRPC when progress notifications are required.
- [x] Add Tracking EF Core migration.

Verification:
- [x] User can mark meals completed/not completed.
- [x] Progress summary updates correctly.
- [x] Tracking Service sends a real gRPC request to Notification Service.
- [x] Notification Service publishes the accepted progress notification to RabbitMQ.
- [x] Tracking data remains in `TrackingDb` only.

Done when:
- The required gRPC flow is part of a real business use case.

### Milestone 5 — RabbitMQ And Notification Delivery

Target: T+7.5 to T+9 days.

Status: Completed on 2026-10-09.

Code/work to do:
- [x] Implement durable RabbitMQ exchange/queue declarations.
- [x] Implement reminder consumer in Notification Worker.
- [x] Implement notification consumer in Notification Worker.
- [x] Implement retry/logging strategy.
- [x] Add dead-letter queue if time allows.
- [x] Implement Resend email sender abstraction.
- [x] Keep a development fallback that logs email payloads when Resend is not configured.

Verification:
- [x] Scheduling a meal eventually reaches Notification Worker.
- [x] Tracking progress notification eventually reaches Notification Worker.
- [x] Worker builds and contains logged fallback delivery when Resend is not configured.
- [x] RabbitMQ Management UI shows expected queues/exchanges.
- [x] `dotnet build LongevityDietPlatform.sln`
- [x] `dotnet test LongevityDietPlatform.sln`
- [x] `docker compose config`

Done when:
- Asynchronous messaging is demonstrable end to end.

### Milestone 6 — Recommendation Service And Gemini Placeholder/Integration

Target: T+9 to T+10.5 days.

Status: Completed on 2026-10-09.

Code/work to do:
- [x] Implement Recommendation Service consumer for `RecommendationRequest`.
- [x] Build AI prompt/context only from approved knowledge and user preferences.
- [x] Add safety guardrails: no diagnosis, treatment advice, disease prediction, or lifespan prediction.
- [x] Add Gemini client abstraction.
- [x] Keep fallback recommendation behavior when Gemini is disabled.
- [x] Publish `RecommendationResult` back to RabbitMQ.
- [x] Add Meal Planning handling for results.

Verification:
- [x] Recommendation request produces a result message.
- [x] Gemini-disabled mode still returns a safe placeholder recommendation.
- [x] Gemini-enabled mode is isolated behind configuration.
- [x] Output stays within project scope.
- [x] `dotnet build LongevityDietPlatform.sln`
- [x] `dotnet test LongevityDietPlatform.sln`
- [x] `docker compose config`

Done when:
- AI-assisted recommendation flow is demonstrable without violating domain rules.

### Milestone 7 — API Gateway Integration

Target: T+10.5 to T+11 days.

Status: Completed on 2026-10-09.

Code/work to do:
- [x] Finalize YARP routes for all public REST services.
- [x] Ensure internal services are not exposed through gateway unless required.
- [x] Verify JWT forwarding works through gateway.
- [x] Add gateway route docs to README.

Verification:
- [x] Web/API clients can call Identity, Diet Knowledge, Meal Planning, and Tracking through the gateway.
- [x] Recommendation Service and Notification Service remain internal.
- [x] `dotnet build LongevityDietPlatform.sln`
- [x] `dotnet test LongevityDietPlatform.sln`
- [x] `docker compose config`

Done when:
- The Web Application can use one backend entry point.

### Milestone 8 — Web Application

Target: T+11 to T+14 days.

Status: Completed on 2026-10-10.

Code/work to do:
- [x] Implement login/register pages.
- [x] Store/use JWT securely enough for the assignment demo.
- [x] Implement user profile view.
- [x] Implement diet guideline browsing.
- [x] Implement food and recipe search/filter screens.
- [x] Implement admin screens for guidelines, foods, recipes.
- [x] Implement meal plan creation/editing screens.
- [x] Implement tracking and progress screens.
- [x] Implement AI recommendation request/review/accept screens.
- [x] Add user-friendly validation messages.

Verification:
- [x] User demo path works from browser.
- [x] Admin demo path works from browser.
- [x] Web calls backend through API Gateway.

Done when:
- The teacher can see the product behavior without manually using Swagger for every workflow.

### Milestone 9 — Database, Docker, And Demo Data

Target: T+14 to T+15 days.

Status: Completed on 2026-10-10.

Code/work to do:
- [x] Add migrations for all four databases.
- [x] Confirm Docker Compose database connection strings.
- [x] Add database initialization instructions.
- [x] Add demo seed data:
  - admin account
  - user account
  - diet guidelines
  - foods
  - recipes
- [x] Add environment variable documentation.

Verification:
- [x] Fresh checkout can run migrations and start.
- [x] Demo data appears after setup.
- [x] Compose starts without manual service hacks.
- [x] `dotnet build LongevityDietPlatform.sln`
- [x] `dotnet test LongevityDietPlatform.sln`
- [x] `docker compose config --quiet`
- [x] `docker compose up -d --build`

Done when:
- The project can be run from a clean machine using documented steps.

### Milestone 10 — Tests

Target: T+15 to T+16.5 days.

Code/work to do:
- Add unit tests for application services.
- Add repository/integration tests where practical.
- Add API tests for important endpoints.
- Add tests for authorization boundaries.
- Add tests for search/filter/pagination behavior.
- Add tests for progress summary calculation.
- Add tests or smoke checks for messaging publishers.

Verification:
- `dotnet test` passes.

Done when:
- Core behaviors have enough coverage to defend the implementation.

### Milestone 11 — Documentation And Submission Package

Target: T+16.5 to T+17.5 days.

Code/work to do:
- Update C4 diagrams if implementation changed.
- Update ERD and physical database docs.
- Update README run/demo instructions.
- Update `PROJECT_PROGRESS.md`.
- Add a demo script with exact presentation steps.
- Add a final submission checklist.

Verification:
- Another person can follow README and demo script.
- Docs match actual architecture.

Done when:
- The repository is understandable to the teacher without extra explanation.

### Milestone 12 — Final QA

Target: T+17.5 to T+18 days.

Code/work to do:
- Run full restore/build/test.
- Run Docker Compose smoke test.
- Verify Swagger endpoints.
- Verify Web demo flow.
- Verify no service accesses another service database.
- Verify no forbidden medical/diagnosis feature exists.
- Verify branch is pushed and PR is current.

Verification:
- `dotnet restore LongevityDietPlatform.sln`
- `dotnet build LongevityDietPlatform.sln`
- `dotnet test`
- `docker compose config`
- `docker compose up --build`

Done when:
- The project is ready to submit.

## Minimum Submission Path

If time is short, finish in this order:

1. Identity register/login/JWT/Admin role.
2. Diet Knowledge admin CRUD and user search/filter/pagination.
3. Meal Planning CRUD with ownership and reminder publish.
4. Tracking meal completion and progress summary.
5. Tracking -> Notification gRPC call.
6. Notification Worker consumes messages and logs/sends email.
7. Recommendation RabbitMQ request/result with safe placeholder Gemini behavior.
8. EF migrations and seed data.
9. Web UI for the main demo flows.
10. Docs, README, C4, ERD, physical DB, final demo script.

This minimum path is enough to demonstrate the required PRN232 architecture and user-facing product flow.

## Assignment Requirement Coverage

| Requirement | Current status | Remaining work |
| --- | --- | --- |
| Microservices architecture | Scaffolded | Implement real business APIs and flows |
| ASP.NET Core REST APIs | Identity, Diet Knowledge, Meal Planning, and Tracking implemented | Keep endpoint behavior aligned as remaining services are added |
| Layered architecture | Project structure exists | Keep controllers out of DbContext and business logic |
| JWT auth/authorization | Identity implemented; Diet Knowledge admin mutations protected; Meal Planning and Tracking ownership checks implemented | Extend to Web flows and future APIs |
| Search/filter/sort/pagination | Implemented for Diet Knowledge, Meal Planning, and Tracking history | Add more list behavior only where useful |
| gRPC internal flow | Tracking -> Notification implemented in a real progress use case | Add tests/smoke documentation |
| RabbitMQ async messaging | Meal Planning publishes reminders/recommendation requests and consumes recommendation results; Recommendation Service consumes requests and publishes results; Notification Service publishes notification messages; Notification Worker consumes reminder/notification messages | Add tests and final demo script |
| .NET Worker Service | Notification Worker implemented and Docker-smoke-tested for reminder/notification delivery | Add tests |
| PostgreSQL database-per-service | Identity, Diet Knowledge, Meal Planning, and Tracking migrations exist | Keep service ownership boundaries intact |
| Docker Compose | Full stack Docker startup, health checks, and demo seed smoke verified through Milestone 9 | Add final demo script/checklist |
| C4 docs | Existing | Keep synchronized with implementation |
| Web Application | Main User/Admin browser flows implemented through the API Gateway | Add final demo script and automated UI/API smoke tests if time allows |
| External providers | Resend sender abstraction with log fallback exists; Gemini client abstraction with disabled-mode fallback exists | Configure real Resend/Gemini keys only for demo/production if needed |
| Tests | Not present | Add unit/integration/API tests |

## Per-Service Checklist

### Identity Service

- [x] Register API.
- [x] Login API.
- [x] JWT generation.
- [x] Password hashing.
- [x] Profile API.
- [x] Role seed.
- [x] Admin/User authorization policy.
- [x] Migration and admin/user seed data.
- [ ] Tests.

### Diet Knowledge Service

- [x] DietGuideline CRUD.
- [x] Food CRUD.
- [x] Recipe CRUD.
- [x] RecipeIngredient handling.
- [x] Activate/deactivate content.
- [x] Search/filter/sort/pagination.
- [x] Admin-only mutations.
- [x] User read endpoints.
- [x] Migration and JSON seed content.
- [x] Approved local demo seed content for public demo flows.
- [x] Source/review metadata for book knowledge.
- [ ] Team source verification and approval of seed content.
- [ ] Tests.

### Meal Planning Service

- [x] MealPlan CRUD.
- [x] MealPlanItem CRUD.
- [x] MealSchedule support if final workflow needs it.
- [x] Ownership checks.
- [x] Validate food/recipe IDs through service API, not database access.
- [x] Reminder publish.
- [x] Recommendation request publish.
- [x] Recommendation result consume.
- [x] Accept/edit recommendation.
- [x] Migration.
- [ ] Tests.

Note: schedule support is represented on `MealPlanItem` rather than a separate table.

### Tracking Service

- [x] DailyTracking CRUD/use cases.
- [x] MealTracking completion use case.
- [x] ProgressSummary calculation.
- [x] Ownership checks.
- [x] gRPC call to Notification Service.
- [x] Migration.
- [ ] Tests.

### Recommendation Service

- [x] RabbitMQ consumer.
- [x] Gemini client abstraction.
- [x] Safe prompt/context builder.
- [x] Disabled-mode fallback.
- [x] Result publisher.
- [x] Logging/error handling.
- [x] Tests or smoke checks.

### Notification Service

- [x] gRPC contract finalization.
- [x] Notification message preparation.
- [x] RabbitMQ publisher.
- [ ] Error handling/logging.
- [ ] Tests or smoke checks.

### Notification Worker

- [x] Reminder consumer.
- [x] Notification consumer.
- [x] Resend email sender abstraction.
- [x] Development logging fallback.
- [x] Retry/error handling.
- [x] Dead-letter queues.
- [x] Docker smoke checks.
- [ ] Automated tests.

### API Gateway

- [x] Public service routes finalized.
- [x] JWT forwarding verified.
- [x] Internal services kept private.
- [x] Gateway smoke test.

### Web Application

- [x] Login/register.
- [x] Profile.
- [x] User diet knowledge browsing.
- [x] Admin content management.
- [x] Meal planning.
- [x] Recommendation request/review/accept.
- [x] Tracking/progress.
- [x] Friendly error/validation handling.

### Documentation And Submission

- [x] README run guide.
- [ ] C0/C1 docs updated.
- [ ] ERD updated.
- [ ] Physical database docs updated.
- [ ] Demo script.
- [ ] Final checklist.
- [ ] `PROJECT_PROGRESS.md` updated after every milestone.

## Rule For Future Work

After finishing any item in this schedule:
- Update `PROJECT_PROGRESS.md`.
- Update this schedule if priorities or scope change.
- Build/test the affected slice.
- Commit and push only after the repository is in a clear state.
