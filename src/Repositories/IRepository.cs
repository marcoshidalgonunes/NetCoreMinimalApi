using NetCoreMinimalApi.Domain.Models;

namespace NetCoreMinimalApi.Repositories;

public interface IRepository<TEntity, TIdentifier>
    where TEntity : Entity<TIdentifier>
{
    Task<TEntity> CreateAsync(TEntity item);

    Task<List<TEntity>> ReadAllAsync();

    Task<List<TEntity>> ReadByCriteriaAsync(string criteria, string search);

    Task<TEntity> ReadByIdAsync(TIdentifier id);

    Task UpdateAsync(TEntity itemIn);

    Task DeleteAsync(TIdentifier id);
}