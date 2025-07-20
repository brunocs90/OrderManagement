using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrderManagement.Api.Extensions.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            // add a custom operation filter which sets default values
            options.OperationFilter<SwaggerDefaultValues>();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = @"Copie no campo abaixo a palavra 'bearer' seguida do token JWT de autenticação.<br>
                                <i>Ex: bearer jwt_token</i><br><br>
                                <b>Esse token é obtido usando o endpoint '/autenticacao'</b><br><br>",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseVersionedSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                // build a swagger endpoint for each discovered API version
                var groupNames = provider.ApiVersionDescriptions
                .Select(description => description.GroupName);

                foreach (var groupName in groupNames)
                {
                    options.SwaggerEndpoint($"{groupName}/swagger.json",
                        groupName.ToUpperInvariant());
                }
            });

        return app;
    }
}