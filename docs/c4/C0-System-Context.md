# C0 - System Context

```mermaid
flowchart LR
    User[User]
    Admin[Admin]
    Platform[Longevity Diet Platform]
    Email[External Email Provider]
    AI[External AI Provider]

    User -->|HTTPS / REST| Platform
    Admin -->|HTTPS / REST| Platform
    Platform -->|HTTPS| Email
    Platform -->|HTTPS, optional| AI
```

The platform is educational and planning-focused. It does not provide diagnosis, treatment, disease prediction, lifespan prediction, or medical advice.
