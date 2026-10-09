using LongevityDiet.MealPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LongevityDiet.MealPlanning.Infrastructure.Seed;

public sealed class MealPlanningDatabaseInitializer(
    MealPlanningDbContext dbContext) : IMealPlanningDatabaseInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
