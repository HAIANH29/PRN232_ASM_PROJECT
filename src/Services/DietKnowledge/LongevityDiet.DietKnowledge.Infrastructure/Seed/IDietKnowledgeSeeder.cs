namespace LongevityDiet.DietKnowledge.Infrastructure.Seed;

public interface IDietKnowledgeSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
