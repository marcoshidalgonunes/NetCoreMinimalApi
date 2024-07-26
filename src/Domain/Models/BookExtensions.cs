using FluentValidation;
using NetCoreMinimalApi.Domain.Validation;

namespace NetCoreMinimalApi.Domain.Models;

internal static class BookExtensions
{
    private static readonly IValidator<Book> _validator = new BookValidator();

    internal static async Task<Book> ValidateAsync(this Book book)
    {
        await _validator.ValidateAndThrowAsync(book);
        return book;
    }
}
