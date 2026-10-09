# Project Completion Schedule — Longevity Diet Platform

This file describes what must be completed to turn the current scaffold into a submission-ready PRN232 project.

Read together with:
- `AGENTS.md` for architecture and coding rules.
- `PROJECT_PROGRESS.md` for the latest factual progress log.

## Current Snapshot

Current phase: Milestone 3 Meal Planning Service is complete; Book Knowledge manual source review remains pending.

Estimated product completion: about 52-56%.

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
- YARP API Gateway skeleton exists.
- RabbitMQ message contract/configuration skeletons exist.
- Recommendation Service exists as a RabbitMQ worker skeleton.
- Notification Service exists as a gRPC service skeleton.
- Notification Worker exists as a worker skeleton.
- Architecture docs exist and match the updated microservice diagram.
- Identity Service now supports register/login/profile/admin-check, password hashing, JWT generation, role seeding, demo admin seeding, and the initial Identity EF Core migration.
- Diet Knowledge Service now supports book-knowledge review metadata, JSON seed data, public browsing/search/filter/sort/pagination for active approved content, admin CRUD, activation/deactivation, and Diet Knowledge EF Core migrations.
- Meal Planning Service now supports authenticated meal plan CRUD, meal plan item scheduling, ownership checks, Diet Knowledge validation through REST, RabbitMQ reminder publishing, recommendation request publishing, recommendation result consumption/storage, recommendation accept flow, and the initial Meal Planning EF Core migration.

What is not yet product-ready:
- CRUD APIs are not implemented yet for Tracking.
- The team still needs to verify book-derived seed items against a legally obtained source copy before marking them `Approved`.
- Search/filter/sort/pagination are implemented for Diet Knowledge and Meal Planning; Tracking still needs list/history behavior where useful.
- JWT ownership checks still need to be applied to Tracking.
- EF Core migration is still needed for Tracking.
- RabbitMQ publish/consume behavior exists for the Meal Planning side of reminders and recommendations; Recommendation Service, Notification Service, and Notification Worker still need their full business consumers/publishers.
- gRPC flow exists as a skeleton but is not connected to real tracking workflows.
- Gemini and Resend integrations are placeholders.
- Web Application is still mostly the default MVC shell.
- Tests and end-to-end Docker smoke tests are not complete.

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
- [ ] Build Recommendation context only from active `Approved` knowledge during Milestone 6.

Verification:
- [x] `dotnet build LongevityDietPlatform.sln`
- [x] Diet Knowledge migration added for provenance/review metadata.
- [ ] Team review confirms source chapter/page/reference for each seed item.
- [ ] Future Recommendation smoke test proves AI context excludes unapproved content.

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

Target: T+6 to T+7.5 days.

Code/work to do:
- Implement DailyTracking endpoints.
- Implement MealTracking endpoints for completed/not completed meals.
- Implement ProgressSummary calculation.
- Add ownership checks using JWT.
- Call Notification Service through gRPC when progress notifications are required.
- Add Tracking EF Core migration.

Verification:
- User can mark meals completed/not completed.
- Progress summary updates correctly.
- Tracking Service sends a real gRPC request to Notification Service.
- Tracking data remains in `TrackingDb` only.

Done when:
- The required gRPC flow is part of a real business use case.

### Milestone 5 — RabbitMQ And Notification Delivery

Target: T+7.5 to T+9 days.

Code/work to do:
- Implement durable RabbitMQ exchange/queue declarations.
- Implement reminder consumer in Notification Worker.
- Implement notification consumer in Notification Worker.
- Implement retry/logging strategy.
- Add dead-letter queue if time allows.
- Implement Resend email sender abstraction.
- Keep a development fallback that logs email payloads when Resend is not configured.

Verification:
- Scheduling a meal eventually reaches Notification Worker.
- Tracking progress notification eventually reaches Notification Worker.
- Worker logs or sends expected email payload.
- RabbitMQ Management UI shows expected queues/exchanges.

Done when:
- Asynchronous messaging is demonstrable end to end.

### Milestone 6 — Recommendation Service And Gemini Placeholder/Integration

Target: T+9 to T+10.5 days.

Code/work to do:
- Implement Recommendation Service consumer for `RecommendationRequest`.
- Build AI prompt/context only from approved knowledge and user preferences.
- Add safety guardrails: no diagnosis, treatment advice, disease prediction, or lifespan prediction.
- Add Gemini client abstraction.
- Keep fallback recommendation behavior when Gemini is disabled.
- Publish `RecommendationResult` back to RabbitMQ.
- Add Meal Planning handling for results.

Verification:
- Recommendation request produces a result message.
- Gemini-disabled mode still returns a safe placeholder recommendation.
- Gemini-enabled mode is isolated behind configuration.
- Output stays within project scope.

Done when:
- AI-assisted recommendation flow is demonstrable without violating domain rules.

### Milestone 7 — API Gateway Integration

Target: T+10.5 to T+11 days.

Code/work to do:
- Finalize YARP routes for all public REST services.
- Ensure internal services are not exposed through gateway unless required.
- Verify JWT forwarding works through gateway.
- Add gateway route docs to README.

Verification:
- Web/API clients can call Identity, Diet Knowledge, Meal Planning, and Tracking through the gateway.
- Recommendation Service and Notification Service remain internal.

Done when:
- The Web Application can use one backend entry point.

### Milestone 8 — Web Application

Target: T+11 to T+14 days.

Code/work to do:
- Implement login/register pages.
- Store/use JWT securely enough for the assignment demo.
- Implement user profile view.
- Implement diet guideline browsing.
- Implement food and recipe search/filter screens.
- Implement admin screens for guidelines, foods, recipes.
- Implement meal plan creation/editing screens.
- Implement tracking and progress screens.
- Implement AI recommendation request/review/accept screens.
- Add user-friendly validation messages.

Verification:
- User demo path works from browser.
- Admin demo path works from browser.
- Web calls backend through API Gateway.

Done when:
- The teacher can see the product behavior without manually using Swagger for every workflow.

### Milestone 9 — Database, Docker, And Demo Data

Target: T+14 to T+15 days.

Code/work to do:
- Add migrations for all four databases.
- Confirm Docker Compose database connection strings.
- Add database initialization instructions.
- Add demo seed data:
  - admin account
  - user account
  - diet guidelines
  - foods
  - recipes
- Add environment variable documentation.

Verification:
- Fresh checkout can run migrations and start.
- Demo data appears after setup.
- Compose starts without manual service hacks.

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
| ASP.NET Core REST APIs | Identity, Diet Knowledge, and Meal Planning implemented; Tracking API scaffolded | Add Tracking business DTOs, controllers, validation rules, status codes |
| Layered architecture | Project structure exists | Keep controllers out of DbContext and business logic |
| JWT auth/authorization | Identity implemented; Diet Knowledge admin mutations protected; Meal Planning ownership checks implemented | Add ownership checks to Tracking |
| Search/filter/sort/pagination | Implemented for Diet Knowledge and Meal Planning | Add history/list behavior to Tracking where useful |
| gRPC internal flow | Skeleton exists | Connect Tracking workflow to Notification Service |
| RabbitMQ async messaging | Contracts/config exist; Meal Planning publishes reminders and recommendation requests and consumes recommendation results | Implement full Recommendation, Notification, and Worker flows |
| .NET Worker Service | Skeleton exists | Implement notification/reminder processing |
| PostgreSQL database-per-service | Identity, Diet Knowledge, and Meal Planning migrations exist; Tracking DbContext shell exists | Tracking migration and data model completion |
| Docker Compose | Skeleton valid; Identity and Diet Knowledge startup smoke tested | Full startup verification with all services and dependencies |
| C4 docs | Existing | Keep synchronized with implementation |
| Web Application | MVC shell exists | Build real User/Admin screens |
| External providers | Placeholders exist | Resend/Gemini abstractions and configured behavior |
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
- [x] Migration and seed data.
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

- [ ] DailyTracking CRUD/use cases.
- [ ] MealTracking completion use case.
- [ ] ProgressSummary calculation.
- [ ] Ownership checks.
- [ ] gRPC call to Notification Service.
- [ ] Migration.
- [ ] Tests.

### Recommendation Service

- [ ] RabbitMQ consumer.
- [ ] Gemini client abstraction.
- [ ] Safe prompt/context builder.
- [ ] Disabled-mode fallback.
- [ ] Result publisher.
- [ ] Logging/error handling.
- [ ] Tests or smoke checks.

### Notification Service

- [ ] gRPC contract finalization.
- [ ] Notification message preparation.
- [ ] RabbitMQ publisher.
- [ ] Error handling/logging.
- [ ] Tests or smoke checks.

### Notification Worker

- [ ] Reminder consumer.
- [ ] Notification consumer.
- [ ] Resend email sender abstraction.
- [ ] Development logging fallback.
- [ ] Retry/error handling.
- [ ] Tests or smoke checks.

### API Gateway

- [ ] Public service routes finalized.
- [ ] JWT forwarding verified.
- [ ] Internal services kept private.
- [ ] Gateway smoke test.

### Web Application

- [ ] Login/register.
- [ ] Profile.
- [ ] User diet knowledge browsing.
- [ ] Admin content management.
- [ ] Meal planning.
- [ ] Recommendation request/review/accept.
- [ ] Tracking/progress.
- [ ] Friendly error/validation handling.

### Documentation And Submission

- [ ] README run guide.
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
