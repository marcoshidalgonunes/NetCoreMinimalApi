using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NetCoreMinimalApi.Domain.Models;

public sealed class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? id { get; set; }

    public required string Name { get; set; }

    public required double Price { get; set; }

    public required string Category { get; set; }

    public required string Author { get; set; }
}
