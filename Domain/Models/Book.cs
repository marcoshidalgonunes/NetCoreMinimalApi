namespace NetCoreMinimalApi.Domain.Models;

public sealed class Book : Entity<string?>
{
    public override string? Id { get; set; }

    public required string Name { get; set; }

    public required double Price { get; set; }

    public required string Category { get; set; }

    public required string Author { get; set; }
}
