namespace NetCoreMinimalApi.Domain.Models;

public abstract class Entity<TIdentifier>
{
    public abstract TIdentifier? Id { get; set; }
}
