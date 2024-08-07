﻿using FluentValidation;
using NetCoreMinimalApi.Domain.Models;

namespace NetCoreMinimalApi.Domain.Validation;

internal sealed class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name not informed");

        RuleFor(p => p.Author)
            .NotEmpty().WithMessage("Author not informed");

        RuleFor(p => p.Category)
            .NotEmpty().WithMessage("Category not informed");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Invalid price");
    }
}
