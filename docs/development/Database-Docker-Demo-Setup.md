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

There are no cross-database foreign keys. Cross-service references remain ID values only.

## EF Core Migrations In Source

| Database | Migration files |
| --- | --- |
| `IdentityDb` | `src/Services/Identity/LongevityDiet.Identity.Infrastructure/Persistence/Migrations` |
| `DietKnowledgeDb` | `src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Infrastructure/Persistence/Migrations` |
| `MealPlanningDb` | `src/Services/MealPlanning/LongevityDiet.MealPlanning.Infrastructure/Persistence/Migrations` |
| `TrackingDb` | `src/Services/Tracking/LongevityDiet.Tracking.Infrastructure/Persistence/Migrations` |

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
| `RESEND_BASE_URL` | Resend API base URL | `https://api.resend.com` |
| `RESEND_API_KEY` | Resend API key | `development-only` |
| `RESEND_FROM_EMAIL` | Sender email for Resend | `onboarding@resend.dev` |
| `RESEND_FROM_NAME` | Sender name for email payloads | `Longevity Diet Platform` |

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

## Reset Local Demo Data

This removes local Docker volumes and recreates the databases from migrations and seed data:

```powershell
docker compose down -v
docker compose up -d --build
```

Only run the reset command when local demo data can be deleted.
