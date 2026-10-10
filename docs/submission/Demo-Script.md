# Demo Script

This script is the recommended presentation path for the teacher. It assumes the project is run locally with Docker Compose and the default demo seed data.

## 1. Pre-Demo Setup

From the repository root:

```powershell
copy .env.example .env
dotnet restore LongevityDietPlatform.sln
dotnet build LongevityDietPlatform.sln
dotnet test LongevityDietPlatform.sln
docker compose config --quiet
docker compose up -d --build
```

Open these pages before presenting:

- Web Application: `http://localhost:5000`
- API Gateway routes: `http://localhost:5001/routes`
- RabbitMQ Management: `http://localhost:15672`

RabbitMQ local credentials:

- Username: `guest`
- Password: `guest`

Demo accounts:

| Role | Email | Password |
| --- | --- | --- |
| Admin | `admin@longevity.local` | `Admin@123456` |
| User | `user@longevity.local` | `User@123456` |

## 2. Architecture Explanation

Show these files:

- `docs/c4/C0-System-Context.md`
- `docs/c4/C1-Internal-Architecture.md`
- `docs/erd/Conceptual-ERD.md`
- `docs/database/Physical-Database.md`

Explain the main points:

- Web calls one backend entry point: API Gateway.
- Public REST services: Identity, Diet Knowledge, Meal Planning, Tracking.
- Internal services: Recommendation Service, Notification Service, Notification Worker.
- Four PostgreSQL databases are owned by four stateful services.
- Tracking to Notification uses gRPC.
- Recommendation, reminder, and notification delivery use RabbitMQ.
- Google Gemini and Resend are external providers with safe local fallbacks.

## 3. Gateway And Health Check

Open:

- `http://localhost:5001/routes`
- `http://localhost:5101/health`
- `http://localhost:5102/health`
- `http://localhost:5103/health`
- `http://localhost:5104/health`
- `http://localhost:5105/health`

Point out:

- API Gateway exposes only Identity, Diet Knowledge, Meal Planning, and Tracking.
- Recommendation Service and Notification Service are internal and do not have public gateway routes.

## 4. Admin Demo Path

1. Open `http://localhost:5000`.
2. Login as `admin@longevity.local` with `Admin@123456`.
3. Open the Admin knowledge screens.
4. Show that Admin can manage:
   - Diet Guidelines
   - Foods
   - Recipes
5. Create or edit one small demo item if needed.
6. Show `Book Sources`.
7. Explain the book-source rule:
   - Only Admin can upload a legally obtained PDF.
   - Uploaded PDFs and raw chunks are private project artifacts.
   - Generated candidates are not user-facing until Admin reviews and approves them.
   - Recommendation uses only active `Approved` managed knowledge, not raw PDF chunks.
8. If the team has a legal sample PDF available, upload it and show:
   - source document row
   - chunks
   - candidate generation
   - approve/reject buttons

Do not upload or distribute an unauthorized full book copy during presentation.

## 5. User Demo Path

1. Logout or open a separate browser session.
2. Login as `user@longevity.local` with `User@123456`.
3. Open Profile to show JWT-backed authenticated user data.
4. Browse Diet Guidelines, Foods, and Recipes.
5. Show search/filter/pagination on Foods or Recipes.
6. Open Meal Planning.
7. Create a meal plan:
   - Name: `Teacher Demo Plan`
   - Start date: today's date
   - End date: a date within the same week
8. Add a meal plan item using an approved food or recipe.
9. Explain that Meal Planning validates food/recipe IDs through Diet Knowledge REST APIs and does not read another service database.

## 6. Recommendation Flow

1. Open the recommendation request screen.
2. Request recommendations with simple preferences such as:
   - `legumes`
   - `vegetables`
   - `simple`
3. Wait briefly and refresh/list the recommendation requests if needed.
4. Show the completed recommendation result.
5. Accept the recommendation into a meal plan.
6. Explain:
   - Meal Planning publishes `RecommendationRequest`.
   - Recommendation Service consumes it from RabbitMQ.
   - Recommendation Service builds context from approved Diet Knowledge only.
   - With Gemini disabled locally, safe fallback recommendations are returned.
   - Meal Planning consumes `RecommendationResult` and stores it for user review.

## 7. Tracking And Notification Flow

1. Open Tracking.
2. Select the meal plan item created during the demo.
3. Mark it completed.
4. Open Progress Summary for the same day.
5. Confirm completed meals and planned meals match.
6. Explain:
   - Tracking stores daily tracking in `TrackingDb`.
   - Tracking calls Notification Service through gRPC when daily progress is complete.
   - Notification Service publishes a notification message to RabbitMQ.
   - Notification Worker consumes it and logs/sends email through Resend fallback.

## 8. RabbitMQ Evidence

Open RabbitMQ Management:

- `http://localhost:15672`

Show expected queues/exchanges, for example:

- `reminder.requests`
- `notification.messages`
- `recommendation.requests`
- `recommendation.results`
- related dead-letter queues

Explain that durable topology, retries, and dead-letter queues are part of the implementation.

## 9. Automated Tests

Run:

```powershell
dotnet test LongevityDietPlatform.sln
```

Explain that tests cover:

- Identity behavior.
- Diet Knowledge validation/search/pagination.
- Admin PDF ingestion workflow.
- Meal Planning validation and publishers.
- Tracking progress notification trigger.
- Recommendation safety rules.
- Notification Worker email delivery fallback.
- API authorization boundaries.

## 10. Closing Summary

End with these points:

- The project demonstrates microservices, layered architecture, REST APIs, JWT, PostgreSQL database-per-service, Docker Compose, gRPC, RabbitMQ, Worker Service, and C4 documentation.
- The domain remains educational and planning-focused.
- The app avoids diagnosis, treatment advice, disease prediction, and lifespan prediction.
- Real book-derived content still requires team review/approval from a legally obtained source before being claimed as final approved book knowledge.
