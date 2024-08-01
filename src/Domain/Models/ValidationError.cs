namespace NetCoreMinimalApi.Domain.Models;

public class ValidationError
{
    public required string Field { get; set; }

    public required string Message { get; set; }

    public object? AttemptedValue { get; set; }
}
