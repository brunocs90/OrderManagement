using OrderManagement.DbAdapter.Context;

namespace OrderManagement.Api.Extensions;

public static class ApplicationInitializationExtensions
{
    public static WebApplication InitializeDatabase(this WebApplication app)
    {
        var env = app.Environment;
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<OrderManagementContext>();
                if (env.IsDevelopment())
                {
                    // Chama o método de extensão para realizar o seeding, se necessário.
                    context.EnsureSeedData();
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred seeding the DB.");
            }
        }

        return app;
    }
}
