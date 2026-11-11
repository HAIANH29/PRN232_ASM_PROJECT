# Cấu trúc hệ thống Longevity Diet Platform (bản chi tiết)

Tài liệu này giúp người mới đọc hiểu: **hệ thống gồm những phần nào, mỗi phần làm gì, các phần nói chuyện với nhau bằng cách nào, và cấu hình nằm ở đâu trong code.**

Cách đọc: mỗi khái niệm đều có 3 phần ngắn:

- **Giải thích đơn giản**: hiểu ý chính, không cần biết lập trình sâu.
- **Dự án dùng ở đâu**: ví dụ cụ thể trong dự án.
- **Cấu hình ở đâu**: đường dẫn file và số dòng (`đường/dẫn/file.cs:dòng`). Số dòng đúng tại thời điểm viết, nếu code đổi thì dòng có thể lệch vài dòng, hãy tìm theo tên hàm hoặc tên cấu hình.

Mọi đường dẫn tính từ thư mục gốc `PRN232_ASM_PROJECT/`.

---

## Mục lục

1. Bức tranh tổng quan
2. Các khái niệm nền tảng (giải thích đơn giản)
3. Cấu trúc thư mục và các lớp code
4. Từng service: vai trò, entity, API, cấu hình
5. Bảng giao tiếp giữa các phần
6. RabbitMQ chi tiết: queue nào, khai báo ở đâu
7. Hệ thống cấu hình: biến môi trường đi từ đâu đến đâu
8. Các luồng nghiệp vụ từng bước
9. Quy tắc thời gian khi lập kế hoạch bữa ăn
10. "Muốn sửa X thì xem ở đâu" (bảng tra nhanh)
11. Những điểm lệch giữa tài liệu cũ và code

---

## 1. Bức tranh tổng quan

### 1.1 Ví dụ cho dễ hình dung: một nhà hàng lớn

| Trong nhà hàng | Trong dự án |
| --- | --- |
| Khách hàng | Người dùng và Admin (trình duyệt) |
| Quầy tiếp tân | **Web** (giao diện MVC): khách nói chuyện với tiếp tân |
| Lễ tân điều phối, chỉ đường tới đúng bộ phận | **API Gateway** (YARP) |
| Các bộ phận: quản lý tài khoản, thư viện công thức, lên thực đơn, theo dõi tiến độ | **4 service REST**: Identity, DietKnowledge, MealPlanning, Tracking |
| Bộ phận nghiên cứu món (chậm, làm nền) | **Recommendation Service** (gọi AI Gemini) |
| Bộ phận gửi thư | **Notification Service** + **Notification Worker** (gửi email qua Brevo) |
| Hộp thư nội bộ để các bộ phận gửi việc cho nhau | **RabbitMQ** |
| Mỗi bộ phận có tủ hồ sơ riêng, không ai lục tủ của ai | **Database riêng cho từng service** |

### 1.2 Danh sách 15 container

Hệ thống chạy bằng Docker Compose (`docker-compose.yml`). Mỗi dòng dưới đây là một "chương trình" chạy riêng.

| Container | Code nằm ở | Làm gì | Cổng máy bạn | Cổng trong container |
| --- | --- | --- | --- | --- |
| `web` | `src/Web/LongevityDiet.Web` | Giao diện web (ASP.NET Core MVC), song ngữ EN/VI | 5000 | 8080 |
| `api-gateway` | `src/Gateways/LongevityDiet.ApiGateway` | Cổng vào duy nhất cho API, chuyển tiếp tới service | 5001 | 8080 |
| `identity-service` | `src/Services/Identity/*` | Đăng ký, đăng nhập, cấp JWT | 5101 | 8080 |
| `diet-knowledge-service` | `src/Services/DietKnowledge/*` | Kho kiến thức ăn uống, tải sách PDF, duyệt kiến thức | 5102 | 8080 |
| `meal-planning-service` | `src/Services/MealPlanning/*` | Kế hoạch bữa ăn, nhắc nhở, yêu cầu gợi ý AI | 5103 | 8080 |
| `tracking-service` | `src/Services/Tracking/*` | Đánh dấu đã ăn, tính tiến độ | 5104 | 8080 |
| `recommendation-service` | `src/Services/Recommendation/LongevityDiet.Recommendation.Service` | Nhận yêu cầu gợi ý từ RabbitMQ, gọi Gemini, trả kết quả | không mở | không mở |
| `notification-service` | `src/Services/Notification/LongevityDiet.Notification.Grpc` | Nhận lời gọi gRPC từ Tracking, đẩy tin vào RabbitMQ | 5105 (chỉ health) | 8080 gRPC, 8081 health |
| `notification-worker` | `src/Workers/LongevityDiet.NotificationWorker` | Lấy tin từ RabbitMQ, hẹn giờ, gửi email Brevo | không mở | không mở |
| `rabbitmq` | image `rabbitmq:3-management` | Hộp thư trung gian | 5672, 15672 (trang quản trị) | |
| `identity-db`, `diet-knowledge-db`, `meal-planning-db`, `tracking-db` | image `postgres:16` | 4 database nghiệp vụ | 5433–5436 | 5432 |
| `vector-db` | image `pgvector/pgvector:pg16` | Database vector (tìm kiếm theo ngữ nghĩa) | 5437 | 5432 |

### 1.3 Sơ đồ giao tiếp

```
 Trình duyệt ──HTTP──► Web ──HTTP + JWT──► API Gateway ──HTTP (chuyển tiếp)──► Identity
   (:5000)              ApiGatewayClient     (:5001)       /identity            DietKnowledge
                                                            /diet-knowledge     MealPlanning
                                                            /meal-planning      Tracking
                                                            /tracking

 MealPlanning ──HTTP──► DietKnowledge          (lấy danh mục món/thực phẩm)
 Tracking ──gRPC──► Notification.Grpc ──publish──► [notification.messages] ──► NotificationWorker ──HTTPS──► Brevo (email)
 MealPlanning ──publish (qua Outbox)──► [reminder.requests] ──────────────────► NotificationWorker
 NotificationWorker ──HTTP + X-Internal-Key──► MealPlanning /internal/reminders/{id}   (hỏi: nhắc nhở còn hợp lệ không?)

 MealPlanning ──publish (qua Outbox)──► [recommendation.requests] ──► Recommendation.Service
 Recommendation.Service ──HTTP──► DietKnowledge   (lấy kiến thức đã duyệt)
 Recommendation.Service ──HTTPS──► Gemini         (tùy chọn)
 Recommendation.Service ──publish──► [recommendation.results] ──► MealPlanning (nhận kết quả)

 DietKnowledge ──HTTPS──► Gemini    (tóm tắt candidate, tạo embedding)
 DietKnowledge ──Npgsql──► VectorDb (lưu và tìm vector)
```

Ba nguyên tắc nền của kiến trúc:

1. **Mỗi service một database riêng**, không đọc chéo, không khóa ngoại chéo, chỉ lưu ID của service khác.
2. **Web chỉ gọi Gateway**, không gọi trực tiếp từng service.
3. **Recommendation, Notification, Worker không đi qua Gateway** (chỉ dùng nội bộ).

---

## 2. Các khái niệm nền tảng (giải thích đơn giản)

### 2.1 HTTP và HTTPS

**Giải thích đơn giản:** HTTP là cách trình duyệt "hỏi và nhận trả lời" từ máy chủ, giống gọi món và nhận món. HTTPS là HTTP nhưng **có khóa**: nội dung bị mã hóa nên người đứng giữa không đọc được mật khẩu hay token.

**Dự án dùng ở đâu:**
- Trình duyệt → Web, Web → Gateway, Gateway → service, service → service: đều là HTTP.
- Gọi ra dịch vụ bên ngoài (Gemini, Brevo) dùng **HTTPS** vì dữ liệu đi qua Internet.
- Khi chạy Docker trên máy, các lời gọi nội bộ dùng HTTP thường (cho đơn giản). HTTPS giữa người dùng và hệ thống là thiết kế mục tiêu khi triển khai thật.

**Cấu hình ở đâu:**
- Địa chỉ Gateway mà Web gọi: `src/Web/LongevityDiet.Web/appsettings.json` mục `ApiGateway:BaseUrl` (đang là `http://api-gateway:8080`).
- Gemini: biến `GEMINI_ENDPOINT` (mặc định `https://generativelanguage.googleapis.com`), xem `docker-compose.yml:66` và `:124`.
- Brevo: `BREVO_BASE_URL` (mặc định `https://api.brevo.com/v3`), `docker-compose.yml:153`; giá trị mặc định trong code ở `src/Workers/LongevityDiet.NotificationWorker/BrevoOptions.cs:7`.
- Web chuyển hướng sang HTTPS khi triển khai thật: `src/Web/LongevityDiet.Web/Program.cs` (`app.UseHttpsRedirection()`).

### 2.2 REST API (dạng "HTTP có quy tắc")

**Giải thích đơn giản:** REST là cách đặt tên địa chỉ cho rõ nghĩa. Ví dụ `GET /api/foods` = "cho tôi xem danh sách thực phẩm", `POST /api/meal-plans` = "tạo một kế hoạch mới", `DELETE .../{id}` = "xóa".

**Dự án dùng ở đâu:** 4 service Identity, DietKnowledge, MealPlanning, Tracking. Mỗi service có các `Controller` (lớp nhận lời gọi) trong thư mục `Controllers/` của project `.Api`.

**Cấu hình ở đâu:** đường dẫn của mỗi API nằm ngay trên controller, ví dụ `[Route("api/meal-plans")]` trong `src/Services/MealPlanning/LongevityDiet.MealPlanning.Api/Controllers/`. Danh sách route đầy đủ: `docs/development/Gateway-Routes.md`.

### 2.3 API Gateway (YARP)

**Giải thích đơn giản:** như **lễ tân chỉ đường**. Bạn chỉ cần biết một địa chỉ (cổng 5001). Gateway nhìn đầu đường dẫn (`/identity/...`, `/meal-planning/...`) rồi chuyển tới đúng service, bỏ phần đầu đi. YARP là thư viện của Microsoft làm việc này.

**Dự án dùng ở đâu:**
- `/identity/api/auth/login` → chuyển thành `/api/auth/login` gửi tới Identity.
- Gateway còn làm "bảo vệ": mọi đường dẫn có `/internal/` bị chặn, trả 404, để endpoint nội bộ không lộ ra ngoài.

**Cấu hình ở đâu:**
- Bảng route (đường dẫn → cluster, kèm `PathRemovePrefix`): `src/Gateways/LongevityDiet.ApiGateway/appsettings.json:10` (`Routes`).
- Bảng cluster (cluster → địa chỉ service): cùng file, dòng 56 (`Clusters`), ví dụ `http://identity-service:8080/`.
- Bật YARP và nạp cấu hình: `src/Gateways/LongevityDiet.ApiGateway/Program.cs:4-5`.
- Chặn `/internal/`: `Program.cs:31`. Bật proxy: `Program.cs:41`.
- Danh sách route hiển thị ở trang `/routes` và `/`: cùng `Program.cs`.

### 2.4 JWT (thẻ ra vào) và phân quyền

**Giải thích đơn giản:** đăng nhập xong, Identity phát cho bạn một **thẻ ra vào** (JWT) có chữ ký. Mỗi lần gọi API, bạn đưa thẻ ra. Các service khác không cần hỏi lại Identity: chúng tự kiểm tra chữ ký bằng **cùng một chìa khóa bí mật**. Trong thẻ có ghi bạn là ai (ID, email) và vai trò (`User` hoặc `Admin`).

**Dự án dùng ở đâu:**
- Identity phát thẻ khi đăng nhập thành công.
- Web lưu thẻ trong phiên (`UserSession`) và gửi kèm mỗi lời gọi lên Gateway.
- Endpoint admin có `[Authorize(Roles = "Admin")]`; endpoint của người dùng có `[Authorize]`, và service chỉ trả dữ liệu của chính người đó.
- Gọi service-to-service **không dùng JWT** (vì không có người đứng sau): Recommendation gọi API công khai; Worker gọi MealPlanning bằng khóa nội bộ `X-Internal-Key`.

**Cấu hình ở đâu:**
- Tạo thẻ: `src/Services/Identity/LongevityDiet.Identity.Infrastructure/Authentication/JwtTokenGenerator.cs` (ký ở dòng 29, gắn vai trò ở dòng 26). Issuer/Audience/SigningKey đọc ở `src/Services/Identity/LongevityDiet.Identity.Infrastructure/DependencyInjection.cs:23-27`.
- Kiểm tra thẻ ở mọi service: `src/BuildingBlocks/LongevityDiet.ApiDefaults/Extensions/LongevityApiDefaultsExtensions.cs:71` (`AddJwtBearer`), Issuer/Audience đọc ở dòng 73-74.
- Dừng service nếu khóa thiếu, quá ngắn hoặc là khóa công khai: `src/BuildingBlocks/LongevityDiet.ApiDefaults/Extensions/JwtSigningKeyResolver.cs:15`.
- Chính sách `AdminOnly`: `src/Services/Identity/LongevityDiet.Identity.Api/Program.cs:14`.
- Khóa bí mật chung: biến `JWT_SIGNING_KEY` trong `.env`, đưa vào từng service ở `docker-compose.yml` (`Jwt__SigningKey`, dòng 38, 61, 86, 107).
- Khóa nội bộ giữa Worker và MealPlanning: `INTERNAL_API_KEY` (`docker-compose.yml:90` và `:152`); kiểm tra ở `src/Services/MealPlanning/LongevityDiet.MealPlanning.Api/Controllers/InternalRemindersController.cs:42-44`.

### 2.5 gRPC

**Giải thích đơn giản:** cũng là cách gọi service khác, nhưng thay vì gửi chữ (JSON), nó gửi dữ liệu **dạng nhị phân gọn nhẹ** theo một **hợp đồng** viết sẵn trong file `.proto`. Giống như hai bên cùng cầm một mẫu đơn có đánh số ô: ai điền sai ô là lỗi ngay khi biên dịch. Nó nhanh và chặt chẽ, nhưng trình duyệt không gọi được, nên chỉ dùng **giữa các service với nhau**.

**Dự án dùng ở đâu:** duy nhất một luồng: **Tracking → Notification**. Khi người dùng đạt mốc tiến độ, Tracking gọi hàm `RequestProgressNotification` của Notification, rồi Notification đẩy tin vào RabbitMQ. (Một phần lý do chọn gRPC ở đây là đề bài yêu cầu có ít nhất một luồng gRPC nội bộ, `AGENTS.md` dòng 14.)

**Cấu hình ở đâu:**
- Hợp đồng: `src/Services/Notification/LongevityDiet.Notification.Grpc/Protos/notification.proto` (service `Notification`, hàm `RequestProgressNotification`).
- Server bật gRPC: `src/Services/Notification/LongevityDiet.Notification.Grpc/Program.cs:6` (`AddGrpc`) và `:11` (`MapGrpcService`).
- Cổng và giao thức server: `src/Services/Notification/LongevityDiet.Notification.Grpc/appsettings.json` mục `Kestrel:Endpoints`: endpoint `Grpc` là `http://+:8080` giao thức `Http2` (dòng 11-14), endpoint `Health` là `http://+:8081` giao thức `Http1` (dòng 15-18).
- Phía gọi (client): `src/Services/Tracking/LongevityDiet.Tracking.Infrastructure/Notification/GrpcNotificationClient.cs`, địa chỉ đọc từ `NotificationService:GrpcAddress`.
- Địa chỉ trong Docker: `docker-compose.yml:109` (`NotificationService__GrpcAddress: http://notification-service:8080`).
- Đăng ký client vào hệ thống: `src/Services/Tracking/LongevityDiet.Tracking.Infrastructure/DependencyInjection.cs:24`.
- Email người nhận lấy từ thẻ JWT: `src/Services/Tracking/LongevityDiet.Tracking.Api/Controllers/CurrentUser.cs:20`.

### 2.6 RabbitMQ (hộp thư trung gian)

**Giải thích đơn giản:** thay vì gọi điện đứng chờ, bạn **bỏ thư vào hộp**, đi làm việc khác. Người kia rảnh thì lấy thư ra làm. Hộp thư giữ thư an toàn kể cả khi người nhận đang tắt.

| Từ | Nghĩa |
| --- | --- |
| **Message** | Một tờ thư (gói dữ liệu nhỏ) |
| **Queue** | Ngăn hộp thư, thư xếp hàng theo thứ tự |
| **Producer / Publish** | Người bỏ thư vào |
| **Consumer / Consume** | Người lấy thư ra xử lý |
| **Exchange + routing key** | "Bưu cục" và "địa chỉ" để đưa thư vào đúng ngăn |
| **Dead-letter queue** | Ngăn thư hỏng: thư lỗi nhiều lần được chuyển sang đây để không kẹt hàng |

**Vì sao dùng:** gọi Gemini mất vài giây; gửi email và nhắc bữa ăn không cần chờ. Dùng RabbitMQ thì người dùng bấm xong là được trả lời ngay, việc nặng chạy nền, và thư không mất khi một service tạm sập.

**Dự án dùng ở đâu:** 4 queue, xem bảng ở mục 6 (có kèm file khai báo và file gửi/nhận).

**Cấu hình ở đâu:** giá trị mặc định (host, cổng, tên exchange/queue/routing key) nằm ở `src/BuildingBlocks/LongevityDiet.Contracts/Messaging/RabbitMqOptions.cs`; từng service có thể ghi đè trong `appsettings.json` (mục `RabbitMq`) hoặc biến môi trường `RabbitMq__*`. Chi tiết mục 6.

### 2.7 Outbox (đảm bảo không mất thư)

**Giải thích đơn giản:** có một rủi ro: bạn lưu món ăn vào database xong, định bỏ thư vào RabbitMQ thì RabbitMQ đang sập. Món đã lưu, thư thì mất, nên không có lời nhắc. Cách chữa: **ghi cái thư vào một bảng trong cùng database, cùng lúc với món ăn** (hoặc cả hai thành công, hoặc cả hai thất bại). Rồi một tiến trình chạy nền đọc bảng đó và bỏ thư vào RabbitMQ; lỗi thì thử lại sau. Giống viết thư và cho vào hộp nhà mình trước, người đưa thư sẽ mang ra bưu điện.

**Dự án dùng ở đâu:** MealPlanning dùng cho 2 loại tin: nhắc bữa ăn và yêu cầu gợi ý AI.

**Cấu hình ở đâu:**
- Bảng `OutboxMessages`: entity `src/Services/MealPlanning/LongevityDiet.MealPlanning.Domain/Entities/OutboxMessage.cs`, đăng ký trong `MealPlanningDbContext`.
- Ghi vào bảng: `src/Services/MealPlanning/LongevityDiet.MealPlanning.Infrastructure/Messaging/EfMealPlanOutbox.cs`.
- Tiến trình nền đọc bảng và publish: `.../Messaging/OutboxDispatcher.cs` (mỗi lần lấy tối đa theo `batchSize`, thử lại với thời gian chờ tăng dần tối đa 300 giây, dòng 86-87) và `.../Messaging/OutboxDispatcherWorker.cs` (lô 50 tin, dòng 15; giữ 1 ngày, dòng 16; dọn mỗi giờ, dòng 35).
- Bật tiến trình nền: `src/Services/MealPlanning/LongevityDiet.MealPlanning.Infrastructure/DependencyInjection.cs:36`.

### 2.8 Database riêng cho từng service, EF Core và Migration

**Giải thích đơn giản:** mỗi service giữ "tủ hồ sơ" riêng (một database PostgreSQL riêng). Muốn biết dữ liệu của service khác thì phải **hỏi service đó** chứ không được mở tủ của nó. Vì vậy trong database chỉ lưu **ID** của thứ thuộc service khác (ví dụ Tracking lưu `MealPlanItemId`, không có khóa ngoại sang MealPlanning).

Code truy cập database bằng **Entity Framework Core (EF Core)**: mỗi bảng là một lớp C# (**Entity**), tập hợp các bảng nằm trong một lớp `DbContext`. **Migration** là các tệp ghi lại "thay đổi cấu trúc bảng", chạy tự động khi service khởi động.

**Cấu hình ở đâu:**
- Chuỗi kết nối: biến `ConnectionStrings__IdentityDb`, `__DietKnowledgeDb`, `__VectorDb`, `__MealPlanningDb`, `__TrackingDb` trong `docker-compose.yml` (dòng 37, 57, 58, 84, 106).
- Đọc chuỗi kết nối và đăng ký DbContext: `MealPlanning.Infrastructure/DependencyInjection.cs:20-21`, `Tracking.Infrastructure/DependencyInjection.cs:19-20`, `Identity.Infrastructure/DependencyInjection.cs:18`, `DietKnowledge.Infrastructure/DependencyInjection.cs:21` (DietKnowledgeDb) và `:138` (VectorDb, bật pgvector ở dòng 144 `UseVector()`).
- Tự chạy migration khi khởi động (`MigrateAsync`): `Identity.Infrastructure/Seed/IdentitySeeder.cs:19`, `DietKnowledge.Infrastructure/Seed/DietKnowledgeSeeder.cs:26`, `MealPlanning.Infrastructure/Seed/MealPlanningDatabaseInitializer.cs:11`, `Tracking.Infrastructure/Seed/TrackingDatabaseInitializer.cs:11`, VectorDb: `DietKnowledge.Infrastructure/VectorDb/PgVectorKnowledgeStore.cs:14`.
- Lệnh `Initialize...Async()` gọi các việc trên ở `Program.cs` của từng service.
- DietKnowledge có **hai** DbContext, nên lệnh `dotnet ef` phải thêm `--context`.

### 2.9 Dependency Injection (DI) và Hosted Service

**Giải thích đơn giản:**
- **DI**: thay vì mỗi lớp tự "tạo" đồ nó cần, có một "kho trung tâm" đăng ký sẵn ("ai cần `IMealPlanRepository` thì đưa cho nó `MealPlanRepository`") rồi tự trao khi cần. Muốn đổi cách làm chỉ sửa chỗ đăng ký. Test cũng dễ vì có thể đưa hàng giả.
- **Hosted Service**: một tiến trình chạy nền suốt đời service, ví dụ vòng lặp "lắng nghe RabbitMQ" hoặc "đọc outbox mỗi vài giây".

**Dự án dùng ở đâu và cấu hình ở đâu:** mỗi service có một file `DependencyInjection.cs` làm "danh sách đăng ký". Muốn biết service dùng những thành phần gì, mở file này:

| Service | File đăng ký | Dòng đáng chú ý |
| --- | --- | --- |
| MealPlanning | `src/Services/MealPlanning/LongevityDiet.MealPlanning.Infrastructure/DependencyInjection.cs` | publisher RabbitMQ (31-33), outbox (34-36), HTTP client tới DietKnowledge (39), worker nhận kết quả AI (46) |
| Tracking | `src/Services/Tracking/LongevityDiet.Tracking.Infrastructure/DependencyInjection.cs` | client gRPC (24) |
| Identity | `src/Services/Identity/LongevityDiet.Identity.Infrastructure/DependencyInjection.cs` | JWT (23-27), băm mật khẩu (48) |
| DietKnowledge | `src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Infrastructure/DependencyInjection.cs` | Gemini (54-75, 89, 113), chọn embedding (119-127), VectorDb (134-147) |
| Recommendation | `src/Services/Recommendation/LongevityDiet.Recommendation.Service/Program.cs` | HTTP client DietKnowledge/Gemini, `RecommendationMessageWorker` |
| NotificationWorker | `src/Workers/LongevityDiet.NotificationWorker/Program.cs` | HTTP client Brevo/MealPlanning, `NotificationMessageWorker` |

Phần Application của mỗi service đăng ký ở `DependencyInjection.cs` ngay trong project `.Application`, ví dụ `src/Services/MealPlanning/LongevityDiet.MealPlanning.Application/DependencyInjection.cs`.

### 2.10 Biến môi trường kiểu `A__B`

**Giải thích đơn giản:** cấu hình trong `appsettings.json` có dạng lồng nhau, ví dụ `Jwt` → `SigningKey`. Khi muốn ghi đè từ bên ngoài (Docker, `.env`), người ta viết gộp bằng **hai dấu gạch dưới**: `Jwt__SigningKey`. Trong code đọc lại là `configuration["Jwt:SigningKey"]`. Cùng một thứ, hai cách viết.

Chi tiết cách cấu hình đi từ `.env` đến code: mục 7.

---

## 3. Cấu trúc thư mục và các lớp code

```
PRN232_ASM_PROJECT/
├─ LongevityDietPlatform.sln       Solution gom mọi project
├─ Directory.Packages.props        Phiên bản NuGet quản lý tập trung
├─ global.json                     Cố định .NET SDK 9.0.318
├─ docker-compose.yml              Khai báo toàn bộ container, cổng, biến môi trường
├─ .env / .env.example             Giá trị biến môi trường (.env không commit)
├─ src/
│  ├─ BuildingBlocks/              Thư viện dùng chung
│  │  ├─ LongevityDiet.ApiDefaults   Khuôn chung cho mọi API: JWT, Swagger, lỗi, health
│  │  └─ LongevityDiet.Contracts     Định dạng tin nhắn RabbitMQ + RabbitMqOptions
│  ├─ Gateways/LongevityDiet.ApiGateway
│  ├─ Services/
│  │  ├─ Identity/        .Api  .Application  .Domain  .Infrastructure
│  │  ├─ DietKnowledge/   .Api  .Application  .Domain  .Infrastructure
│  │  ├─ MealPlanning/    .Api  .Application  .Domain  .Infrastructure
│  │  ├─ Tracking/        .Api  .Application  .Domain  .Infrastructure
│  │  ├─ Recommendation/  LongevityDiet.Recommendation.Service   (worker, không DB)
│  │  └─ Notification/    LongevityDiet.Notification.Grpc        (gRPC, không DB)
│  ├─ Workers/LongevityDiet.NotificationWorker
│  └─ Web/LongevityDiet.Web
├─ tests/LongevityDiet.Core.Tests    Toàn bộ test (xunit, EF InMemory, đồ giả viết tay)
└─ docs/                             Tài liệu: c4, erd, database, development, submission
```

### 3.1 Bốn lớp của mỗi service REST

**Giải thích đơn giản:** giống nhà bếp chia khu vực. Mỗi lớp chỉ làm việc của mình và chỉ "nhìn" lớp bên dưới.

| Lớp | Vai trò | Ví dụ trong MealPlanning |
| --- | --- | --- |
| `Api` | **Quầy nhận order**: controller nhận lời gọi, đổi qua lại giữa JSON và dữ liệu nội bộ, cấu hình khởi động | `Controllers/MealPlansController.cs`, `Contracts/*`, `Program.cs`, `appsettings.json` |
| `Application` | **Bếp trưởng**: chứa luật nghiệp vụ, không biết database hay RabbitMQ cụ thể | `Services/MealPlanningService.cs`, `Common/MealSchedulePolicy.cs`, `Abstractions/` (các "hợp đồng") |
| `Domain` | **Công thức gốc**: các Entity thuần, không phụ thuộc gì | `Entities/MealPlan.cs`, `MealPlanItem.cs` |
| `Infrastructure` | **Kho và ngoại giao**: làm việc thật với database, RabbitMQ, HTTP, gRPC | `Persistence/MealPlanningDbContext.cs`, `Repositories/`, `Messaging/`, `DependencyInjection.cs` |

Hướng phụ thuộc: `Api → Application → Domain` và `Infrastructure → Application, Domain`. Hai quy tắc quan trọng: controller **không** chạm trực tiếp `DbContext`, và **không** trả Entity ra ngoài mà trả DTO (gói dữ liệu riêng cho API).

Các `Abstractions/` trong Application là "hợp đồng" (interface), phần làm thật nằm ở Infrastructure. Ví dụ `IReminderPublisher` (Application) được làm thật bởi `RabbitMqReminderPublisher` (Infrastructure); đăng ký ghép đôi ở `DependencyInjection.cs:31`.

### 3.2 Khuôn chung cho 4 service REST (`ApiDefaults`)

`src/BuildingBlocks/LongevityDiet.ApiDefaults/Extensions/LongevityApiDefaultsExtensions.cs`:
- `AddLongevityPublicApiDefaults(...)` (dòng 18): đăng ký controller, Swagger có ô nhập Bearer, kiểm tra JWT (dòng 71), `AddAuthorization` (dòng 88), health check (dòng 68), định dạng lỗi validation thống nhất.
- `UseLongevityPublicApiDefaults()` (dòng 93): bắt lỗi toàn cục → Swagger → xác thực → phân quyền → `/health` → map controller.

Định dạng trả lời chung (`ApiResponse` gồm `succeeded`, `data`, `message`, `errors`, `traceId`) ở `src/BuildingBlocks/LongevityDiet.ApiDefaults/Api/`; bắt lỗi toàn cục ở `Middleware/GlobalExceptionHandlingMiddleware.cs`.

Cả 4 service có `Program.cs` cùng một khuôn:

```
AddXxxApplication()  →  AddXxxInfrastructure(configuration)  →  AddLongevityPublicApiDefaults(...)
→  InitializeXxxAsync()  (chạy migration + seed)  →  UseLongevityPublicApiDefaults()  →  Run
```

Ví dụ: `src/Services/MealPlanning/LongevityDiet.MealPlanning.Api/Program.cs`.

---

## 4. Từng service: vai trò, entity, API, cấu hình

### 4.1 Identity (`IdentityDb`)

**Vai trò (nói đơn giản):** "bảo vệ cổng", biết ai là ai. Đăng ký, đăng nhập, cấp thẻ JWT, lưu sở thích ăn uống của từng người.

**Entity** (`src/Services/Identity/LongevityDiet.Identity.Domain/Entities/`; DbContext: `.../Infrastructure/Persistence/IdentityDbContext.cs`):

| Entity | Trường chính | Quan hệ |
| --- | --- | --- |
| `User` | Email, NormalizedEmail, DisplayName, PasswordHash, IsActive | n–n `Role`; 1–1 `UserPreferenceProfile` |
| `Role` | Name, NormalizedName | n–n `User` (có `User`, `Admin`) |
| `UserPreferenceProfile` | UserId, PreferenceTags, Notes | 1–1 `User` |

**API:** `AuthController` → `api/auth`: `register`, `login`, `profile`, `profile/preferences`, `admin-check`.

**Cấu hình riêng:**
- Tài khoản mẫu: biến `IdentitySeed__*` (`docker-compose.yml:40-45`), đọc bởi `Identity.Infrastructure/Seed/IdentitySeeder.cs`.
- Băm mật khẩu: `Identity.Infrastructure/Authentication/Pbkdf2PasswordHasher.cs` (đăng ký ở `DependencyInjection.cs:48`).

### 4.2 DietKnowledge (`DietKnowledgeDb` + `VectorDb`)

**Vai trò:** "thư viện kiến thức". Lưu danh mục thực phẩm, công thức, nguyên tắc ăn uống, chế độ ăn. Admin tải sách PDF/TXT lên, hệ thống cắt thành đoạn, nhờ Gemini tóm tắt thành "ứng viên", Admin duyệt thì mới thành kiến thức chính thức. Còn lập chỉ mục vector để tìm theo ý nghĩa.

**Entity** (`src/Services/DietKnowledge/LongevityDiet.DietKnowledge.Domain/Entities/`; DbContext: `.../Infrastructure/Persistence/DietKnowledgeDbContext.cs`):

| Entity | Trường chính | Quan hệ |
| --- | --- | --- |
| `Category` | Name, CategoryType, IsActive | nhóm phân loại cho Guideline/Food/Recipe |
| `DietGuideline` | Title, Summary, Source*, ReviewStatus, IsActive | n–1 `Category` |
| `Food` | Name, CompatibilityNotes, Source*, ReviewStatus | n–1 `Category`; 1–n `NutritionFact` |
| `NutritionFact` | ServingSize, EnergyKcal, ProteinG, CarbohydrateG, FatG, FiberG | n–1 `Food` |
| `Recipe` | Name, Description, Source*, ReviewStatus | 1–n `RecipeIngredient`, `RecipeStep` |
| `RecipeIngredient` | QuantityText | n–1 `Recipe`, n–1 `Food` |
| `RecipeStep` | StepNumber, Instruction | n–1 `Recipe` |
| `DietRegimen` | Name, Description, DurationDays | 1–n `DietRegimenGuideline`, `DietRegimenMeal` |
| `DietRegimenGuideline` | SortOrder, Note | bảng nối `DietRegimen` ↔ `DietGuideline` |
| `DietRegimenMeal` | DayNumber, MealSlot, Notes | n–1 `DietRegimen`, n–1 `Recipe` |
| `BookSourceDocument` | OriginalFileName, StoragePath, Status, ChunkCount, UploadedBy | 1–n `BookSourceChunk`, `KnowledgeCandidate` |
| `BookSourceChunk` | ChunkIndex, PageNumber, Text, TokenEstimate | n–1 Document |
| `KnowledgeCandidate` | CandidateType, Title, Summary, Status, GeneratedBy, ApprovedKnowledgeId | n–1 Document, n–1 Chunk |

`ReviewStatus` có giá trị như `Approved`, `NeedsReview`. **Chỉ nội dung `Approved` và `IsActive` mới ra API công khai** và vào ngữ cảnh gợi ý AI. Đoạn chữ thô và file PDF chỉ Admin thấy.

**VectorDb** (`.../Infrastructure/VectorDb/VectorDbContext.cs`, vector 768 chiều): `KnowledgeEmbedding` (SourceType, SourceId, ContentHash, Content, `Embedding`, EmbeddingModel) và `EmbeddingIndexRun` (Status, ItemsIndexed, ItemsFailed).

**API:**
- Công khai (chỉ `Approved`): `api/categories`, `api/diet-guidelines`, `api/diet-regimens`, `api/foods`, `api/recipes`, `api/knowledge/search`.
- Chỉ Admin: `api/admin/diet-guidelines|diet-regimens|foods|recipes`, `api/admin/book-sources` (tải sách, tạo/duyệt ứng viên), `api/admin/knowledge-candidates` (duyệt/từ chối), `api/admin/knowledge` (xóa toàn bộ).

**Cấu hình riêng:**
- Gemini: `Gemini__Enabled/Endpoint/ApiKey/Model` (`docker-compose.yml:65-68`), nạp ở `DietKnowledge.Infrastructure/DependencyInjection.cs:54-75`. Tắt Gemini thì dùng dữ liệu giả cho ứng viên.
- Embedding: chọn Gemini hay bản nội bộ (`LocalHashEmbeddingGenerator`) ở `DependencyInjection.cs:119-127`.
- Chỉ mục vector: `KnowledgeIndex__EmbeddingModel`, `KnowledgeIndex__IndexOnStartup` (`docker-compose.yml:59-60`); chạy lúc khởi động bởi `KnowledgeIndexStartupService` (`DependencyInjection.cs:147`).
- Nơi lưu PDF: `BookKnowledgeIngestion__StoragePath` + volume `diet-knowledge-book-sources` (`docker-compose.yml:64` và mục `volumes` của service).
- Dữ liệu mẫu: `DietKnowledgeSeed__IncludeDemoApprovedContent`; chi tiết `docs/development/Seed-Data-Strategy.md`.

### 4.3 MealPlanning (`MealPlanningDb`)

**Vai trò:** "người lên thực đơn". Người dùng tạo kế hoạch bữa ăn theo ngày/buổi, đặt nhắc nhở, yêu cầu gợi ý từ AI rồi chấp nhận gợi ý để biến thành kế hoạch.

**Entity** (`src/Services/MealPlanning/LongevityDiet.MealPlanning.Domain/Entities/`; DbContext: `.../Infrastructure/Persistence/MealPlanningDbContext.cs`):

| Entity | Trường chính | Quan hệ |
| --- | --- | --- |
| `MealPlan` | UserId, DietRegimenId?, Name, StartDate, EndDate | 1–n `MealPlanItem` |
| `MealPlanItem` | PlannedDate, MealSlot, RecipeId?, FoodId?, Notes, ReminderAtUtc | n–1 `MealPlan` |
| `MealRecommendationRequest` | UserId, PreferenceTagsJson, Days, Status, SuggestedMealTitlesJson, Disclaimer, AcceptedMealPlanId? | độc lập |
| `OutboxMessage` | MessageType, PayloadJson, NextAttemptAtUtc, Attempts, LastError, ProcessedAtUtc | độc lập (xem 2.7) |

`DietRegimenId`, `RecipeId`, `FoodId` chỉ là **ID tham chiếu** sang DietKnowledge. Không có bảng "lịch bữa ăn" riêng: lịch chính là `PlannedDate` + `MealSlot` + `ReminderAtUtc` của `MealPlanItem`.

**API:** `api/meal-plans` (+ `/{id}/items`), `api/meal-recommendations` (+ `/{id}/accept`); nội bộ: `internal/reminders/{mealPlanItemId}`. Tất cả `[Authorize]`, người dùng chỉ thấy dữ liệu của mình.

**Cấu hình riêng:**
- Địa chỉ DietKnowledge: `DietKnowledgeService__BaseUrl` (`docker-compose.yml:85`).
- RabbitMQ: `RabbitMq__HostName` (`docker-compose.yml:88`), mục `RabbitMq` trong `src/Services/MealPlanning/LongevityDiet.MealPlanning.Api/appsettings.json:19`.
- Múi giờ người dùng: `MealPlanning__UtcOffsetMinutes` (`docker-compose.yml:89`), đọc ở `MealPlanning.Infrastructure/DependencyInjection.cs:24-28` (xem mục 9).
- Khóa nội bộ: `InternalApi__Key` (`docker-compose.yml:90`).
- Luật nghiệp vụ nằm ở `src/Services/MealPlanning/LongevityDiet.MealPlanning.Application/Services/MealPlanningService.cs`.

### 4.4 Tracking (`TrackingDb`)

**Vai trò:** "người ghi sổ ăn uống". Người dùng đánh dấu đã ăn bữa nào, hệ thống tính tiến độ (đã ăn / dự định) và báo qua email.

**Entity** (`src/Services/Tracking/LongevityDiet.Tracking.Domain/Entities/`; DbContext: `.../Infrastructure/Persistence/TrackingDbContext.cs`):

| Entity | Trường chính | Quan hệ |
| --- | --- | --- |
| `UserDietTracking` | UserId, DietRegimenId?, MealPlanId?, StartDate, EndDate?, Status | 1–n `DailyTracking`, `ProgressSummary` |
| `DailyTracking` | UserId, TrackingDate, Notes, PlannedMealCount? | n–1 `UserDietTracking`; 1–n `MealTracking` |
| `MealTracking` | MealPlanItemId, IsCompleted, CompletedAtUtc? | n–1 `DailyTracking` |
| `ProgressSummary` | PeriodStartDate/EndDate, PlannedMeals, CompletedMeals, LastNotificationId?, LastNotificationSentAtUtc? | n–1 `UserDietTracking` |

**API:** `api/daily-trackings` (+ `/{date}/meals/{mealPlanItemId}`), `api/progress-summaries`, `api/diet-trackings`. Tất cả `[Authorize]`.

**Cấu hình riêng:** gRPC tới Notification: `NotificationService__GrpcAddress` (`docker-compose.yml:109`). Logic báo tiến độ: `src/Services/Tracking/LongevityDiet.Tracking.Application/Services/TrackingService.cs` (gọi `RequestProgressNotificationAsync` quanh dòng 178).

### 4.5 Recommendation Service (không có database)

**Vai trò:** "người nghiên cứu món". Nhận yêu cầu từ RabbitMQ, đọc kiến thức **đã duyệt** từ DietKnowledge, nhờ Gemini viết gợi ý (hoặc dùng kết quả an toàn dự phòng), gửi kết quả về RabbitMQ.

**Cấu hình:**
- Đăng ký HTTP client và worker: `src/Services/Recommendation/LongevityDiet.Recommendation.Service/Program.cs`.
- Gemini: `GeminiOptions.cs` (mặc định model `gemini-2.0-flash`, timeout 30s, `MaxOutputTokens` 512); biến `Gemini__*` (`docker-compose.yml:123-126`).
- Địa chỉ DietKnowledge: `DietKnowledgeServiceOptions.cs:7`, biến `DietKnowledgeService__BaseUrl` (`docker-compose.yml:122`).
- Hai API công khai mà nó gọi: `HttpApprovedKnowledgeClient.cs` (guideline/food/recipe) và `HttpSemanticKnowledgeSearch.cs` (`/api/knowledge/search`).
- Luật an toàn nội dung (không chẩn đoán/điều trị): `RecommendationSafetyPolicy.cs`.
- Lắng nghe RabbitMQ: `RecommendationMessageWorker.cs` (`BasicConsume` dòng 59, publish kết quả dòng 265 và 330). Thử lại: `RabbitMq:MaxDeliveryAttempts`/`RetryDelaySeconds` trong `appsettings.json:18-19` (kiểm tra lại nếu file đổi).

### 4.6 Notification Service (gRPC, không có database)

**Vai trò:** "bưu cục nhận đơn gửi thư". Nhận lời gọi gRPC từ Tracking và đẩy tin nhắn vào RabbitMQ để Worker gửi.

**Cấu hình:** xem 2.5. Code: `src/Services/Notification/LongevityDiet.Notification.Grpc/Services/NotificationGrpcService.cs` (nhận gRPC) và `RabbitMqNotificationPublisher.cs` (publish, dòng 64). RabbitMQ: `docker-compose.yml:140`, mục `RabbitMq` trong `appsettings.json:21-28`.

### 4.7 Notification Worker (không có database)

**Vai trò:** "người đưa thư". Lấy hai loại tin từ RabbitMQ: **tin thông báo** (gửi ngay) và **tin nhắc nhở** (hẹn giờ, gửi đúng lúc). Gửi email qua Brevo.

**Cách hẹn giờ (đơn giản):** với tin nhắc nhở chưa đến giờ, Worker gửi một email xác nhận ngay, rồi cho tin vào **queue chờ** (có thời gian sống ngắn). Hết thời gian chờ, tin tự quay lại hàng chính để kiểm tra lại. Còn xa giờ thì dùng queue chờ dài (mặc định 1 giờ), gần giờ thì dùng queue chờ ngắn (mặc định 30 giây). Tới giờ, Worker **hỏi MealPlanning** xem bữa ăn đó còn tồn tại đúng giờ đó không, còn thì gửi, không thì bỏ.

**Cấu hình:**
- Code chính: `src/Workers/LongevityDiet.NotificationWorker/NotificationMessageWorker.cs` (xử lý nhắc nhở quanh dòng 175-285; tạo queue chờ dòng 150-175; `BasicConsume` dòng 95; publish dòng 469).
- Thời gian chờ queue: `RabbitMq:ReminderShortDelaySeconds` (mặc định 30) và `RabbitMq:ReminderLongDelaySeconds` (mặc định 3600). Hai giá trị này **không có trong `appsettings.json`**, chỉ là mặc định trong code (`NotificationMessageWorker.cs:157-158` và `:229`); muốn đổi thì đặt biến `RabbitMq__ReminderShortDelaySeconds` / `RabbitMq__ReminderLongDelaySeconds` cho container `notification-worker`.
- Thử lại và số tin lấy cùng lúc: `MaxDeliveryAttempts: 3`, `RetryDelaySeconds: 5`, `ConsumerPrefetchCount: 4` (`src/Workers/LongevityDiet.NotificationWorker/appsettings.json:8-22`).
- Hỏi MealPlanning: `ReminderValidityChecker.cs` (gắn header `X-Internal-Key`, dòng 38); địa chỉ `MealPlanningService__BaseUrl` và khóa `InternalApi__Key` (`docker-compose.yml:151-152`).
- Gửi email: `BrevoEmailSender.cs`, cấu hình `BrevoOptions.cs`; biến `Brevo__BaseUrl/ApiKey/FromEmail/FromName` (`docker-compose.yml:153-156`). Chưa có key thật hoặc `FromEmail` rỗng thì chỉ ghi log, không gửi.

### 4.8 API Gateway và Web

- **Gateway:** xem 2.3.
- **Web:** `src/Web/LongevityDiet.Web`. Controller: `Auth`, `Home`, `Knowledge`, `Regimens`, `MealPlans`, `Recommendations`, `Tracking`, `AdminKnowledge`, `AdminBookSources`, `AdminRegimens`, `Language`, `App`. Web giữ JWT trong `Services/UserSession.cs` và gọi Gateway qua `Services/ApiGatewayClient.cs` (gắn header Bearer ở dòng 103). Song ngữ EN/VI: `Services/Translations.cs`, `Translator.cs`, `ContentTranslations.cs`, chọn ngôn ngữ bằng cookie (`Controllers/LanguageController.cs`). Chuỗi mới hiển thị cho người dùng phải có đủ bản EN và VI.

---

## 5. Bảng giao tiếp giữa các phần

| # | Từ | Đến | Phương thức | Nội dung | Code / cấu hình |
| --- | --- | --- | --- | --- | --- |
| 1 | Trình duyệt | Web | HTTP | Trang MVC | `Web/Program.cs` |
| 2 | Web | ApiGateway | HTTP + `Authorization: Bearer <JWT>` | Mọi lời gọi API | `ApiGateway:BaseUrl` trong `Web/appsettings.json`; `Web/Program.cs:18-24`; `Web/Services/ApiGatewayClient.cs:103` |
| 3 | ApiGateway | 4 service REST | HTTP (YARP, bỏ prefix) | `/identity`, `/diet-knowledge`, `/meal-planning`, `/tracking` | `ApiGateway/appsettings.json:10` (Routes), `:56` (Clusters); `ApiGateway/Program.cs:4-5` |
| 4 | MealPlanning | DietKnowledge | HTTP | Kiểm tra recipe/food tồn tại và đã duyệt | `DietKnowledgeService__BaseUrl`; `MealPlanning.Infrastructure/DependencyInjection.cs:39`; `.../DietKnowledge/HttpDietKnowledgeCatalogClient.cs` |
| 5 | Tracking | Notification.Grpc | **gRPC** (HTTP/2, không TLS) | `RequestProgressNotification` | `NotificationService__GrpcAddress`; `Tracking.Infrastructure/Notification/GrpcNotificationClient.cs`; `notification.proto` |
| 6 | Notification.Grpc | RabbitMQ | **Publish** | `notification.messages` | `Notification.Grpc/Services/RabbitMqNotificationPublisher.cs:64` |
| 7 | MealPlanning | RabbitMQ | **Publish** (qua Outbox) | `reminder.requests`, `recommendation.requests` | `MealPlanning.Infrastructure/Messaging/RabbitMqJsonPublisher.cs:48`, `RabbitMqReminderPublisher.cs`, `RabbitMqRecommendationRequestPublisher.cs`, `OutboxDispatcher*.cs` |
| 8 | RabbitMQ | Recommendation.Service | **Consume** | `recommendation.requests` | `Recommendation.Service/RecommendationMessageWorker.cs:59` |
| 9 | Recommendation.Service | DietKnowledge | HTTP, không JWT (API công khai) | `/api/knowledge/search`, `/api/diet-guidelines`, `/api/foods`, `/api/recipes` | `DietKnowledgeService__BaseUrl`; `Recommendation.Service/Program.cs`; `HttpApprovedKnowledgeClient.cs`, `HttpSemanticKnowledgeSearch.cs:35` |
| 10 | Recommendation.Service | Gemini | HTTPS | `v1beta/models/{model}:generateContent` | `Gemini__*`; `GeminiClient.cs:30` |
| 11 | Recommendation.Service | RabbitMQ | **Publish** | `recommendation.results` | `RecommendationMessageWorker.cs:265, 330` |
| 12 | RabbitMQ | MealPlanning | **Consume** | `recommendation.results` | `MealPlanning.Infrastructure/Messaging/RabbitMqRecommendationResultWorker.cs:102` |
| 13 | RabbitMQ | NotificationWorker | **Consume** | `reminder.requests` và `notification.messages` | `NotificationWorker/NotificationMessageWorker.cs:95` |
| 14 | NotificationWorker | MealPlanning | HTTP + header `X-Internal-Key` | `GET /internal/reminders/{mealPlanItemId}` | `MealPlanningService__BaseUrl`, `InternalApi__Key`; `NotificationWorker/ReminderValidityChecker.cs:38`; `MealPlanning.Api/Controllers/InternalRemindersController.cs` |
| 15 | NotificationWorker | Brevo | HTTPS | Gửi email | `Brevo__*`; `NotificationWorker/BrevoEmailSender.cs` |
| 16 | DietKnowledge | Gemini | HTTPS | `generateContent` (tóm tắt candidate), `embedContent` (embedding) | `Gemini__*`; `DietKnowledge.Infrastructure/DependencyInjection.cs:89, 113`; `Ingestion/GeminiKnowledgeCandidateGenerator.cs:194`; `VectorDb/EmbeddingGenerators.cs:140` |
| 17 | DietKnowledge | VectorDb | Npgsql (EF Core + pgvector) | Lưu và tìm vector | `ConnectionStrings__VectorDb`; `DietKnowledge.Infrastructure/DependencyInjection.cs:138-144`; `VectorDb/PgVectorKnowledgeStore.cs` |

Ghi chú: Gateway chặn mọi đường dẫn `/internal/` (404). Khi Gemini tắt hoặc Brevo chưa có key thật, hệ thống dùng kết quả dự phòng / chỉ ghi log.

---

## 6. RabbitMQ chi tiết

### 6.1 Bốn queue

| Exchange | Queue | Routing key | Ai bỏ thư | Ai lấy thư | Định dạng tin |
| --- | --- | --- | --- | --- | --- |
| `longevity.reminders` | `reminder.requests` | `reminder.requested` | MealPlanning (qua Outbox) | NotificationWorker | `ReminderRequestedMessage` |
| `longevity.recommendations` | `recommendation.requests` | `recommendation.requested` | MealPlanning (qua Outbox) | Recommendation.Service | `RecommendationRequestMessage` |
| `longevity.recommendations` | `recommendation.results` | `recommendation.completed` | Recommendation.Service | MealPlanning | `RecommendationResultMessage` |
| `longevity.notifications` | `notification.messages` | `notification.requested` | Notification.Grpc | NotificationWorker | `NotificationRequestedMessage` |

- Giá trị mặc định của tên exchange/queue/routing key: `src/BuildingBlocks/LongevityDiet.Contracts/Messaging/RabbitMqOptions.cs` (queue ở dòng 17, 23, 27, 33).
- Định dạng tin nhắn: các file `*Message.cs` cùng thư mục `src/BuildingBlocks/LongevityDiet.Contracts/Messaging/`.
- Ghi đè trong từng service: mục `RabbitMq` của `appsettings.json` (MealPlanning dòng 19, Recommendation dòng 8, NotificationWorker dòng 8, Notification dòng 21) hoặc biến `RabbitMq__*` (host: `docker-compose.yml:88, 121, 140, 150`).

### 6.2 Queue được khai báo và dùng ở file nào

| Việc | File |
| --- | --- |
| MealPlanning khai báo exchange/queue và publish | `MealPlanning.Infrastructure/Messaging/RabbitMqJsonPublisher.cs:21` (exchange), `:28` (queue), `:48` (publish) |
| MealPlanning nghe `recommendation.results` | `MealPlanning.Infrastructure/Messaging/RabbitMqRecommendationResultWorker.cs:57-102` |
| Recommendation khai báo cả queue vào/ra, nghe và publish | `Recommendation.Service/RecommendationMessageWorker.cs:79-135` (khai báo), `:59` (nghe) |
| Notification publish | `Notification.Grpc/Services/RabbitMqNotificationPublisher.cs:36-64` |
| NotificationWorker khai báo, nghe, hẹn giờ | `NotificationWorker/NotificationMessageWorker.cs:107-175` (khai báo queue chính, dead-letter, queue chờ), `:95` (nghe), `:469` (publish lại) |
| Kết nối RabbitMQ của MealPlanning | `MealPlanning.Infrastructure/Messaging/RabbitMqConnectionFactory.cs` |

### 6.3 Sơ đồ "gợi ý món bằng AI"

```
Người dùng bấm "Gợi ý"
   │
   ▼
MealPlanning ──bỏ thư──► [recommendation.requests] ──lấy thư──► Recommendation Service
   │ (trả lời ngay:                                                    │ gọi Gemini, mất vài giây
   │  "đã nhận, đang xử lý")                                           ▼
   ◄──────lấy thư─────── [recommendation.results] ◄───bỏ thư─── gửi kết quả
```

---

## 7. Hệ thống cấu hình: biến môi trường đi từ đâu đến đâu

Cùng một giá trị có thể đến từ nhiều nơi. Khi trùng, **nơi đứng sau thắng nơi đứng trước**:

```
1. Giá trị mặc định trong code     (ví dụ BrevoOptions.cs:7, RabbitMqOptions.cs)
2. appsettings.json của service    (ví dụ src/Services/MealPlanning/.../Api/appsettings.json)
3. appsettings.Development.json    (chỉ khi chạy môi trường Development)
4. Biến môi trường (A__B)          (đến từ docker-compose.yml, thắng hết)
```

Khi chạy Docker: **`.env`** (giá trị thật, không commit; mẫu là `.env.example`) → **`docker-compose.yml`** (`${TÊN_BIẾN:-mặc_định}`) → biến môi trường của container (`Gemini__ApiKey`...) → code đọc bằng `configuration["Gemini:ApiKey"]`.

Ví dụ khóa Gemini đi qua đủ 4 chặng: `.env` (`GEMINI_API_KEY`) → `docker-compose.yml:67` và `:125` (`Gemini__ApiKey: ${GEMINI_API_KEY:-development-only}`) → `DietKnowledge.Infrastructure/DependencyInjection.cs:62` và `Recommendation.Service/Program.cs` → `GeminiClient`.

### 7.1 Bảng biến chính

| Mục đích | Biến trong `.env` | Biến vào container (`docker-compose.yml`) | Nơi code đọc |
| --- | --- | --- | --- |
| Tài khoản DB | `POSTGRES_USER`, `POSTGRES_PASSWORD` | trong chuỗi `ConnectionStrings__*` | `AddDbContext(... UseNpgsql(...))` ở các `DependencyInjection.cs` |
| Khóa JWT | `JWT_SIGNING_KEY`, `JWT_ALLOW_DEVELOPMENT_KEY` | `Jwt__SigningKey`, `Jwt__AllowDevelopmentKey` (dòng 38-39, 61-62, 86-87, 107-108) | `JwtSigningKeyResolver.cs`, `LongevityApiDefaultsExtensions.cs` |
| Khóa nội bộ | `INTERNAL_API_KEY` | `InternalApi__Key` (dòng 90, 152) | `InternalRemindersController.cs:42`, `ReminderValidityChecker.cs` |
| Gemini | `GEMINI_ENABLED`, `GEMINI_ENDPOINT`, `GEMINI_API_KEY`, `GEMINI_MODEL` | `Gemini__*` (dòng 65-68, 123-126) | `DietKnowledge.Infrastructure/DependencyInjection.cs:54-75`, `Recommendation.Service/GeminiOptions.cs` |
| Embedding/vector | `EMBEDDING_MODEL`, `KNOWLEDGE_INDEX_ON_STARTUP` | `KnowledgeIndex__*` (dòng 59-60) | `DietKnowledge.Infrastructure/DependencyInjection.cs:119-147` |
| Email | `BREVO_BASE_URL`, `BREVO_API_KEY`, `BREVO_FROM_EMAIL`, `BREVO_FROM_NAME` | `Brevo__*` (dòng 153-156) | `NotificationWorker/BrevoOptions.cs`, `Program.cs` |
| RabbitMQ | `RABBITMQ_DEFAULT_USER/PASS` | `RabbitMq__HostName` (dòng 88, 121, 140, 150) | `RabbitMqOptions.cs` và các worker/publisher |
| Múi giờ lập kế hoạch | `MEAL_PLANNING_UTC_OFFSET_MINUTES` | `MealPlanning__UtcOffsetMinutes` (dòng 9 cho Web, 89 cho MealPlanning) | `MealPlanning.Infrastructure/DependencyInjection.cs:24-28`, `Web/Controllers/RecommendationsController.cs` |
| Tài khoản mẫu | `IDENTITY_*` | `IdentitySeed__*` (dòng 40-45) | `Identity.Infrastructure/Seed/IdentitySeeder.cs` |
| Dữ liệu mẫu kiến thức | `DIET_KNOWLEDGE_SEED_INCLUDE_DEMO_APPROVED_CONTENT` | `DietKnowledgeSeed__IncludeDemoApprovedContent` | `DietKnowledge.Infrastructure/Seed/DietKnowledgeSeeder.cs` |

Tài khoản demo: `admin@longevity.local` / `Admin@123456`, `user@longevity.local` / `User@123456`.

Gemini và Brevo là **tùy chọn** khi phát triển: `GEMINI_ENABLED=false` hoặc `BREVO_API_KEY` để trống/`development-only` thì hệ thống dùng kết quả dự phòng / chỉ ghi log.

---

## 8. Các luồng nghiệp vụ từng bước

### 8.1 Tạo kế hoạch và nhắc nhở bữa ăn

1. Người dùng thêm món vào kế hoạch (Web → Gateway → `MealPlansController` của MealPlanning).
2. `MealPlanningService` kiểm tra luật (ngày/buổi chưa qua, mục 9; món tồn tại và đã duyệt, hỏi DietKnowledge qua HTTP).
3. Lưu `MealPlanItem` **và** một `OutboxMessage` nhắc nhở trong cùng một lần lưu (nếu giờ nhắc còn ở tương lai).
4. `OutboxDispatcherWorker` đọc bảng outbox, publish vào `reminder.requests`.
5. NotificationWorker nhận tin: chưa đến giờ thì gửi email xác nhận rồi cho vào queue chờ; đến giờ thì gọi `/internal/reminders/{id}` hỏi MealPlanning còn hợp lệ không; còn thì gửi email nhắc qua Brevo.
6. Sửa/xóa món trong lúc tin còn chờ: tin cũ tự bị bỏ vì giờ không còn khớp.

### 8.2 Gợi ý món bằng AI

1. Người dùng chọn sở thích, bấm gợi ý. MealPlanning lưu `MealRecommendationRequest` + `OutboxMessage` cùng lúc, trả lời ngay.
2. Dispatcher publish `recommendation.requests`. Recommendation Service nhận.
3. Recommendation gọi DietKnowledge lấy kiến thức đã duyệt (ưu tiên tìm theo ngữ nghĩa trên VectorDb), rồi gọi Gemini; Gemini tắt thì dùng kết quả an toàn dự phòng.
4. Publish `recommendation.results`. MealPlanning nhận (`RabbitMqRecommendationResultWorker`) và lưu.
5. Người dùng xem gợi ý, phải **chấp nhận** (`/accept`) thì mới thành kế hoạch thật.

### 8.3 Báo tiến độ

1. Người dùng đánh dấu đã ăn bữa nào (`api/daily-trackings/{date}/meals/{id}`).
2. Tracking tính `ProgressSummary` và gọi **gRPC** `RequestProgressNotification` sang Notification.
3. Notification publish `notification.messages`. NotificationWorker gửi email qua Brevo (hoặc ghi log).

### 8.4 Tri thức từ sách

1. Admin tải PDF/TXT lên DietKnowledge (`api/admin/book-sources`). File lưu ở `BookKnowledgeIngestion__StoragePath`.
2. Hệ thống cắt thành đoạn (`BookSourceChunk`), tạo `KnowledgeCandidate` (Gemini tóm tắt, hoặc dữ liệu giả khi tắt Gemini).
3. Admin duyệt/từ chối. Duyệt sẽ tạo Guideline/Food/Recipe `Approved`.
4. Nội dung `Approved` được lập chỉ mục vào VectorDb để tìm theo ngữ nghĩa.

---

## 9. Quy tắc thời gian khi lập kế hoạch bữa ăn

Mục này mô tả sửa lỗi "tạo được bữa đã qua giờ và nhận ngay mail nhắc".

**Giải thích đơn giản:** hệ thống phải biết "bây giờ là mấy giờ của người dùng" và "buổi nào đã hết". Máy chủ chạy giờ UTC, còn người dùng ở Việt Nam (UTC+7), nên cần một **độ lệch múi giờ** cấu hình được.

**Luật:**
- Kế hoạch **không được bắt đầu trước hôm nay** (tính theo giờ địa phương).
- Món trong ngày hôm nay **không được thuộc buổi đã kết thúc**: Sáng hết lúc 10:00, Trưa 14:00, Phụ 17:00, Tối 21:00.
- Chỉ nhắc nhở nào **còn ở tương lai** mới được xếp lịch. Giờ nhắc đã đến hạn thì không gửi (tránh nhận ngay một "nhắc nhở" cho bữa mình vừa tạo).
- Giờ nhắc mặc định theo giờ địa phương: Sáng 07:00, Trưa 11:00, Phụ 15:00, Tối 17:00.
- Sửa món cũ chỉ kiểm tra lại khi đổi **ngày hoặc buổi**; sửa kế hoạch chỉ kiểm tra lại khi dời **ngày bắt đầu**.

**Code ở đâu:**
- Luật và giờ các buổi: `src/Services/MealPlanning/LongevityDiet.MealPlanning.Application/Common/MealSlots.cs` (`DefaultReminderTimeLocal`, `EndTimeLocal`) và `.../Common/MealSchedulePolicy.cs` (`Today`, `HasSlotEnded`, `DefaultReminderAtUtc`).
- Nơi áp dụng: `.../Application/Services/MealPlanningService.cs` (tạo/sửa kế hoạch, thêm/sửa món, bỏ qua nhắc nhở đã đến hạn trong `EnqueueReminderMessages`, hàm `EnsureNotInPast`).
- Múi giờ cấu hình: `MealPlanning__UtcOffsetMinutes` (mặc định 420 = UTC+7), đọc ở `MealPlanning.Infrastructure/DependencyInjection.cs:24-28`; đặt trong `.env` bằng `MEAL_PLANNING_UTC_OFFSET_MINUTES`.
- Đồng hồ có thể thay trong test (`TimeProvider`): xem `tests/LongevityDiet.Core.Tests/MealPlanningServiceTests.cs`.
- Web: form "chấp nhận gợi ý" bắt đầu từ ngày mai nếu đã quá 10:00 giờ địa phương (`src/Web/LongevityDiet.Web/Controllers/RecommendationsController.cs`).

---

## 10. "Muốn sửa X thì xem ở đâu"

| Muốn làm | Xem file / chỗ |
| --- | --- |
| Thêm/đổi route Gateway | `src/Gateways/LongevityDiet.ApiGateway/appsettings.json` (+ cập nhật `docs/development/Gateway-Routes.md`) |
| Thêm API mới cho một service | Controller trong `src/Services/<Tên>/LongevityDiet.<Tên>.Api/Controllers/`, luật trong `.Application/Services/` |
| Thêm bảng/cột | Entity trong `.Domain/Entities/`, khai báo trong `*DbContext.cs`, rồi tạo migration (`dotnet ef migrations add ...`, DietKnowledge thêm `--context`) |
| Đổi tên queue RabbitMQ | `RabbitMqOptions.cs` hoặc mục `RabbitMq` trong `appsettings.json` của **cả bên gửi và bên nhận** |
| Đổi định dạng tin RabbitMQ | `src/BuildingBlocks/LongevityDiet.Contracts/Messaging/*Message.cs` |
| Đổi hàm gRPC | `notification.proto`, rồi code server `NotificationGrpcService.cs` và client `GrpcNotificationClient.cs` |
| Đổi khóa JWT, thời hạn, vai trò | `JwtTokenGenerator.cs`, `JwtSigningKeyResolver.cs`, `LongevityApiDefaultsExtensions.cs`, `JWT_SIGNING_KEY` trong `.env` |
| Đổi API key Gemini / model | `GEMINI_API_KEY`, `GEMINI_MODEL` trong `.env`, rồi tạo lại container `diet-knowledge-service` và `recommendation-service` |
| Đổi cấu hình email Brevo | `BREVO_*` trong `.env`, container `notification-worker` |
| Đổi giờ các buổi ăn, luật thời gian | `MealSlots.cs`, `MealSchedulePolicy.cs` |
| Đổi múi giờ người dùng | `MEAL_PLANNING_UTC_OFFSET_MINUTES` trong `.env` |
| Thêm chữ hiển thị cho người dùng | `Web/Services/Translations.cs` (cả EN và VI) |
| Thêm service/container mới | `docker-compose.yml`, route Gateway (nếu có API công khai), và cập nhật `docs/c4/`, `docs/development/Service-Port-Map.md` |
| Chạy test | `dotnet test tests/LongevityDiet.Core.Tests` |

---

## 11. Những điểm lệch giữa tài liệu cũ và code

- `docs/development/Messaging-Flow.md` ghi producer của `notification.messages` là "Notification.Grpc / Tracking". Trong code, Tracking **chỉ gọi gRPC**; chỉ Notification.Grpc publish lên queue này.
- `docs/development/Service-Port-Map.md` ghi cổng host 5105 là "internal gRPC". Thực tế gRPC nghe ở cổng 8080 trong container, còn host `5105` map vào cổng health 8081 (`docker-compose.yml:133-140`). gRPC chỉ dùng trong mạng Docker nên không cần mở cổng ra máy; chỉ lưu ý nếu muốn gọi gRPC từ máy host.
- Giờ nhắc mặc định trước đây tính theo UTC (07:00 UTC = 14:00 giờ Việt Nam), nay đã đổi sang giờ địa phương (mục 9).
