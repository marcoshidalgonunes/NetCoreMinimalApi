using NetCoreMinimalApi.Domain.Models;

namespace NetCoreMinimalApi.Repositories;

public interface IBookRepository
{
    Task<Book> CreateAsync(Book item);

    Task<List<Book>> ReadAllAsync();

    Task<List<Book>> ReadByCriteriaAsync(string criteria, string search);

    Task<Book> ReadByIdAsync(string? id);

    Task UpdateAsync(Book itemIn);

    Task DeleteAsync(string? id);
}