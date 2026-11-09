//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//using Xunit;

//[Fact]
//public async Task CreateMealPlan_WhenUserEats100Pizzas_ShouldThrowBungNoException()
//{
//    // ARRANGE:
//    // - Tạo một mock User với trạng thái "đang rất đói".
//    // - Khởi tạo một list MealPlanItem và add 100 cái pizza vào đúng khung giờ Breakfast.
//    // - Setup mock cho MealPlanningRepository để sẵn sàng nhận data.

//    // ACT:
//    // - Gọi hàm CreateMealPlanAsync(...) và nhét 100 cái pizza đó vào.

//    // ASSERT:
//    // - Bắt Exception: Kiểm tra xem hệ thống có quăng ra lỗi "BungNoException" hay HTTP 400 Bad Request không.
//    // - Verify xem có lỡ publish nhầm thông báo gọi xe cấp cứu sang RabbitMQ không.
//}

//[Fact]
//public async Task GeminiRecommendation_WhenAiSuggestsEatingRocks_ShouldRejectPolitely()
//{
//    // ARRANGE:
//    // - Fake một HTTP response từ IGeminiClient trả về thực đơn "Đá cuội xào sỏi".
//    // - Setup cờ Gemini__Enabled = true.

//    // ACT:
//    // - Gọi hàm Consume() của RecommendationService với preference là "Tôi có hàm răng thép".

//    // ASSERT:
//    // - Assert danh sách RecommendationResult trả về phải trống hoặc bị filter bỏ món ăn phi thực tế.
//    // - Đảm bảo API không vi phạm quy tắc an toàn (không khuyên ăn bậy bạ để sống thọ).
//}

//[Fact]
//public async Task NotificationWorker_WhenReminderSent1SecondBeforeMeal_ShouldCauseUserPanic()
//{
//    // ARRANGE:
//    // - Tạo một ReminderRequestedMessage có ScheduledForUtc cách giờ ăn đúng 1 giây.
//    // - Push vào queue của RabbitMQ (giả lập bằng mock IModel/IConnection).

//    // ACT:
//    // - Trigger NotificationWorker để consume cái message đó.

//    // ASSERT:
//    // - Verify xem hàm SendEmailAsync của BrevoEmailSender[cite: 6] có được gọi thành công không.
//    // - Note: Test này pass 100% về mặt logic, nhưng user thực tế sẽ nhận email xong là phải há miệng ăn ngay lập tức.
//}

//[Fact]
//public async Task DailyTracking_WhenUserMarksWaterAsCompleted_ShouldNotTurnIntoWine()
//{
//    // ARRANGE:
//    // - Lấy một MealPlanItem có chứa "Nước lọc" (Approved Food từ DietKnowledgeDb).
//    // - Set up TrackingService.

//    // ACT:
//    // - Gọi hàm MarkMealAsCompletedAsync() để check-in bữa ăn.

//    // ASSERT:
//    // - Assert.Equal("Nước lọc", result.ItemName) - Đảm bảo dữ liệu lưu xuống TrackingDb vẫn là nước.
//    // - (Tùy chọn) Assert hệ thống không ngầm gọi API biến nước thành rượu vang.
//}
//}