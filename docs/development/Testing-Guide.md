# Testing Guide

All automated tests live in `tests/LongevityDiet.Core.Tests`.

## Run

```powershell
dotnet test LongevityDietPlatform.sln
```

Run a single test class:

```powershell
dotnet test tests/LongevityDiet.Core.Tests --filter "FullyQualifiedName~TrackingServiceTests"
```

## Coverage by area

| Area | Test file |
| --- | --- |
| API authorization | `ApiAuthorizationTests.cs` |
| Identity | `IdentityServiceTests.cs` |
| Diet knowledge | `DietKnowledgeTests.cs`, `DietModuleTests.cs` |
| Book ingestion | `BookKnowledgeIngestionServiceTests.cs` |
| Vector search | `VectorDbTests.cs` |
| Meal planning | `MealPlanningServiceTests.cs` |
| Reliability (outbox, stale reminders, planned-meal progress, JWT key rules) | `ReliabilityTests.cs` |
| Tracking | `TrackingServiceTests.cs`, `UserProfileAndDietTrackingTests.cs` |
| Recommendation | `RecommendationServiceTests.cs` |
| Notification worker | `NotificationWorkerTests.cs` |

Shared fakes are in `TestDoubles.cs`.

## Writing new tests

- Name tests `Method_Scenario_ExpectedResult`.
- Prefer the shared test doubles over real databases or RabbitMQ.
- Cover both the allowed and the forbidden case for authorization changes.
