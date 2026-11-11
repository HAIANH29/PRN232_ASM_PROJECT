# Database, Docker, And Demo Setup

This guide is the clean-machine setup path for the PRN232 demo.

## Prerequisites

- .NET SDK 9
- Docker Desktop with Linux containers
- PowerShell

## First Run

From the repository root:

```powershell
copy .env.example .env
dotnet restore LongevityDietPlatform.sln
dotnet build LongevityDietPlatform.sln
docker compose config --quiet
docker compose up -d --build
```

Open:

- Web: `http://localhost:5000`
- API Gateway: `http://localhost:5001`
- Gateway routes: `http://localhost:5001/routes`
- RabbitMQ Management: `http://localhost:15672`

RabbitMQ default local credentials are `guest` / `guest`.

## Automatic Database Initialization

Each stateful service owns and initializes only its own database:

| Service | Database | Startup behavior |
| --- | --- | --- |
| Identity Service | `IdentityDb` | Applies EF Core migrations, seeds `User` and `Admin` roles, demo admin, and demo user |
| Diet Knowledge Service | `DietKnowledgeDb` | Applies EF Core migrations, seeds book-review data, optionally seeds approved local demo knowledge, and stores Admin PDF ingestion metadata/chunks/candidates |
| Meal Planning Service | `MealPlanningDb` | Applies EF Core migrations |
| Tracking Service | `TrackingDb` | Applies EF Core migrations |
| Diet Knowledge Service | `VectorDb` (separate pgvector container, `localhost:5437`) | Applies its own EF Core migrations (creates the `vector` extension), then indexes active `Approved` knowledge once after startup; unchanged items are skipped on later starts |

There are no cross-database foreign keys. Cross-service references remain ID values only.

## EF Core Migrations In Source

| Database | Migration files |
| --- | --- |
| `IdentityDb` | `src/Services/Identity/LongevityDiet.Identity.Infrastructure/Persistence/Migrations` |
| `DietKnowledgeDb` | `src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Infrastructure/Persistence/Migrations` |
| `MealPlanningDb` | `src/Services/MealPlanning/LongevityDiet.MealPlanning.Infrastructure/Persistence/Migrations` |
| `TrackingDb` | `src/Services/Tracking/LongevityDiet.Tracking.Infrastructure/Persistence/Migrations` |
| `VectorDb` | `src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Infrastructure/VectorDb/Migrations` |

Diet Knowledge Service has two DbContexts, so `dotnet ef` needs `--context DietKnowledgeDbContext` or `--context VectorDbContext`.

## Vector Database (pgvector)

- Container `vector-db` (`pgvector/pgvector:pg16`), volume `vector-db-data`, host port `5437`. Only Diet Knowledge Service connects to it (`ConnectionStrings__VectorDb`).
- It stores embeddings of active `Approved` guidelines, foods and recipes only. Raw book chunks and unreviewed candidates are never embedded.
- Embeddings come from Gemini (`EMBEDDING_MODEL`, default `gemini-embedding-001`, 768 dimensions) when `GEMINI_ENABLED=true` and a real key is set; otherwise a local hashed bag-of-words embedding is used so the feature still works offline. Each row stores the model name; a search only compares vectors from the current model, so after switching models run an Admin rebuild.
- `KNOWLEDGE_INDEX_ON_STARTUP=false` turns off the startup indexing.
- Admin: `POST /api/admin/knowledge-index/rebuild` indexes new or changed items and removes items that are no longer approved or active; `GET /api/admin/knowledge-index/status` shows the model, item count and latest run. Public: `GET /api/knowledge/search?q=&type=&limit=`.
- Recommendation Service asks `GET /api/knowledge/search` for items close to the user's preferences and ranks them first in the prompt; if the search is unavailable it falls back to plain keyword matching.

## Demo Accounts

Configured in `.env`:

| Role | Email | Password |
| --- | --- | --- |
| Admin | `admin@longevity.local` | `Admin@123456` |
| User | `user@longevity.local` | `User@123456` |

The admin account has both `User` and `Admin` roles. The user account has only the `User` role.

## Demo Knowledge Data

Diet Knowledge loads two seed datasets:

- `book-knowledge-seed.json`: source-review working data that remains `NeedsReview`.
- `demo-approved-knowledge-seed.json`: concise approved local demo data so public browse/search, Meal Planning validation, and Recommendation context have usable data immediately after setup.

`DIET_KNOWLEDGE_SEED_INCLUDE_DEMO_APPROVED_CONTENT=true` enables the approved local demo dataset. Set it to `false` when you want to verify only manually reviewed book content.

The local demo dataset is for the teacher demo path. The team still must replace placeholder source references with exact chapter/page/source metadata after checking a legally obtained copy of the book before claiming final book-source approval.

## Admin Book PDF Ingestion

The Web admin area includes `Book Sources` for uploading a legally obtained PDF copy used by the project team.

- Uploaded PDF files are private local artifacts and are not committed to git.
- Docker stores them in the `diet-knowledge-book-sources` volume at `/app/storage/book-sources`.
- Diet Knowledge Service extracts text into `BookSourceChunks` and can generate `KnowledgeCandidates`.
- Generated candidates remain Admin-only until approved into managed Diet Knowledge content.
- Public browsing, Meal Planning validation, and Recommendation context still use only active `Approved` guidelines, foods, and recipes.

## Environment Variables

| Variable | Purpose | Default |
| --- | --- | --- |
| `POSTGRES_USER` | PostgreSQL username for all local databases | `postgres` |
| `POSTGRES_PASSWORD` | PostgreSQL password for all local databases | `postgres` |
| `RABBITMQ_DEFAULT_USER` | RabbitMQ username | `guest` |
| `RABBITMQ_DEFAULT_PASS` | RabbitMQ password | `guest` |
| `JWT_SIGNING_KEY` | Shared local JWT signing key | `development-only-replace-with-secret` |
| `JWT_ALLOW_DEVELOPMENT_KEY` | Lets services start with the public development key (local demo only; services refuse it otherwise) | `true` |
| `INTERNAL_API_KEY` | Shared secret for the internal reminder check (Notification Worker to Meal Planning) | `development-only-internal-key` |
| `IDENTITY_ADMIN_EMAIL` | Demo admin email | `admin@longevity.local` |
| `IDENTITY_ADMIN_PASSWORD` | Demo admin password | `Admin@123456` |
| `IDENTITY_ADMIN_DISPLAY_NAME` | Demo admin display name | `Demo Admin` |
| `IDENTITY_USER_EMAIL` | Demo user email | `user@longevity.local` |
| `IDENTITY_USER_PASSWORD` | Demo user password | `User@123456` |
| `IDENTITY_USER_DISPLAY_NAME` | Demo user display name | `Demo User` |
| `DIET_KNOWLEDGE_SEED_INCLUDE_DEMO_APPROVED_CONTENT` | Loads approved local demo knowledge | `true` |
| `BOOK_KNOWLEDGE_STORAGE_PATH` | Container path for uploaded book PDFs | `/app/storage/book-sources` |
| `GEMINI_ENABLED` | Enables real Gemini calls | `false` |
| `GEMINI_ENDPOINT` | Gemini API base URL | `https://generativelanguage.googleapis.com` |
| `GEMINI_API_KEY` | Gemini API key | `development-only` |
| `GEMINI_MODEL` | Gemini model name | `gemini-2.0-flash` |
| `BREVO_BASE_URL` | Brevo API base URL | `https://api.brevo.com/v3` |
| `BREVO_API_KEY` | Brevo API key | `development-only` |
| `BREVO_FROM_EMAIL` | Sender email; must be a sender or domain verified in Brevo. Empty keeps the log-only fallback | empty |
| `BREVO_FROM_NAME` | Sender name for email payloads | `Longevity Diet Platform` |

## Smoke Checks

```powershell
Invoke-RestMethod http://localhost:5001/routes
Invoke-RestMethod http://localhost:5001/diet-knowledge/api/foods
Invoke-RestMethod http://localhost:5001/diet-knowledge/api/recipes
```

Login through the Web app with either demo account, or post to the gateway:

```powershell
$body = @{ email = "user@longevity.local"; password = "User@123456" } | ConvertTo-Json
Invoke-RestMethod http://localhost:5001/identity/api/auth/login -Method Post -ContentType "application/json" -Body $body
```

Run automated tests before final demo:

```powershell
dotnet test LongevityDietPlatform.sln
```

For the presentation path, follow `docs/submission/Demo-Script.md`.

## Reset Local Demo Data

This removes local Docker volumes and recreates the databases from migrations and seed data:

```powershell
docker compose down -v
docker compose up -d --build
```

Only run the reset command when local demo data can be deleted.
