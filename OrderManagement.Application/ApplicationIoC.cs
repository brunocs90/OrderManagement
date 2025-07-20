using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.Pedidos.UseCases.Implementations;
using OrderManagement.Application.Pedidos.UseCases.Interfaces;
using OrderManagement.Application.Security.UseCases.Implementations;
using OrderManagement.Application.Security.UseCases.Interfaces;

namespace OrderManagement.Application;

public static class ApplicationIoC
{
    public static IServiceCollection AddApplicationIoC(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IGetOrders, GetOrders>();
        services.AddScoped<IGetOrderById, GetOrderById>();
        services.AddScoped<ICreateOrder, CreateOrder>();
        services.AddScoped<IDeleteOrder, DeleteOrder>();
        services.AddScoped<IUpdateOrder, UpdateOrder>();
        services.AddScoped<IProcessOrders, ProcessOrders>();
        services.AddScoped<IGetOrdersByStatus, GetOrdersByStatus>();

        services.AddScoped<IAuthenticateUserPortal, AuthenticateUserPortal>();

        return services;
    }
}