# C0 - System Context

```mermaid
flowchart LR
    User[User]
    Admin[Admin]
    Platform[Longevity Diet Platform]
    Gemini[Google Gemini\nAI Provider]
    Resend[Resend\nEmail Service]

    User -->|HTTPS| Platform
    Admin -->|HTTPS| Platform
    Platform -->|HTTPS| Gemini
    Platform -->|HTTPS| Resend
```

The platform is educational and planning-focused. It does not provide diagnosis, treatment, disease prediction, lifespan prediction, or medical advice.

The diagram intentionally omits internal microservices, databases, and RabbitMQ. Those are shown in `C1-Internal-Architecture.md`.
