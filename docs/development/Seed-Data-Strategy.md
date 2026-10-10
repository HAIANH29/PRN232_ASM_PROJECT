# Seed Data Strategy

Seed data should support local development and teacher demos without breaking service ownership boundaries.

## Principles

- Each service seeds only its own database.
- No service seeds another service database.
- No cross-database foreign keys.
- Cross-service references use IDs only.
- Seed data must be idempotent.
- Demo data must stay inside the approved Longevity Diet educational scope.

## Identity Service

Purpose:
- Seed roles: `User`, `Admin`.
- Seed one demo admin account.
- Seed one demo user account for browser demos.

Implementation target:
- Implemented through `IdentitySeeder`, which applies Identity migrations and seeds `User`/`Admin` roles plus configurable demo admin and demo user accounts.
- Demo admin settings are supplied through `IdentitySeed__AdminEmail`, `IdentitySeed__AdminPassword`, and `IdentitySeed__AdminDisplayName`.
- Demo user settings are supplied through `IdentitySeed__UserEmail`, `IdentitySeed__UserPassword`, and `IdentitySeed__UserDisplayName`.
- The startup seed path retries briefly so Docker Compose can tolerate PostgreSQL startup timing.
- Store demo passwords in configuration for local/demo use; never hard-code production secrets.

## Diet Knowledge Service

Purpose:
- Seed book-derived diet guidelines for review.
- Seed foods compatible with the scoped guidelines.
- Seed recipes and recipe ingredients for browsing, meal planning, and recommendation context.

Implementation target:
- Implemented through `DietKnowledgeSeeder`, which applies Diet Knowledge migrations and loads `book-knowledge-seed.json`.
- Seed content stays concise, summarized in the team's own words, and inside the approved educational Longevity Diet scope.
- Seed content is active but `NeedsReview` by default until the team verifies exact source locations from a legally obtained copy.
- A separate local demo seed file, `demo-approved-knowledge-seed.json`, provides approved teacher-demo data when `DietKnowledgeSeed__IncludeDemoApprovedContent=true`.
- Public browse/search/filter endpoints expose only active `Approved` content.
- Admin endpoints can manage and filter `NeedsReview`, `Approved`, and `Rejected` content.
- Admin PDF ingestion is not seed data. Uploaded PDFs are private local artifacts; extracted chunks and generated candidates are stored in DietKnowledgeDb for review only.
- Generated candidates must be approved into managed Diet Knowledge content before public browse/search, Meal Planning, or Recommendation can use them.
- The startup seed path retries briefly so Docker Compose can tolerate PostgreSQL startup timing.

## Meal Planning Service

Purpose:
- Seed only when a demo needs prebuilt sample meal plans.

Implementation target:
- Prefer creating meal plans through APIs during demo setup after Identity and Diet Knowledge are ready.
- If static seed is needed, store only cross-service IDs, not foreign keys.
- Current implementation applies the `InitialMealPlanningSchema` migration on startup.
- No static meal plan seed is included because meal plans are user-owned and should be created through authenticated APIs.

## Tracking Service

Purpose:
- Seed only when a demo needs sample progress history.

Implementation target:
- Prefer creating tracking records through APIs during demo setup.
- Store planned meal references as IDs only.
- Current implementation applies the `InitialTrackingSchema` migration on startup.
- No static tracking seed is included because tracking records are user-owned and should be created through authenticated APIs.

## Recommendation And Notification Services

Recommendation Service and Notification Service are stateless in the current scope.

Do not create seed data or databases for them.

## Demo Setup Order

Recommended local/demo setup order:

1. Apply Identity migrations.
2. Seed Identity roles and demo accounts.
3. Apply Diet Knowledge migrations.
4. Seed book knowledge review data and approved local demo knowledge when enabled.
5. Optionally upload a legally obtained PDF through Admin Book Sources and generate review candidates.
6. Approve verified Diet Knowledge items through admin APIs, candidate approval, or reviewed seed metadata before demo reset.
7. Apply Meal Planning migrations.
8. Apply Tracking migrations.
9. Create sample meal plans and tracking data through APIs if needed.

## Future Implementation Checklist

- [x] Add Identity seed data after migrations exist.
- [x] Add Diet Knowledge seed data after CRUD models are finalized.
- [x] Add approved local demo knowledge so clean setup has public data immediately.
- [x] Decide whether Meal Planning needs static sample plans or API-created demo data.
- [x] Decide whether Tracking needs static sample progress or API-created demo data.
- [x] Document demo account credentials in local-only docs or `.env.example` placeholders.
