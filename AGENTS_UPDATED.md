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
- Reminder and notification emails
- AI-assisted recommendation based only on approved diet knowledge and user preferences

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
- Google Gemini — AI Provider
- Resend — Email Service

### Internal containers/services
- Web Application
- API Gateway — YARP
- Identity Service — ASP.NET Core Web API
- Diet Knowledge Service — ASP.NET Core Web API
- Meal Planning Service — ASP.NET Core Web API
- Tracking Service — ASP.NET Core Web API
- Recommendation Service — internal AI orchestration service
- Notification Service — internal notification orchestration service
- RabbitMQ — Message Broker
- Notification Worker — .NET Worker Service

### Databases
- IdentityDb — PostgreSQL
- DietKnowledgeDb — PostgreSQL
- MealPlanningDb — PostgreSQL
- TrackingDb — PostgreSQL

Recommendation Service and Notification Service are stateless in the current scope and do not require their own databases.

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
- Work with approved diet knowledge when creating plans
- Publish reminder messages to RabbitMQ
- Publish AI recommendation requests to RabbitMQ
- Consume AI recommendation results from RabbitMQ when needed

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
- Call Notification Service via gRPC when a progress-related notification is required

### Recommendation Service
Responsibilities:
- Consume recommendation requests from RabbitMQ
- Prepare AI context/prompt from approved project data
- Call Google Gemini over HTTPS
- Publish recommendation results back to RabbitMQ
- Never diagnose or provide medical treatment advice
- Never write directly to MealPlanningDb
- No database in the current scope

### Notification Service
Responsibilities:
- Receive notification requests from Tracking Service via gRPC
- Prepare notification messages
- Publish notification messages to RabbitMQ
- No notification history/read-unread feature in the current scope
- No database in the current scope

### Notification Worker
Responsibilities:
- Consume notification/reminder messages from RabbitMQ
- Send email through Resend over HTTPS
- Handle background delivery processing

Do not create a second Reminder Worker. The previous Reminder Worker role is replaced by **Notification Worker**.

## 5. Communication Rules

### Client to backend
Web Application -> API Gateway:
- HTTPS / REST

API Gateway -> public business REST services:
- HTTP / REST

Public business services behind the gateway:
- Identity Service
- Diet Knowledge Service
- Meal Planning Service
- Tracking Service

Recommendation Service and Notification Service are internal services and do not need direct public routes unless explicitly required later.

### Internal synchronous communication
Tracking Service -> Notification Service:
- **gRPC**

Use this as the project’s required gRPC communication flow.

### Internal asynchronous communication

#### AI Recommendation
Meal Planning Service -> RabbitMQ:
- Publish `RecommendationRequest`

RabbitMQ -> Recommendation Service:
- Consume `RecommendationRequest`

Recommendation Service -> Google Gemini:
- HTTPS

Recommendation Service -> RabbitMQ:
- Publish `RecommendationResult`

RabbitMQ -> Meal Planning Service:
- Consume `RecommendationResult`

#### Reminder / Notification
Meal Planning Service -> RabbitMQ:
- Publish meal reminder messages

Tracking Service -> Notification Service:
- gRPC

Notification Service -> RabbitMQ:
- Publish notification messages

RabbitMQ -> Notification Worker:
- Consume notification/reminder messages

Notification Worker -> Resend:
- HTTPS

## 6. Communication Intent
Use each communication style for a clear reason:

- **HTTPS**: public/external communication that must be protected in transit
- **HTTP/REST**: normal synchronous API communication behind the gateway
- **gRPC**: internal synchronous service-to-service communication where a typed contract is useful
- **RabbitMQ**: asynchronous communication where the sender should not wait for the receiver to finish

Do not use a technology just to satisfy a checklist.

## 7. Layered Architecture Inside REST Services
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

## 8. Database Rules
Use PostgreSQL and database-per-service.

- IdentityDb
- DietKnowledgeDb
- MealPlanningDb
- TrackingDb

Rules:
- Each stateful business service owns its database.
- No service reads another service database directly.
- No cross-database foreign keys.
- Cross-service references use IDs only.
- Use EF Core migrations per service.
- Do not create RecommendationDb or NotificationDb unless a concrete future requirement needs persistence.

## 9. Initial Functional Scope

### User
- Register
- Login
- View profile
- Browse diet guidelines
- Search/filter foods
- Search/filter recipes
- Create/update meal plans
- Request AI-assisted meal recommendations
- Mark meal completion
- View daily/weekly progress
- Receive reminder/notification emails

### Admin
- Login
- CRUD Diet Guidelines
- CRUD Foods
- CRUD Recipes
- Activate/deactivate managed content

Do not add admin capabilities unrelated to this scope.

## 10. AI Recommendation Rules
AI is an enhancement, not the source of truth.

Recommendation must be based on:
- Approved knowledge derived from *The Longevity Diet*
- Foods/recipes available in the system
- User preferences supplied by the user

Google Gemini must not be used as a generic medical/nutrition authority.

Never implement:
- diagnosis
- treatment advice
- disease-specific medical recommendations
- lifespan prediction

The final meal plan is owned and saved by Meal Planning Service only after the user accepts/edits the recommendation.

## 11. API Quality Rules
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

## 12. Authentication Rules
- JWT Bearer authentication
- Roles: User, Admin
- Admin-only mutation endpoints for managed diet knowledge
- User owns and can access only their own meal plans/tracking data

## 13. Docker Rules
Provide Dockerfiles for deployable services and a docker-compose.yml for local development.

Docker Compose should eventually include:
- web
- api-gateway
- identity-service
- diet-knowledge-service
- meal-planning-service
- tracking-service
- recommendation-service
- notification-service
- notification-worker
- rabbitmq
- postgres databases

External providers are not Docker containers:
- Google Gemini
- Resend

Do not add Kubernetes unless explicitly requested.

## 14. Architecture Documentation
Maintain:
- docs/c4/C0-System-Context.md
- docs/c4/C1-Internal-Architecture.md
- docs/erd/Conceptual-ERD.md
- docs/database/Physical-Database.md

### C0 — System Context
Show only:
- User
- Admin
- Longevity Diet Platform
- Google Gemini [AI Provider]
- Resend [Email Service]

Do not show internal microservices/databases/RabbitMQ in C0.

### C1 — Internal Architecture
Show:
- Web Application
- API Gateway [YARP]
- Identity Service
- Diet Knowledge Service
- Tracking Service
- Meal Planning Service
- Recommendation Service
- Notification Service
- 4 PostgreSQL databases
- RabbitMQ [Message Broker]
- Notification Worker [.NET Worker Service]
- Google Gemini [AI Provider]
- Resend [Email Service]

Keep relationship labels short:
- HTTPS
- HTTP / REST
- gRPC
- Publish
- Consume

Keep diagrams simple: correct, sufficient, no unnecessary text.

## 15. Main Business Flow

### Normal usage
1. User/Admin accesses Web Application.
2. Web Application calls API Gateway.
3. Gateway routes REST requests to the appropriate business service.
4. Each service reads/writes only its own database.

### AI recommendation
1. User requests an AI-assisted meal recommendation.
2. Request reaches Meal Planning Service.
3. Meal Planning Service publishes a recommendation request to RabbitMQ.
4. Recommendation Service consumes the request.
5. Recommendation Service calls Google Gemini over HTTPS.
6. Recommendation Service publishes the recommendation result to RabbitMQ.
7. Meal Planning Service consumes the result.
8. User reviews/edits the recommendation.
9. Accepted meal plan is saved in MealPlanningDb.

### Progress notification
1. User records meal completion.
2. Tracking Service updates progress in TrackingDb.
3. When a notification is needed, Tracking Service calls Notification Service via gRPC.
4. Notification Service publishes a notification message to RabbitMQ.
5. Notification Worker consumes the message.
6. Notification Worker calls Resend over HTTPS.
7. Resend sends the email to the user.

### Meal reminder
1. Meal Planning Service publishes a reminder message to RabbitMQ.
2. Notification Worker consumes the reminder message.
3. Notification Worker sends the reminder email through Resend.

## 16. Coding Workflow
Always work incrementally.

Before coding:
1. Read this AGENTS.md.
2. Read PROJECT_PROGRESS.md to understand what has already been built, what is incomplete, and what the next expected work is.
3. Inspect the repository.
4. State what you will create/change.
5. Do not invent missing requirements.
6. Prefer a minimal correct scaffold over a large speculative implementation.

For each implementation phase:
1. Update architecture/docs if architecture changed.
2. Implement only the requested slice.
3. Build the affected projects.
4. Fix compilation errors.
5. Report created/changed files.
6. Report assumptions and TODOs.
7. Update PROJECT_PROGRESS.md in the same change set with completed work, remaining work, verification results, and any new assumptions/TODOs.

### Project progress tracking
Maintain PROJECT_PROGRESS.md at the repository root as the shared progress tracker for humans and AI agents.

Rules:
- Every agent must read PROJECT_PROGRESS.md before making project changes.
- Every completed change must update PROJECT_PROGRESS.md before commit/push or before reporting completion.
- Keep progress entries factual and concise: date, branch/commit when known, completed work, verification, remaining work, and blockers.
- Do not mark a feature complete unless the code, documentation, and verification for that feature are actually done.
- If a task changes architecture, update both the architecture docs and PROJECT_PROGRESS.md.

## 17. Phase 1 — Scaffold Only
When asked to "build the initial skeleton", create structure only:
- Solution
- Projects
- Project references
- Basic configuration
- Health endpoints
- Swagger for public REST APIs
- Empty/placeholder layers
- DbContext shells for the 4 persistent services
- Docker Compose skeleton
- RabbitMQ contracts/configuration skeleton
- Recommendation request/result message contracts
- Tracking -> Notification gRPC contract skeleton
- Notification message contract
- Documentation folders

Do NOT implement all business features in Phase 1.
Do NOT call real Gemini or Resend in Phase 1 unless explicitly requested.

## 18. Source of Truth
If a later prompt conflicts with this file, ask for clarification before making a major architecture change.

When requirements change, update this file first so future AI work remains consistent.
