namespace LongevityDiet.MealPlanning.Application.Common;

/// <summary>
/// Decides what "now" and "already passed" mean for meal planning. Dates and meal slots are local
/// wall-clock concepts, so the UTC offset of the people using the app is configurable (default UTC+7).
/// </summary>
public sealed class MealSchedulePolicy(TimeSpan? utcOffset = null)
{
    public static readonly TimeSpan DefaultUtcOffset = TimeSpan.FromHours(7);

    public TimeSpan UtcOffset { get; } = utcOffset ?? DefaultUtcOffset;

    public DateOnly Today(DateTimeOffset nowUtc)
    {
        return DateOnly.FromDateTime(nowUtc.ToOffset(UtcOffset).DateTime);
    }

    /// <summary>True when the slot on that local date is already over (earlier day, or today past the slot end).</summary>
    public bool HasSlotEnded(DateOnly date, string mealSlot, DateTimeOffset nowUtc)
    {
        var local = nowUtc.ToOffset(UtcOffset).DateTime;
        var today = DateOnly.FromDateTime(local);
        if (date != today)
        {
            return date < today;
        }

        return TimeOnly.FromDateTime(local) >= MealSlots.EndTimeLocal(mealSlot);
    }

    public DateTimeOffset DefaultReminderAtUtc(DateOnly date, string mealSlot)
    {
        var local = date.ToDateTime(MealSlots.DefaultReminderTimeLocal(mealSlot), DateTimeKind.Unspecified);
        return new DateTimeOffset(local, UtcOffset).ToUniversalTime();
    }
}
