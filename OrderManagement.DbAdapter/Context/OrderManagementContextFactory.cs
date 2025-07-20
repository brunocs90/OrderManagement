using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace OrderManagement.DbAdapter.Context;

public class OrderManagementContextFactory : IDesignTimeDbContextFactory<OrderManagementContext>
{
    public OrderManagementContext CreateDbContext(string[] args)
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Path.Combine(TryGetSolutionDirectoryInfo().FullName, "OrderManagement.Api"));
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            }).Build();

        var configuration = (IConfiguration)builder.Services.GetService(typeof(IConfiguration))!;
        var connectionString = configuration.GetValue<string>("DbAdapterConfiguration:ConnectionString");

        var optionsBuilder = new DbContextOptionsBuilder<OrderManagementContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new OrderManagementContext(optionsBuilder.Options);
    }

    private static DirectoryInfo TryGetSolutionDirectoryInfo()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null && directory.GetFiles("*.sln").Length == 0)
            directory = directory.Parent;

        return directory!;
    }
}