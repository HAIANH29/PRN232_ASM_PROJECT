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
2. MealPlanning publishes a message to `recommendation.requests`.
3. Recommendation.Service builds context from approved knowledge only.
4. If Gemini is enabled and configured it generates the answer; otherwise a safe fallback is returned.
5. The result is published to `recommendation.results` and stored by MealPlanning.

## Notification flow

1. Tracking calls Notification.Grpc (internal gRPC).
2. A message is published to `notification.messages`.
3. NotificationWorker sends it through Brevo, or logs it when no real key is set.

## Debugging

- Management UI: `http://localhost:15672` (default `guest` / `guest`).
- Logs: `docker compose logs recommendation-service notification-worker`.
