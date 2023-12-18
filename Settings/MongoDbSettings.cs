namespace NetCoreMinimalApi.Settings;

public sealed class MongoDbSettings : IMongoDbSettings
{
    public required string CollectionName { get; set; }

    public required string ConnectionString { get; set; }

    public required string DatabaseName { get; set; }
}
