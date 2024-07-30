using Microsoft.IdentityModel.Tokens;

namespace NetCoreMinimalApi.Services;

internal static class OAuth2Service
{
    public static IServiceCollection AddOAuth2(this IServiceCollection services,
                                               IConfiguration configuration)
    {
        _ = services
            .AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = !$"{configuration["Keycloak:ssl-required"]}".Equals("None", StringComparison.InvariantCultureIgnoreCase);
                options.MetadataAddress = $"{configuration["Keycloak:auth-server-url"]}/realms/{configuration["Keycloak:realm"]}/.well-known/openid-configuration";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Keycloak:auth-server-url"],
                    ValidAudiences = [$"{configuration["Keycloak:resource"]}", "account"]
                };
            });

        _ = services.AddAuthorizationBuilder()
            .AddPolicy("Default", policy =>
            {
                policy.RequireAuthenticatedUser();
            });

        return services;
    }
}