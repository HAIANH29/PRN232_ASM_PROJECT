# Submission Checklist

Use this checklist before handing in the repository.

## Required Verification

- [ ] `dotnet restore LongevityDietPlatform.sln` passes.
- [ ] `dotnet build LongevityDietPlatform.sln` passes.
- [ ] `dotnet test LongevityDietPlatform.sln` passes.
- [ ] `docker compose config --quiet` passes.
- [ ] `docker compose up -d --build` starts the full stack.
- [ ] Web opens at `http://localhost:5000`.
- [ ] API Gateway route summary opens at `http://localhost:5001/routes`.
- [ ] RabbitMQ Management opens at `http://localhost:15672`.

## Demo Accounts

- [ ] Admin login works: `admin@longevity.local` / `Admin@123456`.
- [ ] User login works: `user@longevity.local` / `User@123456`.
- [ ] Admin-only pages reject normal users.
- [ ] Protected user pages reject unauthenticated visitors.

## Functional Demo

- [ ] User can browse approved Diet Guidelines.
- [ ] User can search/filter Foods.
- [ ] User can search/filter Recipes.
- [ ] Admin can create/update/deactivate managed Diet Knowledge content.
- [ ] Admin can open Book Sources.
- [ ] Admin book-source upload/chunk/candidate workflow is explained or demonstrated with a legal sample PDF.
- [ ] User can create a meal plan.
- [ ] User can add meal plan items.
- [ ] Invalid food/recipe references are rejected by Meal Planning.
- [ ] User can request an AI-assisted recommendation.
- [ ] Recommendation result is returned through RabbitMQ flow.
- [ ] User can accept a recommendation into a meal plan.
- [ ] User can mark a meal completed/not completed.
- [ ] Progress summary updates correctly.
- [ ] Progress completion triggers Tracking -> Notification gRPC and RabbitMQ notification flow.
- [ ] Notification Worker logs or sends the expected email payload.

## Architecture Requirements

- [ ] Microservices are present and documented.
- [ ] Public REST APIs are implemented for Identity, Diet Knowledge, Meal Planning, and Tracking.
- [ ] Public REST APIs use Swagger and health endpoints.
- [ ] Each REST service follows API -> Application -> Infrastructure -> Domain layering.
- [ ] JWT Bearer authentication is used.
- [ ] Admin mutations require Admin role.
- [ ] User-owned Meal Planning and Tracking data is protected by user id.
- [ ] Search/filter/sort/pagination are demonstrated where appropriate.
- [ ] gRPC flow is Tracking Service -> Notification Service.
- [ ] RabbitMQ async flows are demonstrated.
- [ ] Notification Worker is a .NET Worker Service.
- [ ] PostgreSQL database-per-service is preserved.
- [ ] No service reads another service database directly.
- [ ] No cross-database foreign keys are introduced.
- [ ] Recommendation Service has no database.
- [ ] Notification Service has no database.

## Documentation

- [ ] README run instructions match the current project.
- [ ] `docs/c4/C0-System-Context.md` matches the external system context.
- [ ] `docs/c4/C1-Internal-Architecture.md` matches the internal services.
- [ ] `docs/erd/Conceptual-ERD.md` matches current domain entities.
- [ ] `docs/database/Physical-Database.md` matches current EF migrations/tables.
- [ ] `docs/development/Database-Docker-Demo-Setup.md` can be followed by another person.
- [ ] `docs/submission/Demo-Script.md` can be followed during presentation.
- [ ] `PROJECT_PROGRESS.md` is updated.
- [ ] `PROJECT_SCHEDULE.md` is updated.

## Book And AI Safety

- [ ] Uploaded book PDFs are not committed to git.
- [ ] Raw PDF chunks are not exposed to normal users.
- [ ] Recommendation context uses only active `Approved` managed knowledge.
- [ ] AI output avoids diagnosis, treatment advice, disease prediction, and lifespan prediction.
- [ ] Real book-derived content has source metadata before being marked `Approved`.
- [ ] The team can explain that the app is educational/planning-focused, not medical advice.

## Git

- [ ] Working tree is clean.
- [ ] Latest branch is pushed to GitHub.
- [ ] Commit history includes the final documentation package.
