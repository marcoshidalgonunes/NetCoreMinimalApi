using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NetCoreMinimalApi.Services;

internal static class OAuth2Service
{
    public static IServiceCollection AddOAuth2(this IServiceCollection services,
                                               IConfiguration configuration)
    {
        _ = bool.TryParse(configuration["OIDC:RequireHttpsMetadata"], out bool requireHttpsMetadata);
        var authority = configuration["OIDC:Authority"];

        _ = services
            .AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.Authority = authority;
                options.MetadataAddress = configuration["OIDC:MetadataAddress"] ?? throw new ArgumentNullException("OIDC:MetadataAddress");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authority,
                    ValidAudiences = [configuration["OIDC:ClientId"], "account"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["OIDC:ClientSecret"] ?? string.Empty))
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