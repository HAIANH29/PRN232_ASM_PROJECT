# MASTER PROMPT — Scaffold Longevity Diet Platform

You are acting as a senior .NET distributed-systems engineer.

Before doing any work, read `AGENTS.md` in the repository root and treat it as the project source of truth.

## Goal
Create the **initial project skeleton only** for the PRN232 assignment **Longevity Diet Platform**. Do not build the whole product yet.

The system is based on the book *The Longevity Diet*. It helps users understand approved book-derived diet knowledge, create meal plans, track adherence/progress, and receive reminders. It is not a medical diagnosis/treatment platform.

## Required architecture
Create a .NET solution that is ready for:

- ASP.NET Core MVC Web Application
- YARP API Gateway
- Identity Service
- Diet Knowledge Service
- Meal Planning Service
- Tracking Service
- RabbitMQ
- .NET Reminder Worker
- PostgreSQL database-per-service
- JWT authentication
- Docker Compose
- Swagger/OpenAPI
- gRPC requirement
- Optional AI Recommendation Service

### gRPC choice
Scaffold `Recommendation Service` and a minimal `.proto` contract so:
`Meal Planning Service -> Recommendation Service` uses **gRPC**.

Do not implement real AI logic yet. Provide an interface/configuration placeholder for a future external AI provider.

### Async messaging
Scaffold:
`Meal Planning Service -> RabbitMQ -> Reminder Worker`

Do not implement a complex scheduling engine yet. Create only the contracts/configuration/placeholder consumer needed to prove the architecture.

## Internal architecture
Each REST microservice must follow:
`API -> Services -> Repository`

Prefer projects/layers corresponding to:
- `.Api`
- `.Application`
- `.Infrastructure`
- `.Domain`

Do not let controllers access DbContext directly.

## Databases
Prepare separate PostgreSQL contexts/configuration for:
- IdentityDb
- DietKnowledgeDb
- MealPlanningDb
- TrackingDb

Never create cross-database foreign keys.

## Phase-1 entities
Create only minimal domain model shells needed to establish ownership:

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

Do not over-model the domain yet.

## Deliverables
At the end, the repository should contain:

1. A compiling `.sln`
2. Project structure for all required containers/services
3. Correct project references
4. Minimal `Program.cs` for each executable
5. Swagger for REST APIs
6. Health check endpoints
7. EF Core DbContext skeletons
8. YARP routing skeleton
9. RabbitMQ producer/consumer contract skeleton
10. gRPC `.proto` + server/client skeleton
11. Dockerfiles
12. `docker-compose.yml`
13. `.env.example`
14. `README.md` explaining how to run the skeleton
15. Keep and update `AGENTS.md`
16. Architecture docs:
    - `docs/c4/C0-System-Context.md`
    - `docs/c4/C1-Internal-Architecture.md`
    - `docs/erd/Conceptual-ERD.md`
    - `docs/database/Physical-Database.md`

## C0 content
Keep C0 simple:
- User
- Admin
- Longevity Diet Platform
- Email Provider
- AI Provider only because Recommendation Service is scaffolded

Do not show internal services/databases in C0.

## C1 content
Show:
- Web Application
- API Gateway
- Identity Service
- Diet Knowledge Service
- Meal Planning Service
- Tracking Service
- Recommendation Service
- 4 owned PostgreSQL databases
- RabbitMQ
- Reminder Worker
- Email Provider
- AI Provider

Label communication only where useful:
- HTTPS / REST
- HTTP / REST
- gRPC
- Publish / Consume
- HTTPS

Keep diagram text short.

## Important restrictions
- Do not invent unrelated features.
- Do not add mobile app.
- Do not add Cloudinary.
- Do not add payment.
- Do not add Kubernetes.
- Do not implement full CRUD yet.
- Do not implement real email sending yet.
- Do not implement real AI calls yet.
- Do not use one shared database.
- Do not create direct database access across services.
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
