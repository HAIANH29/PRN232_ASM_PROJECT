# Configuration Reference

Cấu hình đọc từ `appsettings*.json` và biến môi trường (dấu `__` thay cho `:`). Docker Compose lấy giá trị từ file `.env` (sao chép từ `.env.example`), mỗi biến đều có giá trị mặc định trong `docker-compose.yml`.

## Biến trong `.env`

### Hạ tầng

| Biến | Mặc định | Dùng cho |
| --- | --- | --- |
| `POSTGRES_USER` | `postgres` | Tất cả PostgreSQL |
| `POSTGRES_PASSWORD` | `postgres` | Tất cả PostgreSQL |
| `RABBITMQ_DEFAULT_USER` | `guest` | RabbitMQ |
| `RABBITMQ_DEFAULT_PASS` | `guest` | RabbitMQ |

### Xác thực và dữ liệu demo

| Biến | Mặc định | Ánh xạ |
| --- | --- | --- |
| `JWT_SIGNING_KEY` | `development-only-replace-with-secret` | `Jwt__SigningKey` ở Identity, Diet Knowledge, Meal Planning, Tracking |
| `IDENTITY_ADMIN_EMAIL` | `admin@longevity.local` | `IdentitySeed__AdminEmail` |
| `IDENTITY_ADMIN_PASSWORD` | `Admin@123456` | `IdentitySeed__AdminPassword` |
| `IDENTITY_ADMIN_DISPLAY_NAME` | `Demo Admin` | `IdentitySeed__AdminDisplayName` |
| `IDENTITY_USER_EMAIL` | `user@longevity.local` | `IdentitySeed__UserEmail` |
| `IDENTITY_USER_PASSWORD` | `User@123456` | `IdentitySeed__UserPassword` |
| `IDENTITY_USER_DISPLAY_NAME` | `Demo User` | `IdentitySeed__UserDisplayName` |

### Diet Knowledge

| Biến | Mặc định | Ánh xạ / ý nghĩa |
| --- | --- | --- |
| `DIET_KNOWLEDGE_SEED_INCLUDE_DEMO_APPROVED_CONTENT` | `true` | `DietKnowledgeSeed__IncludeDemoApprovedContent`, nạp tri thức demo đã duyệt |
| `BOOK_KNOWLEDGE_STORAGE_PATH` | `/app/storage/book-sources` | Nơi lưu PDF upload (volume `diet-knowledge-book-sources`) |
| `EMBEDDING_MODEL` | `gemini-embedding-001` | `KnowledgeIndex__EmbeddingModel` |
| `KNOWLEDGE_INDEX_ON_STARTUP` | `true` | `KnowledgeIndex__IndexOnStartup` |

### Google Gemini (tùy chọn)

| Biến | Mặc định | Ý nghĩa |
| --- | --- | --- |
| `GEMINI_ENABLED` | `false` | Bật gọi Gemini |
| `GEMINI_ENDPOINT` | `https://generativelanguage.googleapis.com` | Endpoint API |
| `GEMINI_API_KEY` | `development-only` | Khóa API thật khi bật |
| `GEMINI_MODEL` | `gemini-2.0-flash` | Mô hình sinh nội dung |

Khi `GEMINI_ENABLED=false` hoặc khóa chưa hợp lệ, embedding dùng phương án băm cục bộ và Recommendation trả về kết quả dự phòng an toàn.

### Brevo email (tùy chọn)

| Biến | Mặc định | Ý nghĩa |
| --- | --- | --- |
| `BREVO_BASE_URL` | `https://api.brevo.com/v3` | Endpoint |
| `BREVO_API_KEY` | `development-only` | Khóa thật để gửi email |
| `BREVO_FROM_EMAIL` | (trống) | Email người gửi |
| `BREVO_FROM_NAME` | `Longevity Diet Platform` | Tên người gửi |

Không có khóa thật thì Notification Worker chỉ ghi log nội dung email thay vì gửi.

## Cấu hình theo service

| Service | Khóa chính |
| --- | --- |
| Identity | `ConnectionStrings__IdentityDb`, `Jwt__*`, `IdentitySeed__*` |
| Diet Knowledge | `ConnectionStrings__DietKnowledgeDb`, `ConnectionStrings__VectorDb`, `KnowledgeIndex__*`, `DietKnowledgeSeed__*`, `BookKnowledgeIngestion__StoragePath`, `Gemini__*` |
| Meal Planning | `ConnectionStrings__MealPlanningDb`, `DietKnowledgeService__BaseUrl`, `RabbitMq__HostName`, `Jwt__SigningKey` |
| Tracking | `ConnectionStrings__TrackingDb`, `NotificationService__GrpcAddress`, `Jwt__SigningKey` |
| Recommendation | `RabbitMq__HostName`, `DietKnowledgeService__BaseUrl`, `Gemini__*` |
| Notification gRPC | `RabbitMq__HostName` |
| Notification Worker | `RabbitMq__HostName`, `Brevo__*`, `RabbitMq:ConsumerPrefetchCount` (mặc định 4) |

## Tên hàng đợi RabbitMQ (`RabbitMqOptions`)

| Mục | Exchange | Queue | Routing key |
| --- | --- | --- | --- |
| Nhắc nhở | `longevity.reminders` | `reminder.requests` | `reminder.requested` |
| Yêu cầu gợi ý | `longevity.recommendations` | `recommendation.requests` | `recommendation.requested` |
| Kết quả gợi ý | `longevity.recommendations` | `recommendation.results` | `recommendation.completed` |
| Thông báo | `longevity.notifications` | `notification.messages` | `notification.requested` |

Kết nối mặc định: `localhost:5672`, `guest/guest`.

## Gateway

Route YARP khai báo trong `appsettings.json` của `LongevityDiet.ApiGateway`: mỗi tiền tố (`/identity`, `/diet-knowledge`, `/meal-planning`, `/tracking`) được bỏ tiền tố rồi chuyển tới service tương ứng trong mạng Docker. Xem `Gateway-Routes.md`.

## Quản lý package

Phiên bản package khai báo tập trung trong `Directory.Packages.props`; xem `Api-Conventions.md`.
