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
