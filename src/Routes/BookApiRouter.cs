using NetCoreMinimalApi.Models;
using NetCoreMinimalApi.Repositories;

namespace NetCoreMinimalApi.Routes;

internal static class BookApiRouter
{
    internal static RouteGroupBuilder AddBookApi(this RouteGroupBuilder group)
    {
        group.MapGet(string.Empty, async (IBookRepository db) =>
        {
            return await db.ReadAllAsync();
        });

        group.MapGet("/{id}", async (string? id, IBookRepository db) =>
        {
            return await db.ReadByIdAsync(id) is Book book
                ? Results.Ok(book)
                : Results.NotFound();
        });

        group.MapGet("/{criteria}/{search}", async (string criteria, string search, IBookRepository db) =>
        {
            return await db.ReadByCriteriaAsync(criteria, search);
        });

        group.MapPost(string.Empty, async (Book book, IBookRepository db) =>
        {
            await db.CreateAsync(book);

            return Results.Created($"/{book.id}", book);
        });

        group.MapPut(string.Empty, async (Book bookIn, IBookRepository db) =>
        {
            if (await db.ReadByIdAsync(bookIn.id) is Book book)
            {
                await db.UpdateAsync(bookIn);
                return Results.NoContent();
            }

            return Results.NotFound();
        });

        group.MapDelete("/{id}", async (string? id, IBookRepository db) =>
        {
            if (await db.ReadByIdAsync(id) is Book book)
            {
                await db.DeleteAsync(id);
                return Results.NoContent();
            }

            return Results.NotFound();
        });

        return group;
    }
}
