# Kịch bản thuyết minh video demo (tiếng Việt)

Video ~6-7 phút, bám theo `Demo-Script.md`. Cột **Hình** là thao tác trên màn hình, cột **Lời đọc** là câu thuyết minh.

## Quy ước đọc thuật ngữ tiếng Anh

- Trong lời đọc, thuật ngữ tiếng Anh viết **in đậm**: phải đọc theo **phát âm tiếng Anh**, không đọc theo vần tiếng Việt.
- Tên riêng/lệnh/URL **không đọc**, chỉ hiện trên màn hình (ví dụ `localhost:5000`, mật khẩu).
- Chữ viết tắt đọc từng chữ cái: JWT = "jay-đáp-bồ-liu-ti", REST = "rét", API = "ây-pi-ai", gRPC = "gee-A-pi-xi", RPC → đọc "g R P C".
- Nếu dùng giọng AI (TTS): xem mục "Cách làm giọng đọc" cuối file.

### Bảng cách đọc

| Thuật ngữ | Đọc như | Gợi ý phiên âm |
| --- | --- | --- |
| microservices | mai-crâu-sơ-vi-xì-zừ | /ˈmaɪkroʊˌsɜːrvɪsɪz/ |
| API Gateway | ây-pi-ai ghết-quây | /ˌeɪpiˈaɪ ˈɡeɪtweɪ/ |
| REST | rét | /rɛst/ |
| JWT | jay-đáp-bồ-liu-ti | J-W-T |
| gRPC | gee-A-pi-xi | g-R-P-C |
| RabbitMQ | ra-bịt em-kiu | /ˈræbɪt ɛm kjuː/ |
| outbox | ao-bóc | /ˈaʊtbɒks/ |
| worker | quơ-cờ | /ˈwɜːrkər/ |
| PostgreSQL | pốt-grét-ét-kiu-eo | /ˈpoʊstɡrɛs kjuː ɛl/ |
| pgvector | pi-gi-véc-tơ | /pi dʒi ˈvɛktər/ |
| Docker Compose | đóc-cơ com-pâu | /ˈdɒkər kəmˈpoʊz/ |
| Gemini | je-mi-nai | /ˈdʒɛmɪnaɪ/ |
| Brevo | bờ-rê-vô | /ˈbrɛvoʊ/ |
| dashboard | đát-bo | /ˈdæʃbɔːrd/ |
| fallback | phôn-bách | /ˈfɔːlbæk/ |
| Admin | át-min | /ˈædmɪn/ |
| User | diu-dơ | /ˈjuːzər/ |
| login / logout | lót-in / lót-ao | /ˈlɒɡɪn/ |
| profile | prồ-phai | /ˈproʊfaɪl/ |
| meal plan | mil plan | /miːl plæn/ |
| tracking | tra-kinh | /ˈtrækɪŋ/ |
| unit test | diu-nít tét | /ˈjuːnɪt tɛst/ |
| Approved | ờ-pruvd | /əˈpruːvd/ |
| Book Sources | búc sọt-xìz | /bʊk ˈsɔːrsɪz/ |
| Smart search | xmát xớt | /smɑːrt sɜːrtʃ/ |
| Meal Ideas | mil ai-đi-ờz | /miːl aɪˈdɪəz/ |
| Diet Regimens | đai-ợt re-gi-mừnz | /ˈdaɪət ˈrɛdʒɪmənz/ |

## Kịch bản

### 1. Mở đầu (~30 giây)

| Hình | Lời đọc |
| --- | --- |
| Trang chủ Web, `localhost:5000` | Xin chào thầy cô. Đây là video demo của nhóm cho dự án **Longevity Diet Platform**, một nền tảng **microservices** hỗ trợ lập kế hoạch và theo dõi chế độ ăn, dựa trên cuốn sách The Longevity Diet. |
| Chữ "Educational, not medical" | Hệ thống chỉ mang tính giáo dục, không chẩn đoán, không điều trị, không dự đoán bệnh hay tuổi thọ. |

### 2. Kiến trúc (~60 giây)

| Hình | Lời đọc |
| --- | --- |
| `C1-Internal-Architecture` | Giao diện **Web** chỉ gọi một cổng duy nhất là **API Gateway**. Phía sau có bốn dịch vụ **REST**: Identity, Diet Knowledge, Meal Planning và Tracking, mỗi dịch vụ sở hữu một cơ sở dữ liệu **PostgreSQL** riêng. |
| Chỉ vào Recommendation, Notification | Các dịch vụ nội bộ gồm Recommendation, Notification và Notification **Worker**. Tracking gọi Notification bằng **gRPC**; gợi ý món ăn và nhắc nhở đi qua **RabbitMQ**. **Gemini** và **Brevo** là dịch vụ ngoài, và đều có **fallback** an toàn khi chạy cục bộ. |

### 3. Khởi động và kiểm tra (~30 giây)

| Hình | Lời đọc |
| --- | --- |
| Terminal: `docker compose up -d`; mở `localhost:5001/routes` | Hệ thống chạy bằng **Docker Compose**. Trang **routes** của gateway chỉ công khai bốn dịch vụ; Recommendation và Notification được giữ nội bộ. |
| Mở các trang `/health` | Cả năm dịch vụ đều báo **health** bình thường. |

### 4. Admin (~75 giây)

| Hình | Lời đọc |
| --- | --- |
| Đăng nhập admin | Đầu tiên em đăng nhập bằng tài khoản **Admin**. |
| Màn hình Guidelines, Foods, Recipes | **Admin** quản lý kiến thức: hướng dẫn chế độ ăn, thực phẩm và công thức. |
| **Book Sources** | Trong **Book Sources**, chỉ **Admin** được tải lên tệp PDF hợp pháp. Nội dung được cắt thành các đoạn và tạo ra các ứng viên kiến thức. Chúng chỉ hiển thị cho người dùng sau khi **Admin** duyệt, tức là chuyển sang trạng thái **Approved**. Gợi ý món ăn chỉ dùng kiến thức đã **Approved**, không bao giờ dùng đoạn thô từ PDF. |

### 5. Người dùng và lập kế hoạch (~75 giây)

| Hình | Lời đọc |
| --- | --- |
| Đăng xuất, đăng nhập user | Bây giờ em đăng nhập bằng tài khoản **User**. |
| Trang **Profile** | Trang **Profile** hiển thị dữ liệu người dùng lấy từ token **JWT**. |
| Foods: tìm kiếm, lọc, phân trang | Người dùng duyệt thực phẩm và công thức, có tìm kiếm, lọc và phân trang. |
| Tạo **meal plan** "Teacher Demo Plan", thêm một món | Em tạo một **meal plan** và thêm một món. Meal Planning kiểm tra mã món qua **REST API** của Diet Knowledge, chứ không đọc trực tiếp cơ sở dữ liệu của dịch vụ khác. |

### 6. Gợi ý món ăn (~60 giây)

| Hình | Lời đọc |
| --- | --- |
| **Meal Ideas**, nhập `legumes, vegetables, simple` | Ở **Meal Ideas**, em nhập sở thích như đậu, rau và món đơn giản. |
| Kết quả, bấm Accept | Yêu cầu được lưu vào **outbox**, rồi gửi qua **RabbitMQ** tới Recommendation. Dịch vụ này tạo gợi ý từ kiến thức đã **Approved**. Khi tắt **Gemini**, hệ thống dùng gợi ý **fallback** an toàn. Người dùng xem và chấp nhận thì gợi ý mới trở thành kế hoạch. |

### 7. Theo dõi và thông báo (~45 giây)

| Hình | Lời đọc |
| --- | --- |
| **Tracking**, đánh dấu hoàn thành | Ở **Tracking**, em đánh dấu món đã ăn, rồi xem tổng kết tiến độ trong ngày. |
| Chỉ vào log Notification Worker | Khi hoàn thành đủ, Tracking gọi **gRPC** tới Notification, thông báo đi qua **RabbitMQ** tới **Worker**, và gửi email qua **Brevo** hoặc ghi log. |

### 8. Module mới (~45 giây)

| Hình | Lời đọc |
| --- | --- |
| Admin: Semantic search index | Admin có thẻ chỉ mục tìm kiếm ngữ nghĩa, lưu trong cơ sở dữ liệu **pgvector** riêng. |
| User: **Smart search** "light bean dinner" | Người dùng dùng **Smart search** để tìm món theo ý nghĩa. |
| **Diet Regimens**, **Start tracking** | Với **Diet Regimens**, người dùng bấm bắt đầu theo dõi một thực đơn mẫu. |

### 9. RabbitMQ và kiểm thử (~45 giây)

| Hình | Lời đọc |
| --- | --- |
| Giao diện quản trị **RabbitMQ**, danh sách queue | Trên giao diện **RabbitMQ** có các hàng đợi nhắc nhở, thông báo, yêu cầu và kết quả gợi ý, kèm hàng đợi lỗi. Dữ liệu qua **outbox** nên không mất khi broker gặp sự cố. |
| Terminal: `dotnet test` | Cuối cùng là chín mươi bốn **unit test**, tất cả đều đạt. |

### 10. Kết (~20 giây)

| Hình | Lời đọc |
| --- | --- |
| Slide kết | Dự án thể hiện **microservices**, **REST**, **JWT**, **PostgreSQL** theo từng dịch vụ, **gRPC**, **RabbitMQ** và **Docker Compose**. Xin cảm ơn thầy cô đã theo dõi. |

> Số "94 test" lấy từ `PROJECT_PROGRESS.md`. Chạy lại `dotnet test` trước khi quay để chắc con số còn đúng.

## Cách làm giọng đọc

1. **Tự đọc (chuẩn nhất):** đọc theo bảng phát âm ở trên, thu âm từng mục rồi ghép.
2. **Giọng AI (Azure Speech, giọng `vi-VN`):** bọc thuật ngữ tiếng Anh bằng thẻ `<lang>`, hoặc ép phát âm bằng `<phoneme>`:

```xml
<speak version="1.0" xmlns="http://www.w3.org/2001/10/synthesis" xml:lang="vi-VN">
  <voice name="vi-VN-HoaiMyNeural">
    Giao diện Web chỉ gọi một cổng duy nhất là
    <lang xml:lang="en-US">API Gateway</lang>.
    Tracking gọi Notification bằng
    <lang xml:lang="en-US"><say-as interpret-as="characters">gRPC</say-as></lang>.
  </voice>
</speak>
```

3. **ElevenLabs / giọng đa ngôn ngữ:** dùng model `eleven_multilingual_v2`, giữ nguyên chữ Anh trong câu; nếu đọc sai, viết lại theo cột "Đọc như" của bảng.
4. Luôn nghe lại các từ khó trước: **gRPC, RabbitMQ, pgvector, PostgreSQL, JWT**.
