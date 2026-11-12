# Authentication and Authorization

## Tổng quan

Identity Service là nơi duy nhất phát hành JWT. Các service khác (Diet Knowledge, Meal Planning, Tracking) chỉ xác thực token bằng cùng khóa ký, issuer và audience. Gateway chỉ chuyển tiếp header `Authorization: Bearer <token>`, không tự xác thực.

Phần cấu hình dùng chung nằm trong `src/BuildingBlocks/LongevityDiet.ApiDefaults` (`AddLongevityPublicApiDefaults`).

## Token

| Thuộc tính | Giá trị mặc định | Cấu hình |
| --- | --- | --- |
| Issuer | `LongevityDiet.Identity` | `Jwt:Issuer` |
| Audience | `LongevityDiet.Platform` | `Jwt:Audience` |
| Khóa ký | `development-only-replace-with-secret` | `Jwt:SigningKey` (biến `JWT_SIGNING_KEY`) |
| Thời hạn | 120 phút | `AccessTokenMinutes` |

Các service validate issuer, audience, chữ ký và thời hạn. Tất cả service phải dùng cùng `Jwt__SigningKey`, nếu lệch sẽ nhận 401.

## Claims

| Claim | Nội dung |
| --- | --- |
| `ClaimTypes.NameIdentifier` | Id người dùng (Guid) |
| `ClaimTypes.Email` | Email |
| `ClaimTypes.Name` | Tên hiển thị |
| `ClaimTypes.Role` | Mỗi vai trò một claim (`User`, `Admin`) |

Các service nghiệp vụ lấy user id từ `NameIdentifier`. Token không có Guid hợp lệ sẽ bị từ chối.

## Vai trò

| Vai trò | Quyền |
| --- | --- |
| Anonymous | Đọc tri thức đã duyệt, tìm kiếm, đăng ký, đăng nhập |
| `User` | Hồ sơ sở thích, kế hoạch bữa ăn, gợi ý, theo dõi |
| `Admin` | Quản trị tri thức, nạp sách, chỉ mục vector |

Đăng ký công khai chỉ tạo vai trò `User`. Tài khoản Admin được seed khi khởi động từ cấu hình `IdentitySeed`.

## Cách áp dụng quyền

- `[Authorize]`: cần token hợp lệ (Meal Planning, Tracking).
- `[Authorize(Roles = "Admin")]`: các route `/api/admin/...` của Diet Knowledge.
- `[Authorize(Policy = "AdminOnly")]`: Identity định nghĩa policy này bằng `RequireRole("Admin")`.
- `[AllowAnonymous]`: nhóm endpoint đọc công khai của Diet Knowledge.

## Mật khẩu

Mật khẩu được băm bằng PBKDF2 (`Pbkdf2PasswordHasher`) trước khi lưu vào `Users.PasswordHash`.

## Phía Web application

Web lưu token trong phiên người dùng (`UserSession`) và gắn vào mỗi lời gọi tới gateway qua `ApiGatewayClient`. Xem `Web-Application-Guide.md`.

## Lưu ý bảo mật cho demo

- Khóa ký mặc định chỉ dành cho phát triển. Đổi `JWT_SIGNING_KEY` khi triển khai thật.
- Tài khoản demo trong `.env.example` công khai, không dùng ngoài môi trường demo.
- Ở demo Docker, giao tiếp dùng HTTP trên localhost; ranh giới client-platform dự kiến là HTTPS.
