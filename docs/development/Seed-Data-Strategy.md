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
- Seed one demo user account if useful for browser demos.

Implementation target:
- Implemented through `IdentitySeeder`, which applies Identity migrations and seeds `User`/`Admin` roles plus a configurable demo admin account.
- Demo admin settings are supplied through `IdentitySeed__AdminEmail`, `IdentitySeed__AdminPassword`, and `IdentitySeed__AdminDisplayName`.
- The startup seed path retries briefly so Docker Compose can tolerate PostgreSQL startup timing.
- Store demo passwords in configuration for local/demo use; never hard-code production secrets.

## Diet Knowledge Service

Purpose:
- Seed approved book-derived diet guidelines.
- Seed foods compatible with the approved guidelines.
- Seed recipes and recipe ingredients for browsing, meal planning, and recommendation context.

Implementation target:
- Add an idempotent seeding component under Diet Knowledge Infrastructure.
- Keep seed content concise but enough for demo search/filter/recipe flows.
- Mark managed content as active by default unless a demo scenario needs inactive content.

## Meal Planning Service

Purpose:
- Seed only when a demo needs prebuilt sample meal plans.

Implementation target:
- Prefer creating meal plans through APIs during demo setup after Identity and Diet Knowledge are ready.
- If static seed is needed, store only cross-service IDs, not foreign keys.

## Tracking Service

Purpose:
- Seed only when a demo needs sample progress history.

Implementation target:
- Prefer creating tracking records through APIs during demo setup.
- Store planned meal references as IDs only.

## Recommendation And Notification Services

Recommendation Service and Notification Service are stateless in the current scope.

Do not create seed data or databases for them.

## Demo Setup Order

Recommended local/demo setup order:

1. Apply Identity migrations.
2. Seed Identity roles and demo accounts.
3. Apply Diet Knowledge migrations.
4. Seed approved guidelines, foods, and recipes.
5. Apply Meal Planning migrations.
6. Apply Tracking migrations.
7. Create sample meal plans and tracking data through APIs if needed.

## Future Implementation Checklist

- [x] Add Identity seed data after migrations exist.
- [ ] Add Diet Knowledge seed data after CRUD models are finalized.
- [ ] Decide whether Meal Planning needs static sample plans or API-created demo data.
- [ ] Decide whether Tracking needs static sample progress or API-created demo data.
- [x] Document demo account credentials in local-only docs or `.env.example` placeholders.
