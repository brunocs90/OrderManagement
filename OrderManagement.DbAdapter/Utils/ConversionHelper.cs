using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderManagement.Shared.Extensions;

namespace OrderManagement.DbAdapter.Utils;

public static class ConversionHelper
{
    public static ValueConverter<TEnum, string> CreateEnumConverter<TEnum>()
        where TEnum : struct, Enum
    {
        return new ValueConverter<TEnum, string>(
            toDb => toDb.ToCharString(),
            fromDb => fromDb.ToEnum<TEnum>());
    }
}