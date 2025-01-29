using ClinicalTrials.Domain.Entities;
using System.Linq.Expressions;

namespace ClinicalTrials.Application.Repositories;

public interface IClinicalTrialsMetadataRepository
{
    Task<bool> CreateAsync(ClinicalTrialMetadata metadata, CancellationToken cancellationToken);
    
    Task<ClinicalTrialMetadata[]> GetAsync(
        Expression<Func<ClinicalTrialMetadata, bool>> filter, 
        CancellationToken cancellationToken);
    
    Task<ClinicalTrialMetadata?> GetByIdAsync(string id, CancellationToken cancellationToken);
}
