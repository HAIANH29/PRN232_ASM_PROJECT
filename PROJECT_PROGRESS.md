# Project Progress — Longevity Diet Platform

This file is the shared progress tracker for the project. Every human or AI agent must read it before changing the repository and update it after completing any work.

## Current Status

- Phase: all milestones including the revised database design (five modules + VectorDb) and the 2026-11-11 reliability fixes are implemented; manual real book source review/approval, a visual check of the new Web pages, and final QA remain.
- Branch: `HA/phase-1-microservices-scaffold`.
- Last pushed baseline before the 2026-11-11 change: commit `2afb60e`.
- Verification baseline (2026-11-12): `dotnet build LongevityDietPlatform.sln` succeeds and `dotnet test LongevityDietPlatform.sln` runs 94 tests, all passing (the 4 tests added after the 2026-11-11 baseline of 90 cover the past-date rules in `MealPlanningServiceTests`). The 2026-11-11 changes have not yet been run against Docker/PostgreSQL (see the change log).
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
- Added Notification Worker placeholder for notification/reminder messages and future Brevo delivery.
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
- Implemented Notification Worker reminder and notification consumers, durable RabbitMQ topology declarations, retry handling, dead-letter queues, Brevo sender abstraction, and development log fallback delivery.
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

## Team Contribution Assessment

Assessment dated 2026-11-09 (team of 3). Percentages are the agreed split of overall project effort.

| Member | Share | Responsibility |
|---|---|---|
| Lam Thien Hai Anh | 45% | Architecture and infrastructure (microservices, YARP Gateway, JWT/Identity, RabbitMQ, gRPC, Docker Compose, migrations); hardest flows: Diet Knowledge (PDF ingestion, candidates, pgvector VectorDb), Recommendation (Gemini), Meal Planning and meal reminders; code review and merge. |
| Member 1 | 35% | Tracking + Notification (API, tests, gRPC integration); QA of all end-to-end flows and unit tests; C4/ERD documentation, slides and demo; liaison with the lecturer on class requirements. |
| Member 2 | 20% | Demo/seed content, EN/VI translations, simple Web screens, one simple CRUD, screenshots and demo video. |

Notes:
- Every member's work must be committed on their own branch and merged through a PR, so commits/PRs serve as contribution evidence.
- Percentages reflect the agreed assignment; if actual contribution differs near submission, update this table to match the real commit history.
- Update this table in the Change Log whenever the split changes.

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

### 2026-11-14 — Study guide (Word) for the oral defense

- Added `docs/submission/Longevity-Diet-Study-Guide.docx` (Vietnamese): project overview, architecture, services, databases, business flows, protocols (HTTP/HTTPS, REST, YARP gateway, JWT, gRPC, RabbitMQ, outbox) with `file:line` config locations, 40+ likely lecturer Q&A, a quick-lookup table and an honest limitations table.
- Built on `docs/development/System-Structure.md`; ~40 file:line references re-checked against the code. Found that Web keeps the JWT in an HttpOnly cookie (`ldp_access_token`), not a session as `Authentication-and-Authorization.md` says.
- Verification: the .docx was generated with docx-js; XML parts parse, but it was not rendered visually (no LibreOffice on this machine) and the table of contents fills in when Word updates fields on open. Line numbers drift if code changes.

### 2026-11-14 — Vietnamese demo-video narration script

- Added `docs/submission/Demo-Video-Narration-vi.md`: Vietnamese voice-over per demo section (follows `Demo-Script.md`), English terms bolded with a pronunciation table and TTS/SSML tips.
- Verification: documentation only; re-run the test count ("94") before recording. TODO: record the video and listen to the hard terms (gRPC, RabbitMQ, pgvector).

### 2026-11-14 — Delete meal-idea requests; suggested meals start from the current meal

- Added `DELETE /meal-planning/api/meal-recommendations/{id}` (owner only; removes just the suggestion record, a plan already created from it is kept) plus Delete buttons on the Web "Meal ideas" list and details pages (EN/VI string added).
- Suggested meals on the review page now run in consecutive slots from the current/next meal in local time (e.g. 12:00 on 14/11 -> Lunch 14/11, Dinner 14/11, Breakfast 15/11) instead of one day per meal. Slot end times are mirrored from `MealSlots.EndTimeLocal` in `RecommendationsController`.
- Verification: solution builds; 95 tests pass (1 new delete test in `MealPlanningServiceTests`). NOT verified in the running UI/Docker; the slot scheduling in the Web controller has no unit test.

### 2026-11-12 — Submission package (PDF documents)

- Added `docs/Submission-Package/`: PDFs for Proposal, SRS, Business Rules (BR-01..BR-82, derived from the Application-layer services), ERD & Database, Architecture Diagrams plus a combined pack; the two `.drawio` files (C4 and database) with PNG exports; Mermaid-based use-case, sequence, state, deployment and per-service ERD diagrams; real Web screenshots (User and Admin) taken from the running Docker stack. Documents are in English with `[Team name]` / `[Lecturer name]` placeholders.
- Verification: PDFs rendered with Edge headless and spot-checked page by page; no code changed. Rebuild scripts are in `source/build/`.
- Remaining: fill the placeholders; no screenshot of a book-source detail/candidate review page (no document was uploaded in the demo data).

### 2026-11-12 — Test count re-run (94) and presentation script

- Verification: `dotnet test LongevityDietPlatform.sln` gives 94/94 passing (was 90 on 2026-11-11; the 4 new tests are the past-date meal plan/slot/reminder tests in `MealPlanningServiceTests`). BR-04 and state transitions still have no dedicated tests.
- Presentation: the Canva deck (`DAHalyY5ssU`) test figure is updated from 90 to 94 (slides and speaker notes). Added `docs/submission/Presentation-Script.docx`, a Vietnamese spoken script (about 18 minutes) built from the slides and notes.

### 2026-11-11 — Meal plans cannot be created in the past

- Bug: a plan or meal item could be created for a date or meal time that had already passed (e.g. breakfast of today at lunchtime), and the Notification Worker then emailed that "reminder" at once because its time was already due.
- Fix (Meal Planning Application): new `MealSchedulePolicy` (UTC offset, default +7, config `MealPlanning__UtcOffsetMinutes` / `MEAL_PLANNING_UTC_OFFSET_MINUTES`, set in Compose for Meal Planning and Web) and `MealSlots.EndTimeLocal` (Breakfast 10:00, Lunch 14:00, Snack 17:00, Dinner 21:00 local). `MealPlanningService` now rejects (400 validation) a plan whose start date is before today, and a meal item whose slot has already ended. Editing an existing item only re-checks when its date or slot changes, and updating a plan only re-checks when its start date moves. A reminder whose time is already due is no longer queued, so no immediate mail is sent for the meal being created right now.
- Behaviour change: default reminder times (07:00, 11:00, 15:00, 17:00) are now local time; they used to be UTC, which made the breakfast reminder arrive at 14:00 in UTC+7.
- Web: the "accept suggestions" form starts from tomorrow once it is past 10:00 local, so its first row (breakfast) is not already over.
- Verification: solution builds; 94 tests pass (4 new in `MealPlanningServiceTests` with a fixed clock; existing tests now use a fixed clock too). NOT verified: `min` attributes on the Web date inputs were not added.
- Verification update (same day): rebuilt Docker and tested for real. A plan starting yesterday and a same-day past breakfast return 400, dinner today is accepted, tomorrow's breakfast reminder is queued for 00:00 UTC, and no immediate reminder mail is sent. Known gap: the validation message is English inside the Vietnamese UI, and the "saved, will remind you at ..." confirmation mail is still sent.
- Docs (same day): new `docs/development/System-Structure.md` (detailed Vietnamese guide: simple explanations, entities, services, communication methods, `file:line` of where each is configured, config flow `.env` -> `docker-compose.yml` -> code). `C1-Internal-Architecture.md` and `AsignmentDiagram.drawio` aligned with code (Meal Planning and Recommendation call Diet Knowledge over HTTP, Diet Knowledge to VectorDb, "Consume" labels; drawio layout not looked at). Found doc/code mismatches still to fix: `Messaging-Flow.md` says Tracking publishes `notification.messages` (only Notification.Grpc does) and `Service-Port-Map.md` says 5105 is gRPC (it is the health port 8081; gRPC is on 8080).
- Local setup: `GEMINI_API_KEY` in the gitignored `.env` was replaced by a key from a new AI Studio project; the free-tier limits are the same, only the counter is fresh.

### 2026-11-11 — Reliability fixes: stale reminders, outbox, planned-meal progress, JWT key, repo cleanup

- Stale reminders: `ReminderRequestedMessage` carries `MealPlanItemId`. Before sending, the Notification Worker asks Meal Planning (`GET /internal/reminders/{itemId}?scheduledForUtc=`, header `X-Internal-Key`, config `InternalApi__Key`) whether the item still exists with that reminder time; deleted or rescheduled meals are dropped. The check fails open if Meal Planning is unreachable. Updating an item now queues a new reminder only when its reminder time changed (otherwise the waiting one stays valid). The gateway returns 404 for any `/internal/` path.
- Outbox: Meal Planning no longer publishes to RabbitMQ inside the request. Reminder and recommendation messages are saved to the new `OutboxMessages` table in the same `SaveChanges` as the plan/item/request (`IMealPlanOutbox`, migration `AddOutboxMessages`); `OutboxDispatcherWorker` publishes them in order, retries with back-off and never drops a message, and deletes processed rows after one day. Delivery is at least once.
- Progress: `SetMealCompletionRequest.PlannedMealCount` (optional, 0-50) is stored on `DailyTrackings.PlannedMealCount` (migration `AddDailyTrackingPlannedMealCount`). A day's planned meals are the larger of that value and the tracked meals, so the "day completed" notification fires only when every planned meal is eaten. The Web check-in page sends the number of meals planned for the day. Clients that do not send it behave as before.
- JWT: `JwtSigningKeyResolver` makes every public API fail at startup when `Jwt:SigningKey` is missing, shorter than 32 bytes, or the public development key, unless the environment is Development or `Jwt:AllowDevelopmentKey=true`. Docker Compose sets `JWT_ALLOW_DEVELOPMENT_KEY` (default `true`, local demo only); `.env.example` documents both it and `INTERNAL_API_KEY`.
- Repo/docs: restored `docs/book-knowledge/Book-Knowledge-Integration.md` (README links to it); `.gitignore` now ignores `docs/book-knowledge/*.txt`, `*.pdf` (the full book text must not be committed); `Physical-Database.md` (VectorDb port, OutboxMessages, PlannedMealCount, migrations), `Conceptual-ERD.md`, `Gateway-Routes.md`, `Database-Docker-Demo-Setup.md` updated. `AsignmentDiagram.drawio` now lives in `docs/c4/` (the move is still uncommitted).
- Verification: solution builds with 0 warnings; 90 tests pass (9 new in `ReliabilityTests`: reminder kept/requeued/stale checks, outbox publish and broker-down back-off, planned-meal progress and notification, planned count validation, JWT key rules). `dotnet ef` generated both migrations. NOT verified: the two migrations against PostgreSQL, the dispatcher against a real RabbitMQ, the worker-to-Meal-Planning check over Docker networking, and the Web check-in page in a browser.
- Docs/code consistency pass (same day): `AGENTS.md` (owned entities, outbox, worker-to-Meal-Planning check, regimens/profile/diet tracking in scope, VectorDb in C1) and `AGENTS_UPDATED.md` (was an older copy missing the VectorDb blocks; now identical to `AGENTS.md`), `C1-Internal-Architecture.md` (new Worker to Meal Planning edge, outbox note) and the C1 drawio (edge added, layout not looked at), `README.md` (endpoint lists, outbox, planned meals, JWT rule, tests), `Book-Knowledge-Integration.md` (recipe candidates, TXT, batches, approve-all, indexing), `Seed-Data-Strategy.md`, `Demo-Script.md`, `PROJECT_SCHEDULE.md`, `CLAUDE.md`. Checked against code: every controller route, every migration and DbSet, and the Compose/`.env.example` variables.
- Known limits: if a reminder time is changed A to B and back to A, the old and new messages both match and two mails can be sent; the progress notification trusts the client-sent planned count (it only affects the user's own data); the outbox can deliver a message twice after a crash between publish and save.
- Full-stack check (same day): `docker compose up` ran the whole stack (15 containers) and the three demo workflows were smoke-tested by hand; old reminders were purged from RabbitMQ.
- Presentation deck (Canva `DAHalyY5ssU`, 25 slides, Vietnamese, following `Software_Project_Presentation_Guideline_V1.0.pptx` and `Workflow_La_Gi_Dinh_Nghia_Va_Dac_Ta.pdf`): the generator invented content, so every slide was rewritten from facts in the repo (90/90 tests, 15 containers, 56 candidates from 52/177 book chunks, real status names, real doc paths). Unknown items are left as `[...]` placeholders: GitHub/demo links, presenter and answerer names, request IDs and email log for the demo. The two known test gaps (BR-04, state transitions) are stated on the Limitations slide. The deck edits are committed in Canva (edit link https://www.canva.com/d/X1IRHv6UrLYI9RA).
- Deck follow-ups (same day): speaker notes added to all 25 slides (timing hints and `[...]` placeholders for names/links); a real screenshot of the Vietnamese home page placed and enlarged on slide 1; arrows on slide 7 (business flow) re-routed as clean elbow connectors; slide 9 (architecture) re-laid out so arrows are straight and non-crossing, with the wrong Gateway to Recommendation/Notification arrows removed (those services are not routed by the gateway). Two arrows on slide 9 (Recommendation to Gemini, Recommendation to RabbitMQ) are hand-drawn shapes, not connectors, so they do not follow if the boxes are moved. Still to fill by the team: GitHub/demo links, member 2/3 and presenter names, request IDs and email log for the demo, backup video, survey figures.

### 2026-11-09 — Team contribution assessment (45 / 35 / 20)

- Added a "Team Contribution Assessment" section recording the agreed split: Hai Anh 45%, Member 1 35%, Member 2 20%, with each member's responsibilities.
- Docs only; no code changed, no tests run. Member names for 1 and 2 are placeholders and should be filled in.

### 2026-11-09 — Final pass: regimen seed, profile preferences in Meal Ideas, docx, fresh-DB check

- Seed: the demo seed (`demo-approved-knowledge-seed.json`, gated by `DietKnowledgeSeed__IncludeDemoApprovedContent`) now also creates "Demo plant-forward 3-day menu" (3 demo guidelines, 6 meals using the two demo recipes), clearly labelled as team-written demo content. It is created once and never overwritten, so Admin edits survive restarts.
- Meal Ideas: the preference field is prefilled from the member's saved profile tags, with a hint and a link to the Profile.
- `docs/database/Database-Diagram-Explanation.docx` was rewritten in Vietnamese for the five modules plus VectorDb (previous version kept outside the repo).
- README, `Demo-Script.md` (new section 7b) and `Submission-Checklist.md` updated.
- Verification: 81 tests pass, 0 warnings. All five migration chains (Identity, DietKnowledge, VectorDb, MealPlanning, Tracking) were applied to an EMPTY throwaway pgvector container and produced exactly the diagram's tables (4 + 13 + 2 + 3 + 4); the container was removed afterwards. Docker run: seeded regimen visible through the API and the Web, prefill and hint shown on Meal Ideas.
- Not done / open: real book-derived content still needs manual verification (existing TODO); the new Web pages were checked by a scripted run, not by eye; no commit pushed.

### 2026-11-09 — Web UI for the new modules

- Member screens: Profile gets an "Eating preferences" form (tags + notes); new nav item and pages `Regimens` (list, details with menu by day, start tracking); Daily Check-in gets a "My diet tracking" card (start with a regimen and/or meal plan, mark completed, cancel, history); Knowledge page gets a "Smart search" box and a new `Knowledge/Search` page (semantic search over the VectorDb with match %), plus category chip, collapsible recipe steps and food nutrition facts on the cards; meal plan create/edit forms got an optional diet regimen select.
- Admin screens: new `AdminRegimens` (list/create/activate, edit with add/remove guidelines and meals), `AdminKnowledge` shows the semantic index status with an "Update index" button and the categories, EditFood manages nutrition facts, EditRecipe manages preparation steps and category, EditGuideline/EditRecipe have an optional category field. Admin nav has "Diet Regimens".
- About 115 new strings have Vietnamese entries in `Services/Translations.cs`.
- Bugs found by the UI run and fixed: new child rows with preset ids under a tracked parent were saved as updates (500) for regimen guidelines/meals, nutrition facts, recipe steps and recipe ingredients (repositories now mark them as added; 2 regression tests); optional text fields left empty were rejected as required (now nullable).
- Verification: solution builds with 0 warnings; 81 tests pass. A scripted browser-style run against Docker (login with the demo Admin and User, real anti-forgery forms) passed for every new action: rebuild index, create regimen, add/remove guideline and meal, a meal beyond the duration refused, nutrition fact add/remove, recipe step add/remove, preferences save, regimen details, start/finish diet tracking, smart search with and without type filter, meal plan with regimen, Vietnamese pages for member and admin, guest access. The pages were NOT looked at visually (the Chrome automation did not respond), so layout, spacing and the new CSS reuse still need a manual look. The test data created by the run was deleted.
- Known limits: updating a recipe through the API is rejected while any of its ingredient foods is not active and approved (existing rule), so such recipes cannot get steps from the Admin UI; the Admin recipe form still edits only one ingredient (existing behaviour); no Web test project exists.
- Still not done: seed data for regimens, Recommendation using the saved profile preference tags, `Database-Diagram-Explanation.docx` rewrite, final QA, commit.

### 2026-11-09 — VectorDb (pgvector) implemented and used by Recommendation

- New `vector-db` container (`pgvector/pgvector:pg16`, host port 5437, volume `vector-db-data`) in `docker-compose.yml`, owned by Diet Knowledge Service through a second DbContext (`VectorDbContext`, migration `InitialVectorSchema` in `Infrastructure/VectorDb/Migrations`; the infrastructure folder/namespace is `VectorDb` because a `Vector` namespace clashes with `Pgvector.Vector`). Tables `KnowledgeEmbeddings` (`vector(768)`, HNSW cosine index, unique per `SourceType`+`SourceId`) and `EmbeddingIndexRuns`.
- `KnowledgeIndexService` embeds only active `Approved` guidelines, foods and recipes (title, category and approved text; never raw chunks or candidates), skips unchanged items by content hash, and removes rows that are no longer eligible (only when the whole run succeeded). A hosted service runs it once after startup (`KNOWLEDGE_INDEX_ON_STARTUP`). Embeddings: Gemini `gemini-embedding-001` at 768 dimensions when Gemini is enabled with a real key, otherwise a local hashed bag-of-words fallback (`local-hash-768`); the model name is stored per row and searches compare only the current model.
- New endpoints: public `GET /api/knowledge/search?q=&type=&limit=`, Admin `POST /api/admin/knowledge-index/rebuild` and `GET /api/admin/knowledge-index/status` (via the gateway under `/diet-knowledge/...`).
- Recommendation Service calls the search endpoint (`ISemanticKnowledgeSearch`) with the sanitized preference tags and ranks the closest approved items first in the Gemini prompt; if the search is unavailable it falls back to the previous keyword matching. It never reads the VectorDb directly.
- Verification: solution builds with 0 warnings; 79 tests pass (7 new in `VectorDbTests`: hashing embedding, approved-only indexing, unchanged/removed handling, failure keeps old rows, search validation and ranking, unconfigured store, Recommendation ranking and fallback). `docker compose up -d --build` created the extension and tables; the startup run indexed 133 of 134 items with the real Gemini embedding model (one hit a 429 rate limit and was added by the Admin rebuild), final index 126 items after the smoke data was removed. Search returned relevant items, including Vietnamese queries on Vietnamese content (403 for a User on rebuild, 401 without a token, 400 for an empty query or type `Chunk`). A recommendation request with preferences `legumes`, `light dinner` completed with legume dishes and the Recommendation logs show the call to `/api/knowledge/search`.
- Cleanup: the "Smoke ..." test data from the previous entry was deleted from the dev databases (regimens, recipes, foods, meal plans, tracking rows, the demo user's smoke preferences).
- Known limits: with Gemini on, the first index of a large knowledge set can hit free-tier rate limits (items left out are picked up by the next rebuild); the local fallback only matches shared words, not meaning; indexing runs at startup and on Admin rebuild, not automatically when an Admin approves a new item.
- Still not done: Web UI for the new modules and a search box/Admin rebuild button, seed data for regimens, Recommendation using the saved profile preference tags, final QA, `Database-Diagram-Explanation.docx` rewrite, commit.

### 2026-11-09 — UserProfile, UserDietTracking and MealPlan.DietRegimenId implemented

- Identity: new `UserProfiles` table (entity `UserPreferenceProfile`, migration `AddUserProfiles`). `GET /api/auth/profile` now also returns `preferenceTags` and `notes`; new `PUT /api/auth/profile/preferences` saves them (tags trimmed and de-duplicated case-insensitively, at most 20 tags of 40 characters, no commas; notes up to 1000 characters).
- Tracking: new `UserDietTrackings` table (migration `AddUserDietTracking`) and endpoints `POST/GET /api/diet-trackings`, `GET /api/diet-trackings/{id}`, `PATCH /api/diet-trackings/{id}/status` (Completed or Cancelled). A user has at most one `Active` tracking; it needs a diet regimen id and/or a meal plan id (not verified against the other services). `DailyTrackings` and `ProgressSummaries` got an optional `UserDietTrackingId` FK and are linked automatically to the active tracking whose period covers the date. Other users' trackings return 404.
- Meal Planning: `MealPlans.DietRegimenId` (optional ID, migration `AddMealPlanDietRegimenId`), accepted on create (and on update; omitted keeps the current value).
- Database diagram regenerated: only the two VectorDb tables are still marked as not implemented. Updated `Physical-Database.md`, `Conceptual-ERD.md`, `Gateway-Routes.md`.
- Verification: solution builds with 0 warnings; 72 tests pass (5 new in `UserProfileAndDietTrackingTests`). `docker compose up -d --build` on the existing volumes applied all four new migrations (Diet Knowledge, Identity, Tracking, Meal Planning). The old `Foods.Category` text became 7 `Categories` rows and all 119 foods are linked. An API smoke through the gateway passed: categories, food with nutrition facts, recipe with steps and category, regimen create/public read (400 for a day outside the duration, 403 for a User), preferences save and update, meal plan with `dietRegimenId`, diet tracking start (409 for a second active one, 404 for another user, finish), daily meal completion. The smoke found one bug, fixed here: a new `UserProfiles` row assigned only through `User.Profile` was treated as an update (500); `IUserRepository.AddProfile` now adds it explicitly.
- The smoke left test data in the dev databases (food/recipe/regimen/meal plan named "Smoke ...", one completed tracking for the demo user, preferences on the demo user).
- Still not done: pgvector VectorDb, Web UI for preferences, categories, regimens, steps, nutrition and diet tracking (the Web app only uses the old API shape and keeps working), seed data for regimens, Recommendation using the saved preference tags.

### 2026-11-09 — Diet Knowledge code aligned with the revised database design (Categories, NutritionFacts, RecipeSteps, Diet Regimen)

- Decisions: the Category column becomes a `Categories` table with `CategoryId` FKs, but the API keeps a category name string (`Category` on foods, optional on recipes and guidelines). `Foods` keeps its name in code (the diagram says "Foods (Ingredient)").
- Added to Diet Knowledge: `Category`, `NutritionFact`, `RecipeStep`, `DietRegimen`, `DietRegimenGuideline`, `DietRegimenMeal` entities, DbContext mapping and migration `AddCategoriesRegimensNutrition` (copies each distinct `Foods.Category` text into `Categories` and links it before dropping the column). Foods get optional `nutritionFacts`, recipes get optional `category` and `steps` on admin create/update (omitted = unchanged, list = replaced); guidelines get optional `category`.
- New endpoints: `GET /api/categories?categoryType=`, public `GET /api/diet-regimens[/{id}]` (active `Approved` only) and Admin `GET/POST/PUT/PATCH activation /api/admin/diet-regimens` (through the gateway under `/diet-knowledge/...`). Regimens must reference active approved guidelines/recipes, meal days within `DurationDays`, valid meal slots. "Delete all knowledge" now also deletes regimens first.
- Verification: solution builds with 0 warnings; 67 tests pass (6 new in `DietModuleTests`). The migration and the new endpoints were NOT run against PostgreSQL (Docker Desktop was not running); the data-copy SQL in the migration is untested.
- Still not done: Web UI for the new fields/regimens, seed data for regimens, `UserProfile` (Identity), `UserDietTracking` (Tracking, plus `UserDietTrackingId` on daily tracking and progress), `DietRegimenId` on meal plans, and the pgvector VectorDb. Docs below still list those as design only.

### 2026-11-09 — Database design revised after lecturer feedback (docs/diagram only)

- Lecturer feedback: the Diet (green) and Tracking (purple) modules were too thin, modules were incomplete (User; Diet with Category/Recipe/Ingredient; diet/nutrition regimen; UserDietTracking), and the vector DB must be separate.
- `docs/database/LongevityDiet-Database.drawio` was regenerated (new layout, 5 modules + separate VectorDb). New tables (marked `*`): `UserProfiles`, `Categories`, `NutritionFacts`, `RecipeSteps`, `DietRegimens`, `DietRegimenGuidelines`, `DietRegimenMeals`, `UserDietTrackings`, and in the new pgvector `VectorDb` `KnowledgeEmbeddings`, `EmbeddingIndexRuns`. Previous version saved only in the scratchpad; git still has the last commit.
- Updated `Physical-Database.md` (Revised design section), `Conceptual-ERD.md`, `C1-Internal-Architecture.md` (planned VectorDb), and `AGENTS.md` (VectorDb owned by Diet Knowledge, no new microservice).
- Verification: diagram exported to PNG with the draw.io CLI and checked visually. No code, migration, Docker Compose or test change; build/tests not re-run.
- Remaining / assumptions: the new tables are NOT implemented (no entities, migrations, APIs, Docker service for pgvector). Design assumptions to confirm with the lecturer: regimens live in Diet Knowledge Service, `Foods` is shown as Ingredients, and only approved knowledge is embedded (raw chunks never). The 2026-10-26 draw.io diagram entry above describes the previous layout.

### 2026-10-31 — Fix "No knowledge candidates could be generated" with Gemini

- Cause: Gemini sometimes writes `sourceChapter` (and could write an ingredient quantity) as a bare number, e.g. `"sourceChapter": 4`. The candidate record expected strings, so the JSON parse failed and the whole answer was reported as "not a JSON array"; every group failed, nothing was marked processed, and Generate returned "No knowledge candidates could be generated right now". Some groups also hit Gemini HTTP 429 (rate limit) earlier.
- Fix: new `LenientStringConverter` reads numbers/null as strings for the Gemini draft fields. The "not a JSON array" error now includes `finishReason` and the first 300 characters of the answer, so the next failure is diagnosable from the log.
- Verification: 61 tests pass (1 new). Real run on the uploaded Vietnamese book after the fix: 56 candidates (46 DietGuideline, 9 Food, 1 Recipe), 52 of 177 chunks analyzed, no Gemini failures in the log.

### 2026-10-31 — Live percentage while candidates are generated

- Clicking "Generate candidates / next batch" now opens a small popup with a spinning ring and the percentage (plus `processed/total` chunks) of that click; the popup is built by `book-candidates.js` and removed when the request ends. Diet Knowledge keeps an in-memory `GenerationProgressTracker` (singleton, per document); `GeminiKnowledgeCandidateGenerator` reports after each Gemini group via a new optional `onProgress` callback, and `GET /api/admin/book-sources/{id}/candidates/generate/status` returns `{running, processedChunks, totalChunks, percent}`.
- Web: the generate form is now a `data-fetch` form (no full reload; result shown as a toast, page sections refreshed) and `book-candidates.js` polls `AdminBookSources/GenerationStatus/{id}` every second while the request runs. Vietnamese label added.
- Verification: build 0 warnings, 60 tests pass (2 new). Status endpoint answers `running:false` when idle. The live bar was not watched during a real Gemini run; progress moves per Gemini group, so with Gemini off (placeholder drafts) it jumps straight to done.

### 2026-10-31 — Vietnamese keywords in the medical-content filters

- `BookChunkRelevance` now also screens Vietnamese: chunk filter terms (ung thư, hóa trị, tiểu đường, đái tháo đường, tim mạch, sa sút trí tuệ, khối u, tự miễn, xơ cứng rải rác, viêm khớp) and bulk-approval blocked terms (bệnh, điều trị, chữa, chẩn đoán, dự đoán, tiên lượng). Text is compared in NFC so decomposed (NFD) diacritics still match. "tuổi thọ" is deliberately not blocked: it is the book's whole theme.
- Effect on the uploaded Vietnamese book (SQL estimate): about 73 of 177 chunks hit two or more chunk-filter terms and would be skipped; the 14-day menu chunks (154-166) are unaffected. Chunks already analyzed are not re-filtered.
- Verification: build 0 warnings, 58 tests pass (5 new). Docker `diet-knowledge-service` rebuilt.

### 2026-10-31 — Recipe candidates from the uploaded book, grounded in its text

- Gemini may now return `Recipe` candidates with an `ingredients` array. The prompt says to use ONLY what the excerpt states (no outside knowledge) and to copy ingredient names/quantities as written. The generator keeps only ingredients whose name appears in the excerpt and drops a recipe with none left. Ingredients are also appended to the candidate summary so the admin sees them while reviewing.
- New column `KnowledgeCandidates.IngredientsJson` (migration `AddCandidateIngredients`, applied at startup). Approving a Recipe candidate creates the `Recipe` and its `RecipeIngredient` rows: an ingredient matching an existing Food by name (case-insensitive) is reused; otherwise a name-only Food is created with `ReviewStatus = NeedsReview` (no nutrition note invented). A recipe candidate with no ingredients is refused. "Approve all" can now approve recipes too.
- Verification: build 0 warnings, 53 tests pass (2 new). Not run against Gemini or the Docker database yet: chunks 154-166 of the uploaded book (the 14-day menu) are already marked analyzed, so Generate will skip them until their `CandidatesGeneratedAtUtc` is cleared or the document is re-uploaded.
- Known gap (fixed below): `BookChunkRelevance` medical-term filters were English-only.

### 2026-10-31 — Admin "delete all knowledge" endpoint

- Added `DELETE /api/admin/knowledge?confirm=true` (Admin only; via gateway `/diet-knowledge/api/admin/knowledge`). It permanently deletes every guideline, food and recipe (ingredients cascade) in one transaction, and resets candidates that had been approved back to `NeedsReview` (clears `ApprovedKnowledgeId`). Book sources, chunks and stored files are kept. Without `confirm=true` it returns `400`. Returns the counts deleted/reset.
- Web: red "Delete all knowledge" button with a confirm dialog on the Admin Knowledge page (`AdminKnowledgeController.DeleteAll`), Vietnamese labels added.
- Verification: build 0 warnings, 51 tests pass (2 new). Docker smoke: no token `401`, User `403`, Admin without confirm `400`. The actual delete (`ExecuteDelete`/`ExecuteUpdate`, not supported by EF InMemory) was NOT run against the dev database to avoid wiping its data.

### 2026-10-31 — Admin book source upload accepts .txt

- Admin Book Sources upload now accepts `.txt` as well as `.pdf` (25 MB limit unchanged). `BookTextExtractor` picks `PdfBookTextExtractor` or the new `TextBookTextExtractor` by stored file extension; a text file is read as one page (page 1) and chunked by size. Stored name/content type follow the type; validation messages and Vietnamese translations updated.
- Verification: build 0 warnings, 49 tests pass (1 changed, 1 new). Not yet tried against the running Docker stack; text files have no real page numbers, so candidate sources show "page 1".

### 2026-10-26 — Edit meal plan items

- Each scheduled item on the meal plan page now has an Edit button. It loads the item into the item form (heading "Edit item", button "Save item", Cancel link) and saves through the existing `PUT /api/meal-plans/{id}/items/{itemId}` via the new `MealPlansController.UpdateItem`. Vietnamese labels added.
- The "100% completion rate with 0 meals" item from the QA notes was a misreading: that card shows the period rate (1 of 1 planned meals eaten) and the view already returns 0% when nothing is planned. No change needed.
- Verification: `docker compose up -d --build web` builds; in the browser editing the note of the QA test item to "qa edited" saved and redirected back with the date, slot, recipe and reminder unchanged. No Web test project exists.

### 2026-10-26 — Sort control on the member Knowledge page

- Added a "Sort by" dropdown (Name A-Z default, Name Z-A, Newest first, Oldest first) to `/Knowledge`. It applies to guidelines, foods and recipes at once, maps to the existing `sortBy`/`sortDirection` API parameters (title/name or `createdAt`), is kept when moving between pages, and has Vietnamese labels. The filter bar grid is widened only inside `.search-hero` so the Tracking page layout is unchanged.
- Verification: `docker compose up -d --build web` builds; in the browser Name (Z-A) lists guidelines from "Weight and Muscle Mass..." downward and Newest first changes the order away from alphabetical. No Web test project exists.
- Correction to the QA note above: the backend `DELETE` on guidelines/foods/recipes is a soft delete (it only deactivates), so the Admin "Deactivate" button already covers it; no hard delete was added.

### 2026-10-26 — Pagination for the Knowledge pages

- The member Diet Knowledge page (`/Knowledge`) always requested page 1 (20 guidelines, 30 foods, 30 recipes), so most of the 88 approved guidelines were unreachable; the Admin Knowledge lists had the same limit (25 per list). Each of the three lists now has its own Previous/Next pager (`guidelinePage`, `foodPage`, `recipePage`) that keeps the search, category and status filters. Added the shared `Views/Shared/_Pager.cshtml`, `PagerViewModel`, pager CSS and the Vietnamese strings (Page / Previous / Next).
- Verification: `docker compose up -d --build web` builds; in the browser, Admin guidelines show "Page 2 / 4" with 25 rows and the member page shows "Page 2 / 5" with 20 cards. No Web test project exists, so there is no automated test for this.
- Remaining UI gaps found in the QA pass: no sort control on the Knowledge pages, no Admin delete of guidelines/foods/recipes, no edit for meal plan items, and the Tracking page shows 100% when 0 meals are planned.

### 2026-10-26 — Database diagram (draw.io)

- Added `docs/database/LongevityDiet-Database.drawio`: all tables of the four databases (IdentityDb, DietKnowledgeDb, MealPlanningDb, TrackingDb) with PK/FK markers, in-database foreign keys (crow's foot) and dashed red logical references (`UserId`, `RecipeId`, `FoodId`, `MealPlanItemId`, `AcceptedMealPlanId`, `ApprovedKnowledgeId`) that have no foreign key. Built from `Physical-Database.md` and the `*DbContext` relationships.
- Verification: exported to PNG with the draw.io desktop CLI and checked the layout. Column types are not shown; regenerate/edit the diagram when the schema changes.

### 2026-10-19 — Fix invisible "Sign in" button in the Home hero

- The hero's secondary button inherited the white `.btn-outline-secondary` background while its text is white, so the label vanished; the button group was also right-aligned. The hero now uses a transparent background for that button and left-aligns the group.
- Follow-up: both hero buttons look the same (transparent with a white outline) and turn white only on hover or keyboard focus.
- Verification: checked in the browser while signed out; the "Sign in" label is visible and both buttons sit on the left.

### 2026-10-19 — Recommendation prompt picks relevant guidelines instead of the alphabetical head

- Problem: Recommendation.Service fetched 50 guidelines sorted by title and sent only the first 12 to Gemini, so with 88 approved guidelines the AI always saw the same A-Z head of the catalog.
- Now it fetches up to 100 per type and sends up to 25 guidelines, 40 foods and 30 recipes. When there are more items than slots, items whose text mentions the user's preference words come first; the remaining slots start at a position derived from the request id (stable for one request, different between requests).
- Verification: build 0 warnings, 48 automated tests pass (2 new: preference match beyond the first slots, per-request variation).
- Note: preference matching is plain substring matching, so Vietnamese preferences do not match English knowledge text and fall back to the rotating sample.

### 2026-10-19 — Candidate review without page reloads

- Approve, Reject, Approve all and the new Reject placeholders buttons on the book source page are submitted with `fetch` (`wwwroot/js/book-candidates.js`): a single card updates in place, bulk actions re-render only the header and the candidate list, and results show as a toast. The MVC actions answer JSON when the request carries `X-Requested-With: fetch` and keep the redirect plus flash message otherwise, so the page still works without JavaScript.
- Added `POST /api/admin/book-sources/{id}/candidates/reject-placeholders` (rejects pending `LocalChunkFallback` drafts) and the matching Admin button (EN/VI).
- SignalR was considered and dropped: these are client-initiated calls, so push messaging adds a hub and proxy setup without a benefit.
- Verification: build 0 warnings, 46 automated tests pass (1 new).

### 2026-10-19 — Admin Knowledge lists approved content by default

- The Admin Knowledge page listed guidelines, foods and recipes in every review state together, so unapproved items looked live. It now shows only `Approved` content by default, with filter buttons for `NeedsReview` and all statuses (query `?status=Approved|NeedsReview|all`; the existing `reviewStatus` Admin API filter is reused).
- Verification: build 0 warnings, 45 automated tests pass.

### 2026-10-19 — Approve all candidates button

- Added `POST /api/admin/book-sources/{id}/candidates/approve-all` and an Admin "Approve all" button (with confirmation, EN/VI) on the book source page.
- Bulk approval skips `LocalChunkFallback` placeholders and any candidate whose title or summary mentions disease, treatment, cure, diagnosis, prediction, lifespan or the cancer/diabetes-style terms (`BookChunkRelevance.IsSafeForBulkApproval`); those stay `NeedsReview`. Duplicate titles and recipe candidates are counted as failed and the run continues. The result message reports approved / left for manual review / failed.
- Verification: build 0 warnings, 45 automated tests pass (2 new). The button was rendered but not pressed against the real data.

### 2026-10-19 — Analyze a whole book source batch by batch

- Problem: every "Generate candidates" click re-analyzed the first 8 chunks (pages 1-9 only) and ignored the rest of the 185-page PDF.
- Added `BookSourceChunks.CandidatesGeneratedAtUtc` (migration `AddChunkCandidatesGeneratedAt`, backfilled from existing candidates). Each click now analyzes the next pending chunks, up to `BookKnowledgeIngestion__MaxChunksPerGeneration` (24), and skips titles that already exist for the document.
- Added `BookChunkRelevance`: drops "Scan to Download"/Bookey footers and skips boilerplate, quotes, acknowledgments and clinical-treatment chunks (two or more cancer/diabetes/Alzheimer-style terms).
- Gemini generator now groups several chunks into one request (6000 characters), can return `Food` candidates (never `Recipe`), and fills page/reference from the chunk. Output token default raised to 2048.
- When Gemini fails (for example HTTP 503) the affected chunks stay pending instead of getting placeholder drafts; placeholders are used only when Gemini is disabled.
- Added `GET /api/admin/book-sources/{id}/candidates/progress` and an Admin progress bar plus "Generate next batch" button (EN/VI).
- Verification: build 0 warnings, 43 automated tests pass (10 new). On the uploaded Bookey summary PDF the batches took it from 3/112 to 112/112 relevant chunks (49 chunks skipped) and 113 `NeedsReview` candidates (12 Food, 101 DietGuideline). Nothing is approved automatically.
- Note: the uploaded PDF is a Bookey summary of the book, not the full text, so coverage is limited by the source. Two candidates mention disease wording ("Low-Protein Diet and Longevity", "Longevity Diet Protein Approach") and should be rejected or reworded at review.
- TODO: Admin must review the 113 candidates; `gemini-3.1-flash-lite` returned HTTP 503 for a long stretch today (run finished with `gemini-3.5-flash-lite` via a temporary env override, `.env` unchanged).

### 2026-10-19 — Web UI decoration to fill empty space

- Problem: on wide screens (about 1900px+) the 1280px content column left large empty side margins, and Home had a blank lower half.
- `wwwroot/css/site.css`: soft gradient/dot-pattern page background; large faint leaf/fruit watermarks in the side margins on screens of 1500px or more; every `.page-head` is now a gradient banner with a leaf watermark.
- `Views/Home/Index.cshtml`: green gradient hero with floating icon bubbles (CTA buttons for visitors), icon chips on the four module cards, and a "How it works" three-step section. New strings have Vietnamese entries in `Services/Translations.cs`.
- Verified visually in Chrome (signed in as the demo user): Home, Knowledge, Tracking, MealPlans, Recommendations. Not rechecked: Admin screens, auth pages while signed out, phone width beyond the existing breakpoints. Content is purely decorative; no new medical claims.

### 2026-10-19 — Switch email provider from Resend to Brevo

- Replaced `ResendEmailSender`/`ResendOptions` with `BrevoEmailSender`/`BrevoOptions` in `NotificationWorker` (`POST https://api.brevo.com/v3/smtp/email`, `api-key` header, JSON `sender`/`to`/`subject`/`textContent`). `IEmailSender` is unchanged. Brevo has no idempotency header, so the old `Idempotency-Key` was dropped; a retried confirmation mail could rarely be sent twice.
- Config: `Resend__*` / `RESEND_*` became `Brevo__*` / `BREVO_*` (`BREVO_BASE_URL`, `BREVO_API_KEY`, `BREVO_FROM_EMAIL`, `BREVO_FROM_NAME`) in `appsettings.json`, `docker-compose.yml`, `.env.example`. `BREVO_FROM_EMAIL` has no default: the log-only fallback stays active when the API key is empty/`development-only` **or** the sender email is empty. The sender must be verified in Brevo.
- Docs updated to Brevo: `README.md`, `CLAUDE.md`, `AGENTS*.md`, `MASTER_PROMPT*.md`, `PROJECT_SCHEDULE.md`, `docs/c4/C0` + `C1`, `docs/development/Database-Docker-Demo-Setup.md`, `docs/submission/Demo-Script.md`, and `AsignmentDiagram.drawio` (both C0 and C1 nodes). Older change-log entries still say Resend because they describe history.
- Tests: `NotificationWorkerTests` rewritten for Brevo (fallback without HTTP, request URL and `api-key` header).
- Verification: solution builds with 0 warnings; 33 tests pass. Real delivery verified end to end with Docker, a Brevo API key and a verified single sender (free-provider address, so mail may land in Spam): a reminder due in 20s produced a confirmation mail at once and the reminder mail at the due time; the user confirmed receiving both. The key and sender live only in the gitignored `.env`; the old Resend key was removed. Sending to several recipients needs no change (recipient = the signed-in user's email).

### 2026-10-19 — Scheduled meal reminder emails (confirmation now + reminder at due time)

- `NotificationWorker` now honours `ReminderRequestedMessage.ScheduledForUtc`. On first delivery of a future reminder it sends a "Meal plan saved - reminder scheduled" confirmation email (Resend category `confirmation`, idempotent by reminder id), then parks the message in a TTL delay queue (`reminder.requests.delay.long`, 1h, used while more than 1h remains; `reminder.requests.delay.short`, 30s otherwise). Delay queues dead-letter back to the reminder exchange, so pending reminders survive worker restarts. When due (or already past) the real reminder email is sent. Retry copies keep the `x-confirmed` header so the confirmation is not repeated.
- New optional settings: `RabbitMq__ReminderShortDelaySeconds` (30), `RabbitMq__ReminderLongDelaySeconds` (3600). Accuracy is about the short delay.
- Verification: worker builds with 0 warnings; end-to-end with Docker + real Resend key (set locally in the gitignored `.env`): published a reminder due in 75s through the RabbitMQ API; confirmation email sent immediately, message re-parked in the short delay queue, reminder email sent at the due time (both HTTP 200 from Resend). Meal Planning was not changed. Resend free sandbox sender (`onboarding@resend.dev`) only delivers to the Resend account owner's email.

### 2026-10-19 — Align `AsignmentDiagram.drawio` with the code; remove UI disclaimer

- Diagram: removed the Meal Planning -> Notification Service gRPC arrow (only Tracking calls gRPC; Meal Planning publishes reminders straight to RabbitMQ). Added Recommendation -> RabbitMQ (publish result), RabbitMQ -> Meal Planning (consume), Recommendation -> Diet Knowledge (HTTP, public APIs) and Diet Knowledge -> Gemini (HTTPS, Admin candidate extraction). Arrow routing was set by hand; review layout in draw.io.
- Web: removed the "education and meal planning only" disclaimer band/note (`_Layout`, Home) and its VI translation. The disclaimer remains in project docs/scope; the no-medical-advice rule still applies to code.
- Verification: diagram XML parses; Web builds with 0 warnings.

### 2026-10-19 — Apply Stitch visual design to the Web UI

- Visual language taken from the Google Stitch export (kept locally in `stitch-export/`, now gitignored): green `#2e7d5b` palette, Plus Jakarta Sans + Inter fonts, Material Symbols icons (loaded from Google Fonts; system fonts are the offline fallback), rounded cards, chips, stat cards.
- Structure unchanged: still Razor + Bootstrap 5, same controllers, view models, tag helpers and `@T[...]`. `wwwroot/css/site.css` was rewritten on the new tokens and keeps every existing class name, so views that have no Stitch design (Home, Admin Edit forms) pick up the theme automatically.
- Converted (markup restyled on real data): `_Layout` (brand mark, active nav pill, avatar chip, disclaimer band, footer), `_Alerts`, Auth Login/Register (tabs + centered card)/Profile, Knowledge (search hero + card grid with Approved/type chips), MealPlans Index (plan cards with Upcoming/Active now/Finished chip derived from dates) and Details (items grouped by day as meal cards), Recommendations Index/Details, Tracking (stat cards + progress ring from real counts, meal cards), AdminBookSources Index/Details, AdminKnowledge Index (stat cards), Shared/Error.
- Deliberately NOT taken from Stitch: invented clinical content (macro ratios, biomarkers, "Longevity Index", fasting windows, Google/Apple sign-in, avatars/"Elena Vance", admin sidebar), because the app is educational only and has no backend for them.
- New Vietnamese strings added to `Services/Translations.cs`.
- Verification: `dotnet build` 0 warnings / 0 errors (Razor views compile); `dotnet test` 33 passed; Web started alone and `/`, `/Auth/Login`, `/Auth/Register` returned 200 (data pages return 500 without the Gateway, as before).
- Remaining TODO: no visual check of the data-backed screens yet (run `docker compose up -d --build` and click through as User and Admin, EN and VI); Home and Admin Edit forms were not redesigned; fonts/icons need internet access.

### 2026-10-14 — English/Vietnamese language switch (guests, members, admins)

- Added an `EN | VI` switch in the top bar for everyone (guest, member, admin). `LanguageController.Set` stores the choice in a cookie (UI language only; date/number parsing stays en-US so forms keep working) and redirects to a local URL only.
- Approach: English text is the key, `Services/Translations.cs` holds the Vietnamese dictionary, `Translator` (`@T["..."]`, `T.F(...)`) looks it up and falls back to English. Data-annotation labels/validation messages go through the same dictionary (`DictionaryStringLocalizerFactory`, `LocalizedValidationAttributeAdapterProvider`, `HumanizedDisplayNameProvider`); flash messages and API error messages are translated when shown.
- All views (member, admin, auth, layout) were converted; stored values such as meal slots stay English in forms (`Breakfast`, ...) while their labels are translated.
- Verification: headless-Chrome run in Vietnamese for guest, member and admin pages (no leftover English UI text; remaining English is database content), wrong-login / empty-form / bad-date errors shown in Vietnamese, switching back to English works, open-redirect attempt ignored.
- Content follow-up (same day): platform content that ships with the app (seed guidelines, foods, recipes, categories, quantities, source notes, recommendation disclaimer) is now translated at display time through `Services/ContentTranslations.cs` (`T.C(...)`, `T.Q(...)`) on the Knowledge, Admin lists, Meal Plans, Daily check-in and Meal ideas screens; edit forms still show the stored value. The UI language is also sent with a recommendation request (`Language` on the API, command and `RecommendationRequestMessage`, default `en`), so Gemini writes meal titles and the disclaimer in Vietnamese when the UI is Vietnamese (new unit tests, 33 pass). The accept form pre-selects catalog items by English or Vietnamese name.
- Remaining TODO: content written by people or by Gemini (admin-entered items, plan names, notes, PDF chunks and Gemini-generated knowledge candidates, recommendations created before this change) stays in its original language; searching in Vietnamese does not match English seed content; new UI text must be added to `Translations.cs` / `ContentTranslations.cs` (a missing entry just shows English); no automated test covers the dictionaries.

### 2026-10-14 — Friendlier member UI (meal ideas, daily check-in, home)

- Meal ideas (Recommendations): clearer wording ("How many meal ideas?" — the old "Days" field really meant the number of suggested meals), tap-to-add preference chips, a 3-step explainer, a note that illness/treatment/lifespan preferences are ignored, friendly status labels, and auto-refresh while a request is pending. Accepting now lists every suggested meal with its own day, meal slot and catalog recipe/food (pre-matched by name), so one request creates a multi-meal plan; plan dates are derived from the chosen days.
- Daily check-in (Tracking): per-day meal list with "Mark as eaten"/"Undo" instead of a dropdown, meal names instead of GUIDs, default progress window = current week. Fixed the progress card: the API's `completionRate` is a 0–1 ratio (100% was shown as "1%") and its `plannedMeals` only counts already-tracked meals, so the planned total is now taken from the meal plans.
- Home page rewritten (welcome + four plain-language entry cards + disclaimer); nav renamed (Home, Diet Knowledge, Meal Plans, Meal Ideas, Daily Check-in); meal plan form labels no longer show raw property names.
- Verification: `dotnet build`/tests pass; Docker run checked with headless Chrome screenshots and a scripted flow (request ideas → untick one → create plan with 3 meals → mark eaten → progress 1 of 4, 25%).
- Remaining TODO: UI copy is English only; the "Add item" form on the meal plan page still has separate Food/Recipe dropdowns; no automated UI tests; mobile layout only checked by CSS, not on a real device.

### 2026-10-14 — Admin-only workspace and book source deletion

- Admin accounts no longer see member screens: layout shows only `Managed Knowledge` and `Book Sources` for admins; `Home`, `Knowledge`, `MealPlans`, `Recommendations`, `Tracking` redirect admins to `AdminKnowledge` (new `AppController.RequireMember`).
- Added `DELETE /api/admin/book-sources/{id}` (Admin only, `204`; `404` if missing): removes the document, chunks (cascade) and candidates, then deletes the stored PDF (only inside the storage root). Knowledge already approved from the book is kept. Web: Delete buttons (with confirm) on the book source list and details pages.
- Tests: 2 new ingestion tests (delete removes document/chunks/candidates/file; missing document throws) — 31 pass.
- Verification (Docker): deleted the older duplicate `82122d9a91ebd063.pdf` through the gateway (`204`, file removed from volume, second delete `404`, delete as `User` `403`); admin web session redirects member routes and nav shows only admin links; user session unchanged.
- Cleanup: removed two orphan PDFs from the `diet-knowledge-book-sources` volume (left by test records deleted earlier via direct SQL); the volume now matches the single remaining database record.
- Remaining TODO: no web-level automated test for the admin redirects.

### 2026-10-14 — Gemini retry, time budget, and per-chunk local fallback

- With Gemini enabled, `candidates/generate` returned `504`: `gemini-3.8-flash` answered `503 UNAVAILABLE` / timed out, and 8 sequential 30 s calls exceeded the gateway timeout.
- `GeminiKnowledgeCandidateGenerator` now retries transient failures (408/429/5xx, timeouts) with linear backoff, caps total Gemini time per request, stops calling Gemini after consecutive failures, and fills failed/skipped chunks with local fallback drafts. New `Gemini__MaxRetries`, `RetryDelaySeconds`, `MaxConsecutiveFailures`, `TotalBudgetSeconds` options (defaults 2 / 2 / 2 / 60; not yet wired into `docker-compose.yml`).
- Verification: `dotnet build` clean, 29 tests pass; against the real 23 MB PDF with Gemini overloaded, generate returned `200` in ~60 s with 5 `NeedsReview` fallback candidates.
- Follow-up verification: with a new key and `GEMINI_MODEL=gemini-3.1-flash-lite` (the free quota of `gemini-3.8-flash` was 20 requests/day and exhausted), candidate generation returned 12 `Gemini` candidates in ~22 s and a recommendation request completed with Gemini-written titles.
- Remaining TODO: no unit test for the retry/fallback path; 60 s budget is close to the gateway timeout.

### 2026-10-14 — Fix PDF ingestion failing on NUL characters

- Real-PDF upload (23 MB, 161 pages) returned `500`: PdfPig text contained `\0`, which PostgreSQL rejects (`22021 invalid byte sequence for encoding "UTF8": 0x00`).
- `PdfBookTextExtractor.Normalize` now strips control characters and unpaired surrogates before chunking.
- Verification: `docker compose up -d --build`, admin login via gateway, re-upload through `/diet-knowledge/api/admin/book-sources` returned `201` with status `Chunked` and 161 chunks; `candidates/generate` produced 5 `LocalChunkFallback` candidates (`NeedsReview`).
- Remaining TODO: no automated regression test for the NUL case yet; the uploaded file is a third-party summary ("Bookey"), not the book itself, so its candidates must not be approved as book-sourced knowledge without review.

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
