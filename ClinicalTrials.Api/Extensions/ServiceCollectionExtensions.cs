using ClinicalTrials.Application.Interfaces;
using ClinicalTrials.Application.Metadata.Queries;
using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Application.Services;
using ClinicalTrials.Infrastructure.Persistence;
using ClinicalTrials.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrials.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            cfg.RegisterServicesFromAssembly(typeof(GetClinicalTrailsMetadataQueryHandler).Assembly);
        });

        return services;
    }

    public static IServiceCollection ConfigurePersistence(
        this IServiceCollection services,
        IConfiguration configuration) 
    {
        var connection = configuration.GetConnectionString("ClinicalTrialsDbContext");
        services.AddDbContext<ClinicalTrialsDbContext>(options =>
            options.UseSqlServer(connection));

        return services;
    }

    public static IServiceCollection AddServiceCollection(this IServiceCollection services)
    {
        services.AddScoped<IClinicalTrialsMetadataRepository, ClinicalTrialsMetadataRepository>();
        services.AddScoped<IJsonService, JsonService>();

        return services;
    }
}
