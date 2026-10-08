namespace LongevityDiet.Identity.Domain.Entities;

public sealed class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<User> Users { get; } = new List<User>();
}
