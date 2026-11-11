# API Gateway Routes

The API Gateway is the single backend entry point for the Web Application and external API clients.

Base URL in Docker Compose:

- `http://localhost:5001`

Base URL inside Docker Compose service-to-service calls:

- `http://api-gateway:8080`

## Public REST Routes

| Gateway prefix | Downstream service | Example downstream path | Example gateway path |
| --- | --- | --- | --- |
| `/identity` | Identity Service | `/api/auth/login` | `/identity/api/auth/login` |
| `/identity` | Identity Service | `/api/auth/profile` | `/identity/api/auth/profile` |
| `/identity` | Identity Service | `PUT /api/auth/profile/preferences` | `/identity/api/auth/profile/preferences` |
| `/diet-knowledge` | Diet Knowledge Service | `/api/diet-guidelines` | `/diet-knowledge/api/diet-guidelines` |
| `/diet-knowledge` | Diet Knowledge Service | `/api/foods` | `/diet-knowledge/api/foods` |
| `/diet-knowledge` | Diet Knowledge Service | `/api/recipes` | `/diet-knowledge/api/recipes` |
| `/diet-knowledge` | Diet Knowledge Service | `/api/categories` | `/diet-knowledge/api/categories` |
| `/diet-knowledge` | Diet Knowledge Service | `/api/knowledge/search` | `/diet-knowledge/api/knowledge/search` |
| `/diet-knowledge` | Diet Knowledge Service (Admin) | `/api/admin/knowledge-index/status`, `/rebuild` | `/diet-knowledge/api/admin/knowledge-index/status` |
| `/diet-knowledge` | Diet Knowledge Service | `/api/diet-regimens` | `/diet-knowledge/api/diet-regimens` |
| `/diet-knowledge` | Diet Knowledge Service (Admin) | `/api/admin/diet-regimens` | `/diet-knowledge/api/admin/diet-regimens` |
| `/meal-planning` | Meal Planning Service | `/api/meal-plans` | `/meal-planning/api/meal-plans` |
| `/meal-planning` | Meal Planning Service | `/api/meal-recommendations` | `/meal-planning/api/meal-recommendations` |
| `/tracking` | Tracking Service | `/api/daily-trackings` | `/tracking/api/daily-trackings` |
| `/tracking` | Tracking Service | `/api/progress-summaries` | `/tracking/api/progress-summaries` |
| `/tracking` | Tracking Service | `/api/diet-trackings` | `/tracking/api/diet-trackings` |

The gateway removes the public prefix before proxying the request to the downstream service.

## JWT Forwarding

Clients send the same bearer token through the gateway:

```http
Authorization: Bearer <access-token>
```

YARP forwards the `Authorization` header to the downstream service. The public REST services remain responsible for JWT validation and role/ownership checks.

## Internal Services

These services are not routed through the gateway in the current scope:

- Recommendation Service
- Notification Service
- Notification Worker
- RabbitMQ
- PostgreSQL databases

The gateway also answers `404` for any path containing `/internal/` (for example `/meal-planning/internal/reminders/{id}`). Those endpoints are service-to-service only, used by the Notification Worker, and are protected by the shared `InternalApi__Key` as a second layer.

Recommendation and notification communication remains internal through RabbitMQ and gRPC as described in the architecture docs.
