using CatchTheRabbit.Core.Interfaces;
using CatchTheRabbit.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CatchTheRabbit.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _testDbPath;

    public CustomWebApplicationFactory()
    {
        _testDbPath = Path.Combine(Path.GetTempPath(), $"integration_test_{Guid.NewGuid()}.db");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing repository registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(ILeaderboardRepository));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add test repository with in-memory SQLite
            services.AddSingleton<ILeaderboardRepository>(sp =>
                new SqliteLeaderboardRepository($"Data Source={_testDbPath}"));
        });

        builder.UseEnvironment("Testing");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (File.Exists(_testDbPath))
        {
            try { File.Delete(_testDbPath); } catch { }
        }
    }
}
