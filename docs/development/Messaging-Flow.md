# Messaging Flow

The platform uses RabbitMQ for asynchronous work. Queue names are the defaults configured in the services.

| Queue | Producer | Consumer | Purpose |
| --- | --- | --- | --- |
| `recommendation.requests` | MealPlanning.Api | Recommendation.Service | Request an AI meal recommendation |
| `recommendation.results` | Recommendation.Service | MealPlanning.Api | Return the generated recommendation |
| `reminder.requests` | MealPlanning.Api | NotificationWorker | Schedule meal reminders (short and long delays) |
| `notification.messages` | Notification.Grpc / Tracking | NotificationWorker | Deliver email notifications |

## Recommendation flow

1. The user asks for a recommendation through the Web app and gateway.
2. MealPlanning saves the request to its outbox table (same transaction as the request); a background dispatcher publishes it to `recommendation.requests`, retrying if RabbitMQ is down.
3. Recommendation.Service builds context from approved knowledge only.
4. If Gemini is enabled and configured it generates the answer; otherwise a safe fallback is returned.
5. The result is published to `recommendation.results` and stored by MealPlanning.

## Reminder flow

1. Adding or rescheduling a meal plan item saves a reminder to the MealPlanning outbox; the dispatcher publishes it to `reminder.requests`.
2. NotificationWorker sends a confirmation mail and parks the reminder in a delay queue until it is due.
3. When due, the worker asks MealPlanning (`/internal/reminders/{itemId}`, key `INTERNAL_API_KEY`) whether the item still exists with that time. Stale reminders are dropped.

## Notification flow

1. Tracking calls Notification.Grpc (internal gRPC).
2. A message is published to `notification.messages`.
3. NotificationWorker sends it through Brevo, or logs it when no real key is set.

## Debugging

- Management UI: `http://localhost:15672` (default `guest` / `guest`).
- Logs: `docker compose logs recommendation-service notification-worker`.
