# C1 - Internal Architecture

```mermaid
flowchart LR
    User[User]
    Admin[Admin]
    Web[Web Application]
    Gateway[API Gateway\nYARP]

    Identity[Identity Service]
    Diet[Diet Knowledge Service]
    Meal[Meal Planning Service]
    Tracking[Tracking Service]
    Recommendation[Recommendation Service]
    Notification[Notification Service]
    Worker[Notification Worker\n.NET Worker Service]
    Rabbit[RabbitMQ\nMessage Broker]

    IdentityDb[(IdentityDb\nPostgreSQL)]
    DietDb[(DietKnowledgeDb\nPostgreSQL)]
    MealDb[(MealPlanningDb\nPostgreSQL)]
    TrackingDb[(TrackingDb\nPostgreSQL)]
    VectorDb[(VectorDb\npgvector)]
    Gemini[Google Gemini\nAI Provider]
    Brevo[Brevo\nEmail Service]

    User -->|HTTPS| Web
    Admin -->|HTTPS| Web
    Web -->|HTTPS| Gateway

    Gateway -->|HTTP / REST| Identity
    Gateway -->|HTTP / REST| Diet
    Gateway -->|HTTP / REST| Meal
    Gateway -->|HTTP / REST| Tracking

    Identity --> IdentityDb
    Diet --> DietDb
    Diet -->|HTTPS| Gemini
    Diet --> VectorDb
    Meal --> MealDb
    Tracking --> TrackingDb

    Meal -->|Publish| Rabbit
    Rabbit -->|Consume| Recommendation
    Recommendation -->|HTTPS| Gemini
    Recommendation -->|Publish| Rabbit
    Rabbit -->|Consume| Meal

    Tracking -->|gRPC| Notification
    Notification -->|Publish| Rabbit
    Rabbit -->|Consume| Worker
    Worker -->|HTTPS| Brevo
```

Recommendation Service and Notification Service are stateless in the current scope and do not own databases. Diet Knowledge Service may call Gemini only for Admin book-source candidate extraction; public user and recommendation flows still rely on approved Diet Knowledge content.

`VectorDb` (PostgreSQL with the pgvector extension, its own container) holds embeddings of active `Approved` guidelines, foods and recipes. Diet Knowledge Service owns it and writes it after startup and on Admin rebuild; other services reach it only through the Diet Knowledge public search API (Recommendation Service uses it to rank approved knowledge by the user's preferences). Embeddings come from Google Gemini when enabled, otherwise from a local fallback.

Local Docker demo ports use HTTP on localhost, but the intended external client-to-platform boundary remains HTTPS in the architecture.
