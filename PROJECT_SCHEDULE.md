# Project Completion Schedule — Longevity Diet Platform

This file describes what must be completed to turn the current scaffold into a submission-ready PRN232 project.

Read together with:
- `AGENTS.md` for architecture and coding rules.
- `PROJECT_PROGRESS.md` for the latest factual progress log.

## Current Snapshot

Current phase: Phase 1 scaffold is complete.

Estimated product completion: about 20-25%.

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

What is not yet product-ready:
- Real authentication is not implemented.
- CRUD APIs are not implemented.
- Search/filter/sort/pagination are not implemented.
- JWT authorization and role policies are not enforced.
- EF Core migrations are not created.
- RabbitMQ publish/consume behavior is mostly placeholder-level.
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

Target: T+0.5 to T+1 day.

Code/work to do:
- Add shared API response conventions where useful.
- Add global exception handling per public API.
- Add validation approach for request DTOs.
- Confirm package versions and central package management.
- Decide seed data strategy for demo accounts and approved diet content.
- Keep `AGENTS.md`, `PROJECT_PROGRESS.md`, and docs synchronized.

Verification:
- `dotnet build LongevityDietPlatform.sln`
- `docker compose config`

Done when:
- All services still build.
- Future endpoints can follow the same response/error/validation pattern.

### Milestone 1 — Identity Service

Target: T+1 to T+2.5 days.

Code/work to do:
- Implement `User` and `Role` persistence.
- Add register endpoint.
- Add login endpoint.
- Add password hashing.
- Add JWT token generation.
- Add profile endpoint.
- Add role seed data: `User`, `Admin`.
- Add admin demo account seed.
- Add authorization policies and role checks.
- Add DTOs and validation.
- Add Identity EF Core migration.

Verification:
- Register/login through Swagger.
- Login returns a valid JWT.
- Protected profile endpoint rejects missing/invalid token.
- Admin-only test endpoint or protected operation rejects normal users.

Done when:
- The rest of the system can rely on JWT Bearer authentication.

### Milestone 2 — Diet Knowledge Service

Target: T+2.5 to T+4 days.

Code/work to do:
- Implement DTOs for DietGuideline, Food, Recipe, RecipeIngredient.
- Implement admin CRUD endpoints.
- Implement activation/deactivation.
- Implement list endpoints with pagination.
- Implement search/filter/sort for foods and recipes.
- Implement read-only user endpoints.
- Add approved seed data from the allowed Longevity Diet domain.
- Add Diet Knowledge EF Core migration.
- Add authorization: Admin mutations only; public/user reads allowed as required.

Verification:
- Admin can create/update/deactivate content.
- User can search/filter recipes and foods.
- Pagination returns stable metadata.
- No EF entities are exposed directly.

Done when:
- Meal Planning and Recommendation can depend on approved knowledge content.

### Milestone 3 — Meal Planning Service

Target: T+4 to T+6 days.

Code/work to do:
- Implement MealPlan and MealPlanItem DTOs.
- Add MealSchedule if required by final workflow.
- Implement create/update/delete meal plan endpoints.
- Implement add/update/remove meal plan items.
- Implement ownership checks using user id from JWT.
- Implement validation for planned dates and meal slots.
- Add explicit HTTP client/API call to Diet Knowledge Service when validating food/recipe ids.
- Publish reminder messages to RabbitMQ when meals are scheduled.
- Publish `RecommendationRequest` messages.
- Consume/store `RecommendationResult` data in a way that supports user review.
- Add accept/edit recommendation flow that saves final accepted plan.
- Add Meal Planning EF Core migration.

Verification:
- User can manage only their own meal plans.
- Meal plan creation rejects invalid food/recipe references.
- Reminder message is published for scheduled meals.
- Recommendation request message is published.

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
| ASP.NET Core REST APIs | Skeletons exist | Add DTOs, controllers, validation, status codes |
| Layered architecture | Project structure exists | Keep controllers out of DbContext and business logic |
| JWT auth/authorization | Not implemented | Register/login/JWT/roles/ownership |
| Search/filter/sort/pagination | Not implemented | Diet Knowledge lists first, then other lists where useful |
| gRPC internal flow | Skeleton exists | Connect Tracking workflow to Notification Service |
| RabbitMQ async messaging | Contracts/config exist | Implement durable publish/consume flows |
| .NET Worker Service | Skeleton exists | Implement notification/reminder processing |
| PostgreSQL database-per-service | DbContext shells exist | Migrations, data model completion, seed data |
| Docker Compose | Skeleton valid | Full startup verification with migrations and dependencies |
| C4 docs | Existing | Keep synchronized with implementation |
| Web Application | MVC shell exists | Build real User/Admin screens |
| External providers | Placeholders exist | Resend/Gemini abstractions and configured behavior |
| Tests | Not present | Add unit/integration/API tests |

## Per-Service Checklist

### Identity Service

- [ ] Register API.
- [ ] Login API.
- [ ] JWT generation.
- [ ] Password hashing.
- [ ] Profile API.
- [ ] Role seed.
- [ ] Admin/User authorization policy.
- [ ] Migration and seed data.
- [ ] Tests.

### Diet Knowledge Service

- [ ] DietGuideline CRUD.
- [ ] Food CRUD.
- [ ] Recipe CRUD.
- [ ] RecipeIngredient handling.
- [ ] Activate/deactivate content.
- [ ] Search/filter/sort/pagination.
- [ ] Admin-only mutations.
- [ ] User read endpoints.
- [ ] Migration and approved seed content.
- [ ] Tests.

### Meal Planning Service

- [ ] MealPlan CRUD.
- [ ] MealPlanItem CRUD.
- [ ] MealSchedule support if final workflow needs it.
- [ ] Ownership checks.
- [ ] Validate food/recipe IDs through service API, not database access.
- [ ] Reminder publish.
- [ ] Recommendation request publish.
- [ ] Recommendation result consume.
- [ ] Accept/edit recommendation.
- [ ] Migration.
- [ ] Tests.

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
