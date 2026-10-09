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
| `/diet-knowledge` | Diet Knowledge Service | `/api/diet-guidelines` | `/diet-knowledge/api/diet-guidelines` |
| `/diet-knowledge` | Diet Knowledge Service | `/api/foods` | `/diet-knowledge/api/foods` |
| `/diet-knowledge` | Diet Knowledge Service | `/api/recipes` | `/diet-knowledge/api/recipes` |
| `/meal-planning` | Meal Planning Service | `/api/meal-plans` | `/meal-planning/api/meal-plans` |
| `/meal-planning` | Meal Planning Service | `/api/meal-recommendations` | `/meal-planning/api/meal-recommendations` |
| `/tracking` | Tracking Service | `/api/daily-trackings` | `/tracking/api/daily-trackings` |
| `/tracking` | Tracking Service | `/api/progress-summaries` | `/tracking/api/progress-summaries` |

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

Recommendation and notification communication remains internal through RabbitMQ and gRPC as described in the architecture docs.
