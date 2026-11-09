# Service and Port Map

Host ports exposed by `docker-compose.yml` for local development.

## Applications

| Service | Host port | Container port | Purpose |
| --- | --- | --- | --- |
| `web` | 5000 | 8080 | ASP.NET Core MVC web application |
| `api-gateway` | 5001 | 8080 | YARP reverse proxy in front of the REST APIs |
| `identity-service` | 5101 | 8080 | Users, roles, JWT, eating-preference profiles |
| `diet-knowledge-service` | 5102 | 8080 | Approved knowledge, semantic search, book ingestion |
| `meal-planning-service` | 5103 | 8080 | Meal plans, reminders, AI recommendation requests |
| `tracking-service` | 5104 | 8080 | Diet, daily and meal tracking, progress |
| `notification-service` | 5105 | 8081 | Internal gRPC notification service |
| `recommendation-service` | - | - | RabbitMQ consumer, no host port |
| `notification-worker` | - | - | RabbitMQ consumer, no host port |

## Infrastructure

| Service | Host port | Notes |
| --- | --- | --- |
| `rabbitmq` | 5672 / 15672 | AMQP / management UI |
| `identity-db` | 5433 | PostgreSQL `IdentityDb` |
| `diet-knowledge-db` | 5434 | PostgreSQL for DietKnowledge |
| `meal-planning-db` | 5435 | PostgreSQL for MealPlanning |
| `tracking-db` | 5436 | PostgreSQL for Tracking |
| `vector-db` | 5437 | PostgreSQL with pgvector (`VectorDb`) |

## Rules of thumb

- Browsers and the Web app talk to the gateway (5001); direct service ports are for debugging.
- Each service owns its own database. No cross-database queries.
- If a port is already in use, change the left side of the mapping in `docker-compose.yml`.
