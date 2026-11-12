# Test results

- Run date: 2026-11-12
- Command: `dotnet test tests/LongevityDiet.Core.Tests --logger "console;verbosity=normal"` (the solution has one test project)
- Result: **94 passed, 0 failed, 0 skipped**, total time about 1.3 s
- Full console output of the run: `Test-Results-raw.txt` (one line per test case with its duration)

## Test cases per class

| Class | Cases | Area |
|---|---|---|
| BookKnowledgeIngestionServiceTests | 29 | Book upload, chunking, candidates, approve/reject |
| ApiAuthorizationTests | 12 | JWT roles, User/Admin boundaries |
| ReliabilityTests | 9 | Outbox, stale reminders, planned-meal progress, JWT key |
| DietModuleTests | 8 | Food, recipe, regimen |
| VectorDbTests | 7 | Embeddings, semantic search |
| RecommendationServiceTests | 7 | Gemini and fallback recommendation |
| MealPlanningServiceTests | 7 | Meal plans, reminders, past-date rules |
| UserProfileAndDietTrackingTests | 5 | Profile and diet tracking |
| DietKnowledgeTests | 4 | Public knowledge APIs |
| TrackingServiceTests | 2 | Daily tracking |
| NotificationWorkerTests | 2 | Reminder worker |
| IdentityServiceTests | 2 | Register and login |
| **Total** | **94** | |

Note: counts are executed test cases (a `[Theory]` row counts as one case), so a class can have more cases than test methods.

Known gaps: BR-04 and state transitions have no dedicated tests. The tests use EF InMemory and fakes, not a real PostgreSQL or RabbitMQ.
