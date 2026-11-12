# Recommendation Safety Policy

Gợi ý bữa ăn của nền tảng chỉ mang tính giáo dục. Hệ thống không đưa ra lời khuyên y tế, chẩn đoán, điều trị, dự đoán bệnh hay dự đoán tuổi thọ. Logic nằm trong `LongevityDiet.Recommendation.Service` (`RecommendationSafetyPolicy`, `RecommendationGenerator`).

## Nguyên tắc

1. Chỉ dùng tri thức đã duyệt (`Approved`) của Diet Knowledge làm ngữ cảnh.
2. Không thêm kiến thức dinh dưỡng bên ngoài hay tuyên bố y tế.
3. Lọc thẻ sở thích đầu vào và tiêu đề gợi ý đầu ra bằng danh sách từ khóa cấm.
4. Mọi kết quả đều kèm disclaimer, có bản tiếng Anh và tiếng Việt theo `language`.

## Từ khóa bị loại

Thẻ sở thích hoặc tiêu đề chứa các cụm sau (không phân biệt hoa thường) sẽ bị bỏ:

- Tiếng Anh: `diagnosis`, `diagnose`, `treatment`, `treat `, `cure`, `disease`, `diabetes`, `cancer`, `lifespan`, `life span`, `predict`, `prediction`.
- Tiếng Việt: `chẩn đoán`, `điều trị`, `chữa`, `bệnh`, `tiểu đường`, `ung thư`, `tuổi thọ`, `dự đoán`.

## Làm sạch dữ liệu

| Bước | Quy tắc |
| --- | --- |
| Thẻ sở thích | Cắt khoảng trắng, bỏ rỗng, bỏ thẻ chứa từ cấm, loại trùng, giữ tối đa 12 thẻ |
| Tiêu đề gợi ý | Cắt khoảng trắng, bỏ rỗng, bỏ tiêu đề chứa từ cấm, tối đa 120 ký tự, loại trùng, giữ đúng số lượng yêu cầu |
| Số ngày | Giới hạn trong khoảng 1-14 |

## Luồng sinh gợi ý

1. Nhận `RecommendationRequestMessage` từ queue `recommendation.requests`.
2. Lấy tri thức đã duyệt qua API công khai của Diet Knowledge.
3. Nếu chưa có tri thức nào, trả kết quả dự phòng với lý do tương ứng.
4. Nếu Gemini được bật: xếp hạng tri thức theo độ gần với sở thích (tìm kiếm vector), dựng prompt có giới hạn số mục (tối đa 25 guideline, 40 food, 30 recipe), yêu cầu Gemini trả JSON gồm `suggestedMealTitles` và `disclaimer`.
5. Nếu Gemini tắt, lỗi, hoặc phản hồi không parse được thành tiêu đề an toàn, dùng kết quả dự phòng cục bộ.
6. Gửi `RecommendationResultMessage` vào `recommendation.results`.

Prompt yêu cầu rõ ràng: chỉ dùng tri thức đã liệt kê, không đưa ra chẩn đoán, điều trị hay dự đoán, và viết bằng tiếng Việt khi `language = vi`.

## Disclaimer

Bản tiếng Anh: "Educational meal planning suggestion based on approved project knowledge and user preferences. It is not medical advice, diagnosis, treatment, disease prediction, or lifespan prediction."

Bản tiếng Việt nằm trong `RecommendationSafetyPolicy.DisclaimerVi`.

## Kiểm thử

Hành vi lọc và ngữ cảnh tri thức đã duyệt được bao phủ trong `tests/LongevityDiet.Core.Tests`; xem `Testing-Guide.md`.

## Giới hạn đã biết

- Danh sách từ cấm là so khớp chuỗi con, đơn giản, có thể loại nhầm hoặc bỏ sót.
- Hồ sơ sở thích là dữ liệu người dùng tự khai, không phải thông tin y tế.
