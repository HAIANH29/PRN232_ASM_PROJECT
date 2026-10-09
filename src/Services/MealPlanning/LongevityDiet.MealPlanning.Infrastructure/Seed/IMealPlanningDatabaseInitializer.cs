namespace LongevityDiet.MealPlanning.Infrastructure.Seed;

public interface IMealPlanningDatabaseInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}
