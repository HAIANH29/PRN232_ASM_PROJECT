namespace LongevityDiet.MealPlanning.Application.Common;

public static class MealSlots
{
    public const string Breakfast = "Breakfast";
    public const string Lunch = "Lunch";
    public const string Dinner = "Dinner";
    public const string Snack = "Snack";

    public static readonly IReadOnlyCollection<string> All =
    [
        Breakfast,
        Lunch,
        Dinner,
        Snack
    ];

    public static bool IsValid(string? value)
    {
        return All.Any(slot => string.Equals(slot, value, StringComparison.OrdinalIgnoreCase));
    }

    public static string Normalize(string value)
    {
        return All.First(slot => string.Equals(slot, value, StringComparison.OrdinalIgnoreCase));
    }

    public static TimeOnly DefaultReminderTimeUtc(string mealSlot)
    {
        return Normalize(mealSlot) switch
        {
            Breakfast => new TimeOnly(7, 0),
            Lunch => new TimeOnly(11, 0),
            Dinner => new TimeOnly(17, 0),
            _ => new TimeOnly(15, 0)
        };
    }
}
