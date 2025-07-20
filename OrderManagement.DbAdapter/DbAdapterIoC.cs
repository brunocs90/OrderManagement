using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Security.Adapters;
using OrderManagement.DbAdapter.Context;
using OrderManagement.DbAdapter.Pedidos.Adapters;
using OrderManagement.DbAdapter.Security.Adapters;

namespace OrderManagement.DbAdapter;

public static class DbAdapterIoC
{
    public static IServiceCollection AddDbAdapterIoC(this IServiceCollection services, DbAdapterConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDbContext<OrderManagementContext>(options => options.UseSqlServer(configuration.ConnectionString));
        services.AddSingleton(configuration);

        services.AddScoped<IUserDbAdapter, UserDbAdapter>();
        services.AddScoped<IOrderDbAdapter, OrderDbAdapter>();

        return services;
    }
}