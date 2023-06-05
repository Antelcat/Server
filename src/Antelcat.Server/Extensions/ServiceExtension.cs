using System.Net;
using Antelcat.Server.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Antelcat.Server.Extensions;

public static partial class ServiceExtension
{
    public static IServiceCollection AddJwtSwaggerGen(this IServiceCollection collection)
        => collection.AddSwaggerGen(static o =>
        {
            o.OperationFilter<AuthorizationFilter>();
            o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Jwt Token Format like [ Bearer {Token} ]",
                Name = nameof(Authorization),
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
        });

   
}