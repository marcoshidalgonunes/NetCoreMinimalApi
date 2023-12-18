using Microsoft.OpenApi.Models;

namespace NetCoreMinimalApi.Services;

public static class SwaggerGenOAuth2Service
{
    private const string SECURITY_SCHEMA = "OAuth2";

    public static IServiceCollection AddSwaggerOAuth2(this IServiceCollection services,
                                               IConfiguration configuration)
    {
        _ = services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weather Forecast API v1.0", Version = "v1" });
            c.AddSecurityDefinition(SECURITY_SCHEMA, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{configuration["Keycloak:auth-server-url"]}/realms/{configuration["Keycloak:realm"]}/protocol/openid-connect/auth"),

                    }
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = SECURITY_SCHEMA //The name of the previously defined security scheme.
						}
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
