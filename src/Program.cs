using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using NetCoreMinimalApi.Routes;
using NetCoreMinimalApi.Repositories;
using NetCoreMinimalApi.Services;
using NetCoreMinimalApi.Settings;
using NetCoreMinimalApi.Middleware;
using FluentValidation;
using NetCoreMinimalApi.Domain.Validation;
using NetCoreMinimalApi.Domain.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var isProduction = builder.Environment.IsProduction();

// requires using Microsoft.Extensions.Options
services.Configure<MongoDbSettings>(
    configuration.GetSection(nameof(MongoDbSettings)));

services.AddSingleton<IMongoDbSettings>(sp =>
       sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

services.AddScoped<IBookRepository, BookRepository>();

services.AddScoped<IValidator<Book>, BookValidator>();

// Add services to the container.
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback || origin.StartsWith("https://"))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

if (!isProduction)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerOAuth2(configuration);

    // Configure JSON options for Swagger
    services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
}

services.AddOAuth2(configuration);

// Configure JSON options
services.Configure<JsonOptions>(options => options.SerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

// Register the exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (!isProduction)
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.OAuthClientId(configuration["Keycloak:resource"]);
    });
}

app.UseHttpsRedirection();

app.MapGroup("/api")
    .AddBookApi()
    .RequireAuthorization();

app.Run();
