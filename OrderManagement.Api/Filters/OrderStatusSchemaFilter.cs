using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using OrderManagement.Domain.Pedidos.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrderManagement.Api.Filters;

public class OrderStatusSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(OrderStatus))
        {
            schema.Enum = new List<IOpenApiAny>
            {
                new OpenApiString("Pending"),
                new OpenApiString("Calculated"),
                new OpenApiString("Sent"),
                new OpenApiString("Cancelled")
            };
            schema.Description = "Order status: Pending, Calculated, Sent, Cancelled";
        }
    }
}