using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClinicalTrials.Infrastructure.Persistence.Repositories;

public class ClinicalTrialsMetadataRepository : IClinicalTrialsMetadataRepository
{
    private readonly ClinicalTrialsDbContext _context;

    public ClinicalTrialsMetadataRepository(ClinicalTrialsDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(ClinicalTrialMetadata metadata, CancellationToken cancellationToken)
    {
        await _context.ClinicalTrialMetadata.AddAsync(metadata, cancellationToken);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<ClinicalTrialMetadata[]> GetAsync(Expression<Func<ClinicalTrialMetadata, bool>> filter, CancellationToken cancellationToken)
    {
        var metadataArray = await _context.ClinicalTrialMetadata
            .Where(filter)
            .ToArrayAsync(cancellationToken);

        return metadataArray;
    }

    public async Task<ClinicalTrialMetadata?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var metadata = await _context.ClinicalTrialMetadata.FindAsync(id, cancellationToken);

        return metadata;
    }
}
