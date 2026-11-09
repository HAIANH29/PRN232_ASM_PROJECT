using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pgvector.EntityFrameworkCore;

namespace LongevityDiet.DietKnowledge.Infrastructure.VectorDb;

// Only used by "dotnet ef" to create migrations; no database connection is made.
public sealed class VectorDbContextFactory : IDesignTimeDbContextFactory<VectorDbContext>
{
    public VectorDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<VectorDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=5437;Database=VectorDb;Username=postgres;Password=postgres",
                npgsql => npgsql.UseVector())
            .Options;

        return new VectorDbContext(options);
    }
}
