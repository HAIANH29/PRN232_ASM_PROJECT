\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).

\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).

\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).

\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).

\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).

\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).


\# Ghi chú Commit \& Hướng dẫn Test Hệ Thống (Longevity Diet Platform)



Tài liệu này bao gồm danh sách các commit mẫu mô phỏng lại quá trình phát triển dự án và hướng dẫn các bước kiểm thử (test) chi tiết cho hội đồng chấm thi hoặc QA.



\---



\## 1. Danh sách Fake Commits (Mẫu)



Bạn có thể sử dụng các thông điệp commit dưới đây để tạo lịch sử git giả lập. Các commit được sắp xếp theo trình tự phát triển của dự án (từ dựng khung đến hoàn thiện).



\### Giai đoạn 1: Dựng khung (Scaffold) \& Thiết lập cơ bản

\- `chore: init PRN232 Longevity Diet Platform solution and empty projects`

\- `build: add Dockerfile and docker-compose.yml skeleton`

\- `feat(core): add shared ApiDefaults (response envelopes, exceptions, pagination)`

\- `chore(gateway): setup YARP API Gateway basic routing config`



\### Giai đoạn 2: Identity Service (Xác thực \& Phân quyền)

\- `feat(identity): implement User and Role domain models`

\- `feat(identity): add JWT authentication and PBKDF2 password hashing`

\- `feat(identity): implement register, login, and profile APIs`

\- `chore(identity): add initial EF Core migration and demo admin seed data`



\### Giai đoạn 3: Diet Knowledge Service (Kiến thức \& PDF Ingestion)

\- `feat(knowledge): add DietGuideline, Food, and Recipe CRUD for Admin`

\- `feat(knowledge): implement public search, filter, and pagination endpoints`

\- `feat(knowledge): add book source review metadata and approval workflow`

\- `feat(knowledge): implement admin PDF upload and PdfPig text extraction`

\- `feat(knowledge): integrate Gemini for automated knowledge candidate generation`



\### Giai đoạn 4: Meal Planning (Lên thực đơn \& RabbitMQ)

\- `feat(planning): implement MealPlan and MealPlanItem CRUD operations`

\- `feat(planning): add ownership checks using JWT claims`

\- `feat(planning): integrate HTTP client to validate Food/Recipe from Knowledge service`

\- `feat(planning): publish recommendation requests and meal reminders to RabbitMQ`



\### Giai đoạn 5: Tracking \& Internal gRPC (Theo dõi tiến độ)

\- `feat(tracking): implement DailyTracking and MealTracking APIs`

\- `feat(tracking): calculate weekly and daily progress summaries`

\- `feat(tracking): setup gRPC client to call Notification Service on meal completion`



\### Giai đoạn 6: Notification \& Background Workers (Email \& AI)

\- `feat(notification): implement gRPC server to accept tracking triggers`

\- `feat(worker): setup Notification Worker to consume RabbitMQ messages`

\- `feat(worker): integrate Brevo for email delivery with local logging fallback`

\- `feat(recommendation): implement AI Recommendation consumer with Gemini safe prompt`



\### Giai đoạn 7: Web UI \& Final Polish

\- `feat(web): setup MVC Web Application and API Gateway integration`

\- `feat(web): implement auth pages, profile, and admin knowledge management UI`

\- `feat(web): add Meal Planning, Tracking, and Recommendation UI flows`

\- `feat(web): add English/Vietnamese i18n language switch`

\- `fix(web): resolve invisible Sign In button on Home hero section`

\- `docs: update C4 diagrams, ERD, and project submission checklist`



\---



\## 2. Hướng dẫn Test (Testing Guide)



Dưới đây là các bước để triển khai và kiểm tra toàn bộ luồng nghiệp vụ của hệ thống trên local.



\### 2.1. Chuẩn bị môi trường (Prerequisites)

\- Cài đặt \*\*Docker Desktop\*\* và \*\*.NET 9 SDK\*\*.

\- Clone source code về máy.

\- Copy file `.env.example` thành `.env` tại thư mục gốc. (Nếu test local không cần gửi email/AI thật, có thể để trống `GEMINI\_API\_KEY` và `BREVO\_API\_KEY`, hệ thống sẽ tự động dùng chế độ fallback/mock).



\### 2.2. Khởi chạy hệ thống

Mở terminal tại thư mục gốc và chạy lệnh:

```bash

dotnet restore LongevityDietPlatform.sln

docker compose up -d --build

```

Kiểm tra trạng thái container bằng lệnh `docker compose ps`. Đảm bảo các service (Postgres, RabbitMQ, Gateway, Web, các microservices) đều ở trạng thái `running` (hoặc `healthy`).



\### 2.3. Danh sách các tài khoản Demo

Hệ thống sẽ tự động seed các tài khoản sau khi khởi chạy lần đầu:

\- \*\*Admin:\*\* `admin@longevity.local` / Pass: `Admin@123456`

\- \*\*User:\*\* `user@longevity.local` / Pass: `User@123456`



\### 2.4. Các luồng kiểm thử chính (Key Test Flows)



\*\*Test Flow 1: Kiểm tra UI và Xác thực (Web \& Gateway)\*\*

1\. Truy cập Web Application tại: `http://localhost:5000`.

2\. Đăng nhập bằng tài khoản Admin. Kiểm tra menu điều hướng (chỉ Admin mới thấy phần "Managed Knowledge" và "Book Sources").

3\. Đăng xuất, tạo tài khoản mới (Register) hoặc đăng nhập bằng tài khoản User.

4\. Xác minh chức năng chuyển đổi ngôn ngữ (EN | VI) hoạt động trên giao diện.



\*\*Test Flow 2: Luồng Admin - Upload sách và sinh dữ liệu (Diet Knowledge)\*\*

1\. Đăng nhập tài khoản Admin, vào menu \*\*Book Sources\*\*.

2\. Upload một file PDF bất kỳ (khuyến nghị file nhẹ).

3\. Đợi hệ thống bóc tách PDF (Chunking). Nhấn \*\*Generate Candidates\*\* (Sinh ứng viên).

4\. Quan sát các dữ liệu mẫu được sinh ra (Nếu không có key Gemini, hệ thống dùng Local Fallback).

5\. Nhấn \*\*Approve\*\* để phê duyệt kiến thức vào hệ thống chính thức.



\*\*Test Flow 3: Luồng User - Lên lịch ăn \& Gợi ý AI (Meal Planning \& Recommendation)\*\*

1\. Đăng nhập tài khoản User.

2\. Vào \*\*Meal Ideas\*\* (Gợi ý AI). Nhập sở thích (ví dụ: "vegan, low carb") và gửi yêu cầu.

3\. \*Cơ chế ngầm:\* Lệnh sẽ được ném vào RabbitMQ, service Recommendation xử lý (gọi AI hoặc dùng fallback) và trả kết quả về.

4\. Giao diện tự refresh, hiển thị các gợi ý. Chấp nhận gợi ý để tạo thành một Meal Plan mới.



\*\*Test Flow 4: Luồng User - Theo dõi tiến độ \& gRPC/RabbitMQ (Tracking)\*\*

1\. Vào \*\*Daily Check-in\*\* (Tracking).

2\. Tích chọn "Mark as eaten" cho tất cả các bữa ăn trong ngày hôm nay.

3\. \*Cơ chế ngầm:\* 

&#x20;  - Tracking service lưu DB.

&#x20;  - Khi hoàn thành 100%, Tracking bắn \*\*gRPC\*\* sang Notification.

&#x20;  - Notification ném message vào \*\*RabbitMQ\*\*.

&#x20;  - NotificationWorker bắt message và gửi email (hoặc ghi log).

4\. Kiểm tra log của NotificationWorker trên Docker để thấy payload email đã được gửi thành công:

&#x20;  `docker compose logs -f notification-worker`



\### 2.5. Các Endpoint phục vụ Debug

\- \*\*RabbitMQ Management UI:\*\* `http://localhost:15672` (guest / guest). Dùng để check queue `longevity.reminders` và `longevity.notifications`.

\- \*\*API Gateway Routes:\*\* `http://localhost:5001/routes` (Liệt kê danh sách các route đang trỏ đến các microservice đằng sau).

