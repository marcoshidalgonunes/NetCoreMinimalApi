using Microsoft.Extensions.Options;
using NetCoreMinimalApi.Domain.Mappers;
using NetCoreMinimalApi.Domain.Models;
using NetCoreMinimalApi.Repositories;
using NetCoreMinimalApi.Services;
using NetCoreMinimalApi.Settings;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

EntityMapper.Map<Book, string?>();

// requires using Microsoft.Extensions.Options
services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection(nameof(MongoDbSettings)));

services.AddSingleton<IMongoDbSettings>(sp =>
       sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

services.AddSingleton<IRepository<Book, string?>, BookRepository>();

// Add services to the container.
services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerOAuth2(configuration);
builder.Services.AddOAuth2(configuration);

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

app.MapGet("/books", async (IRepository<Book, string?> db) =>
{
    return await db.ReadAllAsync();

});
app.MapGet("/book/{id}", async (string? id, IRepository<Book, string?> db) =>
{
    return await db.ReadByIdAsync(id) is Book book
        ? Results.Ok(book)
        : Results.NotFound();
});
app.MapGet("/books/{criteria}/{search}", async (string criteria, string search, IRepository<Book, string?> db) =>
{
    return await db.ReadByCriteriaAsync(criteria, search);
});
app.MapPost("/book", async (Book book, IRepository<Book, string?> db) =>
{
    await db.CreateAsync(book);

    return Results.Created($"/book/{book.Id}", book);
});
app.MapPut("/book", async (Book bookIn, IRepository<Book, string?> db) =>
{
    if (await db.ReadByIdAsync(bookIn.Id) is Book book)
    {
        await db.UpdateAsync(bookIn);
        return Results.NoContent();
    }

    return Results.NotFound();
});
app.MapDelete("/book/{id}", async (string? id, IRepository<Book, string?> db) =>
{
    if (await db.ReadByIdAsync(id) is Book book)
    {
        await db.DeleteAsync(id);
        return Results.NoContent();
    }

    return Results.NotFound();
});


app.Run();
