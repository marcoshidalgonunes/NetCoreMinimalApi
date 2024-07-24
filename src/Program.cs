using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NetCoreMinimalApi.Mappers;
using NetCoreMinimalApi.Repositories;
using NetCoreMinimalApi.Services;
using NetCoreMinimalApi.Settings;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// requires using Microsoft.Extensions.Options
services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

services.AddSingleton<IMongoDbSettings>(sp =>
       sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

services.AddSingleton<IBookRepository, BookRepository>();

// Add services to the container.
services.AddEndpointsApiExplorer();
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback || origin.StartsWith("https://"))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

if (!builder.Environment.IsProduction())
{
    services.AddSwaggerOAuth2(configuration);

    // Configure JSON options for Swagger
    services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
}

services.AddOAuth2(configuration);

// Configure JSON options
services.Configure<JsonOptions>(options => options.SerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.OAuthClientId(configuration["Keycloak:resource"]);
    });
}

app.UseHttpsRedirection();

app.MapGroup("/api/Books").MapBookApi();

app.Run();
