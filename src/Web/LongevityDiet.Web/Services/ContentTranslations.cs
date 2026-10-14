using System.Text.RegularExpressions;

namespace LongevityDiet.Web.Services;

/// <summary>
/// Vietnamese text for the content that ships with the platform (seed guidelines, foods, recipes, their
/// categories, quantities and source notes). Content added later by admins is shown exactly as entered.
/// Only used when displaying; edit forms always show the stored value.
/// </summary>
public static class ContentTranslations
{
    public static readonly IReadOnlyDictionary<string, string> Vi = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        // Food names
        ["Almonds"] = "Hạnh nhân",
        ["Broccoli"] = "Bông cải xanh",
        ["Brown rice"] = "Gạo lứt",
        ["Cannellini beans"] = "Đậu cannellini",
        ["Carrots"] = "Cà rốt",
        ["Chickpeas"] = "Đậu gà",
        ["Extra virgin olive oil"] = "Dầu ô liu nguyên chất",
        ["Farro"] = "Lúa mì farro",
        ["Green lentils"] = "Đậu lăng xanh",
        ["Herbs and lemon"] = "Thảo mộc và chanh",
        ["Kale"] = "Cải xoăn",
        ["Leafy greens"] = "Rau lá xanh",
        ["Lemon-herb seasoning"] = "Gia vị chanh và thảo mộc",
        ["Lentils"] = "Đậu lăng",
        ["Navy beans"] = "Đậu navy",
        ["Tomatoes"] = "Cà chua",
        ["Walnuts"] = "Quả óc chó",
        ["Whole-grain pasta"] = "Mì ống nguyên cám",

        // Food categories
        ["Nut"] = "Hạt",
        ["Vegetable"] = "Rau củ",
        ["Whole grain"] = "Ngũ cốc nguyên hạt",
        ["Legume"] = "Họ đậu",
        ["Healthy fat"] = "Chất béo tốt",
        ["Seasoning"] = "Gia vị",

        // Food notes
        ["Use as a small topping or snack option."] = "Dùng làm lớp phủ nhỏ hoặc món ăn vặt.",
        ["Cruciferous vegetable suitable for simple meals."] = "Rau họ cải, phù hợp cho các bữa ăn đơn giản.",
        ["Minimally refined grain for balanced meal bowls."] = "Ngũ cốc ít tinh chế, hợp cho các bát ăn cân bằng.",
        ["Mild bean option for soups and vegetable dishes."] = "Loại đậu vị nhẹ, hợp với súp và các món rau.",
        ["A simple vegetable option for soups, bowls, and side dishes."] = "Loại rau đơn giản cho súp, bát ăn và món phụ.",
        ["Plant-protein staple for salads, stews, and bowls."] = "Nguồn đạm thực vật chủ lực cho salad, món hầm và bát ăn.",
        ["Primary cooking and dressing fat for demo recipes."] = "Chất béo chính để nấu và trộn salad trong các công thức demo.",
        ["A minimally refined grain for warm bowls and vegetable plates."] = "Ngũ cốc ít tinh chế cho các bát ăn nóng và đĩa rau.",
        ["A flexible plant-protein base for bowls, soups, and salads."] = "Nguồn đạm thực vật linh hoạt cho bát ăn, súp và salad.",
        ["Flavor meals without turning the content into medical advice."] = "Tạo hương vị cho món ăn mà không biến nội dung thành lời khuyên y tế.",
        ["A leafy green that works in salads, soups, and grain bowls."] = "Rau lá xanh hợp với salad, súp và bát ngũ cốc.",
        ["Flexible vegetable base for salads, soups, and warm bowls."] = "Nền rau linh hoạt cho salad, súp và các bát ăn nóng.",
        ["Adds flavor while keeping recipes simple and educational."] = "Thêm hương vị mà vẫn giữ công thức đơn giản và mang tính giáo dục.",
        ["Useful base for soups, salads, and bowls."] = "Nguyên liệu nền hữu ích cho súp, salad và bát ăn.",
        ["A mild bean option for soups, stews, and vegetable-heavy meals."] = "Loại đậu vị nhẹ cho súp, món hầm và các bữa ăn nhiều rau.",
        ["Works well in soups, sauces, salads, and grain bowls."] = "Hợp với súp, nước sốt, salad và bát ngũ cốc.",
        ["Use small portions for texture and plant-based fats."] = "Dùng một lượng nhỏ để tạo độ giòn và bổ sung chất béo thực vật.",
        ["Use moderate portions with vegetables and legumes."] = "Dùng lượng vừa phải, kèm rau và các loại đậu.",

        // Recipe names
        ["Bean and greens soup"] = "Súp đậu và rau xanh",
        ["Chickpea vegetable bowl"] = "Bát đậu gà và rau củ",
        ["Green lentil farro bowl"] = "Bát đậu lăng xanh và farro",
        ["Lentil walnut salad"] = "Salad đậu lăng và óc chó",
        ["Navy bean kale soup"] = "Súp đậu navy và cải xoăn",
        ["Whole-grain pasta with legumes"] = "Mì ống nguyên cám với các loại đậu",

        // Recipe descriptions
        ["A warm soup with cannellini beans, greens, tomatoes, broccoli, herbs, and olive oil."] = "Món súp nóng với đậu cannellini, rau xanh, cà chua, bông cải xanh, thảo mộc và dầu ô liu.",
        ["A plant-forward bowl with chickpeas, brown rice, greens, tomatoes, herbs, lemon, and olive oil."] = "Bát ăn thiên về thực vật với đậu gà, gạo lứt, rau xanh, cà chua, thảo mộc, chanh và dầu ô liu.",
        ["A demo meal with green lentils, farro, kale, carrots, and lemon-herb seasoning."] = "Món demo với đậu lăng xanh, farro, cải xoăn, cà rốt và gia vị chanh thảo mộc.",
        ["A simple salad using lentils, leafy greens, walnuts, tomatoes, herbs, lemon, and olive oil."] = "Món salad đơn giản với đậu lăng, rau lá xanh, óc chó, cà chua, thảo mộc, chanh và dầu ô liu.",
        ["A warm demo soup with navy beans, kale, carrots, and lemon-herb seasoning."] = "Món súp nóng demo với đậu navy, cải xoăn, cà rốt và gia vị chanh thảo mộc.",
        ["A modest whole-grain pasta dish with chickpeas, tomatoes, greens, herbs, lemon, and olive oil."] = "Món mì ống nguyên cám vừa phải với đậu gà, cà chua, rau xanh, thảo mộc, chanh và dầu ô liu.",

        // Guideline titles
        ["Keep added sugar and refined grains limited"] = "Hạn chế đường bổ sung và ngũ cốc tinh chế",
        ["Legume and whole-grain planning base"] = "Nền tảng lên kế hoạch từ đậu và ngũ cốc nguyên hạt",
        ["Plant-forward daily meals"] = "Bữa ăn hằng ngày thiên về thực vật",
        ["Plant-forward everyday meals"] = "Bữa ăn thường ngày thiên về thực vật",
        ["Prefer legumes and whole grains"] = "Ưu tiên các loại đậu và ngũ cốc nguyên hạt",
        ["Simple scheduled meals"] = "Bữa ăn có lịch đơn giản",
        ["Use simple meal timing for planning"] = "Dùng giờ ăn đơn giản khi lên kế hoạch",

        // Guideline summaries
        ["Treat highly refined grains, sweetened drinks, and dessert-style foods as occasional choices rather than everyday planned meals."] = "Xem ngũ cốc tinh chế cao, đồ uống có đường và món tráng miệng là lựa chọn thỉnh thoảng, không phải bữa ăn lên kế hoạch hằng ngày.",
        ["Plan regular meals around beans, lentils, chickpeas, and minimally refined grains, keeping portions practical and balanced."] = "Lên kế hoạch các bữa ăn thường xuyên quanh đậu, đậu lăng, đậu gà và ngũ cốc ít tinh chế, giữ khẩu phần thực tế và cân bằng.",
        ["Build most meals around vegetables, legumes, whole grains, nuts, and olive-oil-based fats. Keep this guidance educational and planning-focused."] = "Xây dựng phần lớn bữa ăn quanh rau củ, các loại đậu, ngũ cốc nguyên hạt, các loại hạt và chất béo từ dầu ô liu. Giữ nội dung này mang tính giáo dục và tập trung vào lập kế hoạch.",
        ["Use vegetables, legumes, whole grains, nuts, and olive-oil-based fats as the normal planning base for meals."] = "Dùng rau củ, các loại đậu, ngũ cốc nguyên hạt, các loại hạt và chất béo từ dầu ô liu làm nền tảng thông thường để lên kế hoạch bữa ăn.",
        ["Use beans, lentils, chickpeas, and minimally refined grains as regular staples for meal plans and recipe ideas."] = "Dùng đậu, đậu lăng, đậu gà và ngũ cốc ít tinh chế làm thực phẩm chính thường xuyên cho kế hoạch bữa ăn và ý tưởng công thức.",
        ["Keep planned meals easy to repeat, easy to track, and framed as educational lifestyle support rather than medical treatment."] = "Giữ các bữa ăn đã lên kế hoạch dễ lặp lại, dễ theo dõi, và coi đó là hỗ trợ lối sống mang tính giáo dục chứ không phải điều trị y tế.",
        ["Keep meal schedules consistent and practical for the user. Do not present timing guidance as treatment for any disease."] = "Giữ lịch ăn nhất quán và thực tế cho người dùng. Không trình bày hướng dẫn về giờ ăn như một cách điều trị bệnh.",

        // Source labels and notes
        ["Team-approved demo scope"] = "Phạm vi demo do nhóm duyệt",
        ["Local demo seed; replace with exact book chapter/page after source verification."] = "Dữ liệu demo cục bộ; cần thay bằng đúng chương/trang của sách sau khi đối chiếu nguồn.",
        ["TODO: team must verify exact source location."] = "Cần làm: nhóm phải xác minh đúng vị trí nguồn.",
        ["Project-ready summary in the team's own words. Verify against the legally obtained source copy before approval."] = "Tóm tắt theo lời của nhóm, sẵn sàng cho dự án. Cần đối chiếu với bản sách hợp pháp trước khi duyệt.",
        ["Local demo summary written in the team's own words for the PRN232 teacher demo. Replace with exact source metadata after team review."] = "Tóm tắt demo cục bộ do nhóm tự viết cho buổi demo môn PRN232. Cần thay bằng thông tin nguồn chính xác sau khi nhóm xem xét.",
        ["Legacy content requires source verification."] = "Nội dung cũ cần được xác minh nguồn.",
        ["The Longevity Diet"] = "The Longevity Diet",
        ["Uploaded PDF review"] = "Xem xét PDF đã tải lên",
        ["Admin review required. Use this chunk to write a concise project-ready summary in the team's own words before approving it."] = "Cần quản trị viên xem xét. Hãy dùng đoạn này để viết một bản tóm tắt ngắn gọn, sẵn sàng cho dự án, bằng lời của nhóm trước khi duyệt.",
        ["N/A"] = "Không có",

        // Fixed texts produced by the recommendation service
        ["Educational meal planning suggestion based on approved project knowledge and user preferences. It is not medical advice, diagnosis, treatment, disease prediction, or lifespan prediction."] = "Gợi ý lập kế hoạch bữa ăn mang tính giáo dục, dựa trên kiến thức đã được duyệt của dự án và sở thích của người dùng. Đây không phải lời khuyên y tế, chẩn đoán, điều trị, dự đoán bệnh tật hay dự đoán tuổi thọ.",
        ["Educational meal planning suggestion based on approved project knowledge and user preferences. It is not medical advice, diagnosis, treatment, disease prediction, or lifespan prediction. No approved knowledge is available yet."] = "Gợi ý lập kế hoạch bữa ăn mang tính giáo dục, dựa trên kiến thức đã được duyệt của dự án và sở thích của người dùng. Đây không phải lời khuyên y tế, chẩn đoán, điều trị, dự đoán bệnh tật hay dự đoán tuổi thọ. Hiện chưa có kiến thức nào được duyệt.",
    };

    // Words inside quantities such as "3/4 cup cooked" or "to taste".
    private static readonly (string English, string Vietnamese)[] QuantityPhrases =
    [
        ("to taste", "vừa ăn"),
        ("cooked", "đã nấu chín"),
        ("serving", "khẩu phần"),
        ("cups", "chén"),
        ("cup", "chén"),
        ("tbsp", "muỗng canh"),
        ("tsp", "muỗng cà phê"),
        ("pinch", "nhúm"),
        ("slices", "lát"),
        ("slice", "lát"),
    ];

    private static readonly Regex UploadedPdfReference =
        new(@"^Uploaded PDF (?<file>.+?); page (?<page>[^;]+); chunk (?<chunk>\d+)$", RegexOptions.Compiled);

    private static readonly Regex ReviewTitle =
        new(@"^Review (?<file>.+?) page (?<page>\S+) chunk (?<chunk>\d+)$", RegexOptions.Compiled);

    private static readonly Regex FallbackMealTitle =
        new(@"^(?<name>.+?) (?<category>Nut|Vegetable|Whole grain|Legume|Healthy fat|Seasoning) meal(?<rest>.*)$",
            RegexOptions.Compiled);

    private static readonly Regex SuggestedNote =
        new(@"^Suggested: (?<title>.+)$", RegexOptions.Compiled);

    public static string Translate(string? text)
    {
        if (string.IsNullOrWhiteSpace(text) || !Translator.IsVietnamese)
        {
            return text ?? string.Empty;
        }

        var trimmed = text.Trim();
        if (Vi.TryGetValue(trimmed, out var exact))
        {
            return exact;
        }

        var upload = UploadedPdfReference.Match(trimmed);
        if (upload.Success)
        {
            return $"PDF đã tải lên {upload.Groups["file"].Value}; trang {upload.Groups["page"].Value}; đoạn {upload.Groups["chunk"].Value}";
        }

        var review = ReviewTitle.Match(trimmed);
        if (review.Success)
        {
            return $"Xem xét {review.Groups["file"].Value}, trang {review.Groups["page"].Value}, đoạn {review.Groups["chunk"].Value}";
        }

        var note = SuggestedNote.Match(trimmed);
        if (note.Success)
        {
            return "Gợi ý: " + Translate(note.Groups["title"].Value);
        }

        var meal = FallbackMealTitle.Match(trimmed);
        if (meal.Success)
        {
            var name = Translate(meal.Groups["name"].Value);
            var category = Translate(meal.Groups["category"].Value);
            return $"Món {name.ToLowerInvariant()} – {category.ToLowerInvariant()}{meal.Groups["rest"].Value}";
        }

        // Titles such as "Green lentil farro bowl (legumes, simple meals)": translate the part before the tags.
        var open = trimmed.IndexOf(" (", StringComparison.Ordinal);
        if (open > 0 && trimmed.EndsWith(')') && Vi.TryGetValue(trimmed[..open], out var head))
        {
            return head + trimmed[open..];
        }

        return text;
    }

    public static string TranslateQuantity(string? quantity)
    {
        if (string.IsNullOrWhiteSpace(quantity) || !Translator.IsVietnamese)
        {
            return quantity ?? string.Empty;
        }

        var result = quantity.Trim();
        foreach (var (english, vietnamese) in QuantityPhrases)
        {
            result = Regex.Replace(result, $@"\b{Regex.Escape(english)}\b", vietnamese, RegexOptions.IgnoreCase);
        }

        return result;
    }
}
