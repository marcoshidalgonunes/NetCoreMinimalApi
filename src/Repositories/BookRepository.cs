using MongoDB.Bson;
using MongoDB.Driver;
using NetCoreMinimalApi.Models;
using NetCoreMinimalApi.Settings;
using System.Text.RegularExpressions;

namespace NetCoreMinimalApi.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IMongoCollection<Book> _collection;

    public BookRepository(IMongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _collection = database.GetCollection<Book>(settings.CollectionName);
    }

    public async Task<Book> CreateAsync(Book item)
    {
        await _collection.InsertOneAsync(item);
        return item;
    }

    public async Task<List<Book>> ReadAllAsync()
    {
        var items = await _collection.FindAsync(item => true);

        return await items.ToListAsync();
    }

    public async Task<List<Book>> ReadByCriteriaAsync(string criteria, string search)
    {
        var queryExpr = new BsonRegularExpression(new Regex(search, RegexOptions.IgnoreCase));
        var builder = Builders<Book>.Filter;
        var filter = builder.Regex(criteria, queryExpr);

        var items = await _collection.FindAsync(filter);

        return await items.ToListAsync();
    }

    public async Task<Book> ReadByIdAsync(string? id)
    {
        using var items = await _collection.FindAsync(item => item.id!.Equals(id));

        return await items.FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Book itemIn) =>
        await _collection.ReplaceOneAsync(item => item.id!.Equals(itemIn.id), itemIn);

    public async Task DeleteAsync(string? id) =>
        await _collection.DeleteOneAsync(item => item.id!.Equals(id));
}