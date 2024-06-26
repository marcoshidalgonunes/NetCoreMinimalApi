﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NetCoreMinimalApi.Models;

public sealed class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public required string Name { get; set; }

    public required double Price { get; set; }

    public required string Category { get; set; }

    public required string Author { get; set; }
}
