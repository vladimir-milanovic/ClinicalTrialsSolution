using ClinicalTrials.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicalTrials.IntegrationTests.Server;

public class ClinicalTrialTestApplicationFactory(
        Dictionary<Type, object>? mockServices = null,
        string? databaseName = null)
    : WebApplicationFactory<Program>
{
    private readonly Dictionary<Type, object> _mockServices = mockServices ?? new Dictionary<Type, object>();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveService(
                typeof(DbContextOptions<ClinicalTrialsDbContext>));

            services.AddDbContext<ClinicalTrialsDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString());
            });

            // Add the mock services
            foreach (var mockService in _mockServices)
            {
                services.AddSingleton(mockService.Key, mockService.Value);
            }
        });

        builder.UseEnvironment("Testing");
    }
}
