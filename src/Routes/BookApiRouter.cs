using NetCoreMinimalApi.Domain.Models;
using NetCoreMinimalApi.Repositories;

namespace NetCoreMinimalApi.Routes;

internal static class BookApiRouter
{
    private const string apiName = "/Books";

    internal static RouteGroupBuilder AddBookApi(this RouteGroupBuilder group)
    {
        group.MapGet(apiName, async (IBookRepository db) =>
        {
            return await db.ReadAllAsync();
        });

        group.MapGet($"{apiName}/{{id}}", async (string? id, IBookRepository db) =>
        {
            return await db.ReadByIdAsync(id) is Book book
                ? Results.Ok(book)
                : Results.NotFound();
        });

        group.MapGet($"{apiName}/{{criteria}}/{{search}}", async (string criteria, string search, IBookRepository db) =>
        {
            return await db.ReadByCriteriaAsync(criteria, search);
        });

        group.MapPost(apiName, async (Book book, IBookRepository db) =>
        {
            await db.CreateAsync(await book.ValidateAsync());

            return Results.Created($"/{book.id}", book);
        });

        group.MapPut(apiName, async (Book bookIn, IBookRepository db) =>
        {
            if (await db.ReadByIdAsync(bookIn.id) is Book book)
            {
                await db.UpdateAsync(await bookIn.ValidateAsync());
                return Results.NoContent();
            }

            return Results.NotFound();
        });

        group.MapDelete($"{apiName}/{{id}}", async (string? id, IBookRepository db) =>
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
