using AutoMapper;
using ClinicalTrials.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicalTrials.IntegrationTests.Server;
public class ScopedFixture : IDisposable
{
    private readonly ClinicalTrialTestApplicationFactory _webApplicationFactory;
    private readonly ClinicalTrialsDbContext _clinicalTrialsDbContext;
    private readonly IServiceProvider _serviceProvider;

    public IMapper Mapper => _serviceProvider.GetRequiredService<IMapper>();

    public ScopedFixture(Dictionary<Type, object>? mockServices = null, string? databaseName = null)
    {
        _webApplicationFactory = new ClinicalTrialTestApplicationFactory(mockServices, databaseName);

        var scope = _webApplicationFactory.Services.CreateScope();

        _serviceProvider = scope.ServiceProvider;

        _clinicalTrialsDbContext = _serviceProvider.GetRequiredService<ClinicalTrialsDbContext>();
    }

    public HttpClient CreateClient() => _webApplicationFactory.CreateClient();

    public void Dispose()
    {
        _webApplicationFactory.Dispose();
        _clinicalTrialsDbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}
