# MASTER PROMPT — Scaffold Longevity Diet Platform

You are acting as a senior .NET distributed-systems engineer.

Before doing any work, read `AGENTS.md` in the repository root and treat it as the project source of truth.

## Goal
Create the **initial project skeleton only** for the PRN232 assignment **Longevity Diet Platform**. Do not build the whole product yet.

The system is based on the book *The Longevity Diet*. It helps users understand approved book-derived diet knowledge, create meal plans, track adherence/progress, receive reminders/notifications, and request AI-assisted meal recommendations. It is not a medical diagnosis/treatment platform.

## Required architecture
Create a .NET solution ready for:

- Web Application
- YARP API Gateway
- Identity Service
- Diet Knowledge Service
- Meal Planning Service
- Tracking Service
- Recommendation Service
- Notification Service
- RabbitMQ
- .NET Notification Worker
- PostgreSQL database-per-stateful-service
- JWT authentication
- Docker Compose
- Swagger/OpenAPI
- gRPC
- Google Gemini integration placeholder
- Resend integration placeholder

## Required communication design

### REST
Web Application -> API Gateway -> public business services

### gRPC
Tracking Service -> Notification Service

Create a minimal `.proto` contract for this flow.

### RabbitMQ — Recommendation flow
Meal Planning Service:
- publishes `RecommendationRequest`

Recommendation Service:
- consumes `RecommendationRequest`
- calls Google Gemini later over HTTPS
- publishes `RecommendationResult`

Meal Planning Service:
- consumes `RecommendationResult`

### RabbitMQ — Notification/reminder flow
Meal Planning Service:
- publishes reminder messages

Tracking Service:
- calls Notification Service via gRPC

Notification Service:
- publishes notification messages

Notification Worker:
- consumes notification/reminder messages
- calls Resend later over HTTPS

Do not create a separate Reminder Worker. Use only:
`Notification Worker [.NET Worker Service]`

## Databases
Prepare separate PostgreSQL contexts/configuration for:
- IdentityDb
- DietKnowledgeDb
- MealPlanningDb
- TrackingDb

Do NOT create:
- RecommendationDb
- NotificationDb

unless a later concrete requirement needs persistence.

Never create cross-database foreign keys.

## Phase-1 entities

Identity:
- User
- Role

Diet Knowledge:
- DietGuideline
- Food
- Recipe
- RecipeIngredient

Meal Planning:
- MealPlan
- MealPlanItem

Tracking:
- DailyTracking
- MealTracking
- ProgressSummary

Do not over-model the domain yet.

## Deliverables
At the end, the repository should contain:

1. A compiling `.sln`
2. Project structure for all required containers/services
3. Correct project references
4. Minimal `Program.cs` for each executable
5. Swagger for public REST APIs
6. Health check endpoints
7. EF Core DbContext skeletons for 4 persistent services
8. YARP routing skeleton
9. RabbitMQ producer/consumer contract skeletons
10. gRPC `.proto` + server/client skeleton for Tracking -> Notification
11. Recommendation request/result message contracts
12. Notification/reminder message contracts
13. Dockerfiles
14. `docker-compose.yml`
15. `.env.example`
16. `README.md`
17. Keep and update `AGENTS.md`
18. Architecture docs:
    - `docs/c4/C0-System-Context.md`
    - `docs/c4/C1-Internal-Architecture.md`
    - `docs/erd/Conceptual-ERD.md`
    - `docs/database/Physical-Database.md`

## C0 content
Keep C0 simple:
- User
- Admin
- Longevity Diet Platform
- Google Gemini [AI Provider]
- Resend [Email Service]

Do not show internal services/databases/RabbitMQ in C0.

## C1 content
Show:
- Web Application
- API Gateway [YARP]
- Identity Service
- Diet Knowledge Service
- Meal Planning Service
- Tracking Service
- Recommendation Service
- Notification Service
- 4 owned PostgreSQL databases
- RabbitMQ [Message Broker]
- Notification Worker [.NET Worker Service]
- Google Gemini [AI Provider]
- Resend [Email Service]

Communication labels should stay short:
- HTTPS
- HTTP / REST
- gRPC
- Publish
- Consume

## Important restrictions
- Do not invent unrelated features.
- Do not add mobile app.
- Do not add Cloudinary.
- Do not add payment.
- Do not add Kubernetes.
- Do not implement full CRUD yet.
- Do not implement real email sending yet.
- Do not implement real Gemini calls yet.
- Do not use one shared database.
- Do not create direct database access across services.
- Do not create RecommendationDb or NotificationDb in Phase 1.
- Do not keep the old `Meal Planning -> Recommendation` gRPC design.
- Do not create a separate Reminder Worker.
- Do not generate hundreds of files without need.

## Working style
First show me the proposed repository tree and explain the responsibility of each top-level project.

Then wait for my approval before generating or modifying the codebase.

After approval:
- implement the scaffold incrementally,
- build after each major step,
- fix compilation errors,
- summarize files created/changed,
- list remaining TODOs.

If the repository is empty, initialize it according to this architecture.
