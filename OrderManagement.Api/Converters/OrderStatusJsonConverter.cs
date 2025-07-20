using OrderManagement.Domain.Pedidos.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderManagement.Api.Converters;

public class OrderStatusJsonConverter : JsonConverter<OrderStatus>
{
    public override OrderStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrEmpty(value))
            return OrderStatus.Pending;

        return Enum.TryParse<OrderStatus>(value, true, out var result) ? result : OrderStatus.Pending;
    }

    public override void Write(Utf8JsonWriter writer, OrderStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}