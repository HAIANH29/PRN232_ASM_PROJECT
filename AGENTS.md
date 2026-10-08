# AGENTS.md — Longevity Diet Platform

## 1. Project Goal
Build a PRN232 distributed application named **Longevity Diet Platform**.

The product helps users understand and apply dietary/lifestyle principles derived from the book **The Longevity Diet**. The platform is an educational/planning/tracking system, not a medical diagnosis or treatment system.

The project must demonstrate:
- Microservices architecture
- ASP.NET Core REST APIs
- Layered architecture inside each service
- JWT authentication/authorization
- Search, filter, sort, pagination where appropriate
- gRPC for at least one internal service-to-service flow
- RabbitMQ for asynchronous messaging
- Background processing with .NET Worker Service
- PostgreSQL with database-per-service
- Docker / Docker Compose
- C4 architecture documentation

## 2. Scope Rules
Always keep the project aligned with the book-based domain.

Do:
- Diet guidelines derived from the approved book content
- Foods and recipes compatible with those guidelines
- Meal planning
- Daily tracking / progress
- Reminder emails
- Optional AI-assisted recommendation based only on approved diet knowledge and user preferences

Do NOT:
- Turn the product into a generic nutrition platform
- Add diagnosis, treatment, disease prediction, lifespan prediction, or medical advice
- Invent unnecessary microservices
- Add mobile app, payment, social networking, wearable integration, or media storage unless explicitly requested
- Share one database across microservices
- Create foreign keys across databases owned by different services
- Add technology only for decoration

## 3. Target Architecture

### External actors
- User
- Admin

### External systems
- Email Provider (e.g. Resend/Brevo/Mailjet)
- AI Provider only when AI recommendation is enabled

### Core containers
- Web Application — ASP.NET Core MVC
- API Gateway — YARP
- Identity Service — ASP.NET Core Web API
- Diet Knowledge Service — ASP.NET Core Web API
- Meal Planning Service — ASP.NET Core Web API
- Tracking Service — ASP.NET Core Web API
- RabbitMQ — Message Broker
- Reminder Worker — .NET Worker Service

### Optional AI container
- Recommendation Service — internal service used by Meal Planning Service through gRPC

## 4. Service Responsibilities

### Identity Service
Owns:
- User
- Role
- Authentication
- JWT token generation
- Basic user profile

Database:
- IdentityDb

### Diet Knowledge Service
Owns:
- DietGuideline
- Food
- Recipe
- RecipeIngredient

Database:
- DietKnowledgeDb

Admin can manage this content.
Users can browse/search/filter this content.

### Meal Planning Service
Owns:
- MealPlan
- MealPlanItem
- MealSchedule

Database:
- MealPlanningDb

Responsibilities:
- Create/update/delete meal plans
- Schedule meals
- Validate or retrieve food/recipe knowledge from another service when required
- Publish reminder-related messages to RabbitMQ

### Tracking Service
Owns:
- DailyTracking
- MealTracking
- ProgressSummary

Database:
- TrackingDb

Responsibilities:
- Mark planned meals completed/not completed
- Store daily tracking
- Calculate progress/history

### Reminder Worker
Responsibilities:
- Consume reminder messages from RabbitMQ
- Process scheduled reminders
- Call external Email Provider over HTTPS

Do not make Reminder Worker a business REST API unless explicitly needed.

### Recommendation Service (optional)
Responsibilities:
- Receive recommendation requests from Meal Planning Service using gRPC
- Use only approved Diet Knowledge plus user preferences
- Call an external AI Provider if configured
- Return suggested meal plans/recipes
- Never diagnose or provide medical treatment advice
- Never write directly to MealPlanningDb

## 5. Communication Rules

### Client to backend
Web Application -> API Gateway:
- HTTPS / REST

API Gateway -> REST microservices:
- HTTP / REST

### Internal synchronous communication
When AI recommendation is enabled:
- Meal Planning Service -> Recommendation Service: gRPC

If Meal Planning Service must read food/recipe data directly:
- Prefer a small explicit service API; do not access DietKnowledgeDb directly.

### Asynchronous communication
Meal Planning Service -> RabbitMQ:
- Publish reminder-related messages

RabbitMQ -> Reminder Worker:
- Consume messages

Reminder Worker -> Email Provider:
- HTTPS

## 6. Layered Architecture Inside REST Services
Each REST microservice should use exactly these logical layers:

API
-> Services
-> Repository

Recommended project structure per service:
- ServiceName.Api
- ServiceName.Application
- ServiceName.Infrastructure
- ServiceName.Domain

Mapping:
- API: Controllers, request/response models, auth entry points
- Application: use cases/business services/interfaces
- Infrastructure: EF Core DbContext, repositories, external implementations
- Domain: entities/domain models

Keep dependencies one-directional.
Do not let Controllers call DbContext directly.

## 7. Database Rules
Use PostgreSQL and database-per-service.

- IdentityDb
- DietKnowledgeDb
- MealPlanningDb
- TrackingDb

Rules:
- Each service owns its database.
- No service reads another service database directly.
- No cross-database foreign keys.
- Cross-service references use IDs only.
- Use EF Core migrations per service.

## 8. Initial Functional Scope

### User
- Register
- Login
- View profile
- Browse diet guidelines
- Search/filter foods
- Search/filter recipes
- Create/update meal plans
- Mark meal completion
- View daily/weekly progress
- Receive reminder emails
- Request AI meal recommendation only if AI module is enabled

### Admin
- Login
- CRUD Diet Guidelines
- CRUD Foods
- CRUD Recipes
- Activate/deactivate managed content

Do not add admin capabilities unrelated to this scope.

## 9. API Quality Rules
For REST APIs:
- Use RESTful resource naming
- Use DTOs
- Validate input
- Return correct HTTP status codes
- Support pagination for list endpoints
- Add search/filter/sort where appropriate
- Add Swagger/OpenAPI
- Add global error handling
- Do not expose EF entities directly

## 10. Authentication Rules
- JWT Bearer authentication
- Roles: User, Admin
- Admin-only mutation endpoints for managed diet knowledge
- User owns and can access only their own meal plans/tracking data

## 11. Docker Rules
Provide Dockerfiles for deployable services and a docker-compose.yml for local development.

Docker Compose should eventually include:
- api-gateway
- identity-service
- diet-knowledge-service
- meal-planning-service
- tracking-service
- reminder-worker
- rabbitmq
- postgres databases
- recommendation-service only if AI module is enabled

Do not add Kubernetes unless explicitly requested.

## 12. Architecture Documentation
Maintain:
- docs/c4/C0-System-Context.md
- docs/c4/C1-Internal-Architecture.md
- docs/erd/Conceptual-ERD.md
- docs/database/Physical-Database.md

C0 must show:
- User
- Admin
- Longevity Diet Platform
- External Email Provider
- External AI Provider only if used

C1 must show:
- Web Application
- API Gateway
- Internal services
- Databases
- RabbitMQ
- Reminder Worker
- External providers
- Communication type labels such as REST, gRPC, Publish/Consume, HTTPS

Keep diagrams simple: correct, sufficient, no unnecessary text.

## 13. Coding Workflow
Always work incrementally.

Before coding:
1. Read this AGENTS.md.
2. Inspect the repository.
3. State what you will create/change.
4. Do not invent missing requirements.
5. Prefer a minimal correct scaffold over a large speculative implementation.

For each implementation phase:
1. Update architecture/docs if architecture changed.
2. Implement only the requested slice.
3. Build the affected projects.
4. Fix compilation errors.
5. Report created/changed files.
6. Report assumptions and TODOs.

## 14. Phase 1 — Scaffold Only
When asked to "build the initial skeleton", create structure only:
- Solution
- Projects
- Project references
- Basic configuration
- Health endpoints
- Swagger for APIs
- Empty/placeholder layers
- DbContext shells
- Docker Compose skeleton
- RabbitMQ configuration skeleton
- gRPC contract/service skeleton if AI module is enabled
- Documentation folders

Do NOT implement all business features in Phase 1.

## 15. Source of Truth
If a later prompt conflicts with this file, ask for clarification before making a major architecture change.

When requirements change, update this file first so future AI work remains consistent.
