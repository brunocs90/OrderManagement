using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OrderManagement.Api.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrderManagement.Api.Extensions.Swagger;
/// <summary>
/// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
/// </summary>
/// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    private const string COMPANY_WEB_SITE = "https://OrderManagement.com.br/";
    private const string LICENSE_SITE = "https://opensource.org/licenses/MIT";

    private readonly IApiVersionDescriptionProvider _provider = provider;

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        options.SchemaFilter<OrderStatusSchemaFilter>();
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "OrderManagement",
            Version = description.ApiVersion.ToString(),
            Description = "OrderManagement",
            Contact = new OpenApiContact() { Name = "Adicionar o nome do Time", Email = "adicionaremail@email...." },
            TermsOfService = new Uri(COMPANY_WEB_SITE),
            License = new OpenApiLicense() { Name = "MIT", Url = new Uri(LICENSE_SITE) },
        };

        if (description.IsDeprecated)
        {
            info.Description += " A API foi depreciada e será removida em breve.";
        }

        return info;
    }
}