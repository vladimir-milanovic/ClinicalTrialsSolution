using Microsoft.Extensions.DependencyInjection;

namespace ClinicalTrials.IntegrationTests.Server;

public static class ServiceCollectionExtensions
{
    public static void RemoveService(this IServiceCollection services, params Type[] serviceTypes)
    {
        var descriptors = services.Where(d => serviceTypes.Contains(d.ServiceType)).ToArray();

        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }
}
