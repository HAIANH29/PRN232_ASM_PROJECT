# Web Application Guide

Project: `src/Web/LongevityDiet.Web` (ASP.NET Core MVC, host port `5000`).
Web không truy cập database; mọi dữ liệu đi qua API Gateway (`http://localhost:5001`).

## Cấu trúc

| Thư mục | Nội dung |
| --- | --- |
| `Controllers/` | Controller MVC cho từng chức năng |
| `Views/` | Razor view theo từng controller, `Shared/` cho layout |
| `Models/` | View model |
| `Services/` | `ApiGatewayClient`, `ApiGatewayOptions`, `UserSession`, `Translator`, `Translations`, `ContentTranslations` |
| `wwwroot/` | CSS, JS, thư viện client |

## Controller và chức năng

| Controller | Chức năng |
| --- | --- |
| `HomeController`, `AppController` | Trang chủ và khung ứng dụng |
| `AuthController` | Đăng ký, đăng nhập, hồ sơ sở thích |
| `KnowledgeController` | Duyệt tri thức đã duyệt, tìm kiếm |
| `RegimensController` | Xem chế độ ăn |
| `MealPlansController` | Tạo và quản lý kế hoạch bữa ăn |
| `RecommendationsController` | Yêu cầu và chấp nhận gợi ý bữa ăn |
| `TrackingController` | Theo dõi hằng ngày và tiến độ |
| `AdminKnowledgeController` | Quản trị guideline, food, recipe |
| `AdminRegimensController` | Quản trị chế độ ăn |
| `AdminBookSourcesController` | Upload sách, duyệt ứng viên tri thức |
| `LanguageController` | Đổi ngôn ngữ giao diện |

## Gọi API

`ApiGatewayClient` gửi request tới gateway, gắn `Authorization: Bearer <token>` lấy từ `UserSession`, và đọc envelope `ApiResponse<T>`. Địa chỉ gateway cấu hình qua `ApiGatewayOptions`. Trong Docker Compose, service `web` phụ thuộc `api-gateway`.

## Đa ngôn ngữ

Giao diện hỗ trợ tiếng Anh và tiếng Việt thông qua `Translator` và các bảng dịch trong `Translations`/`ContentTranslations`; `LanguageController` lưu lựa chọn của người dùng. Ngôn ngữ cũng được gửi kèm yêu cầu gợi ý (`language`) để tiêu đề món ăn theo cùng ngôn ngữ.

## Phân quyền trên giao diện

- Trang đọc tri thức công khai không cần đăng nhập.
- Kế hoạch, gợi ý, theo dõi yêu cầu đăng nhập.
- Các trang quản trị (`Admin*`) chỉ dành cho tài khoản `Admin`; API phía sau vẫn tự kiểm tra vai trò.

## Chạy cục bộ

```powershell
copy .env.example .env
docker compose up -d --build
```

Mở `http://localhost:5000`, đăng nhập bằng tài khoản demo (xem `Configuration-Reference.md`). Nếu trang lỗi kết nối, kiểm tra gateway tại `http://localhost:5001/routes` và xem `Troubleshooting.md`.

## Mở rộng

Khi thêm chức năng mới: thêm API ở service tương ứng, thêm route ở gateway nếu cần, thêm controller và view trong Web, rồi cập nhật bảng dịch cho cả hai ngôn ngữ.
