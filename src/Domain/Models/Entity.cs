using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NetCoreMinimalApi.Domain.Models;

public abstract class Entity<TIdentifier>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public abstract TIdentifier? Id { get; set; }
}
