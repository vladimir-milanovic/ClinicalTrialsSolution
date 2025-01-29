using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

namespace ClinicalTrials.Application.Metadata.Queries;

public record GetClinicalTrialsMetadataQuery(
    string?Title,
    string? Status
    ) : IRequest<ClinicalTrialMetadata[]>;

public class GetClinicalTrailsMetadataQueryHandler : IRequestHandler<GetClinicalTrialsMetadataQuery, ClinicalTrialMetadata[]>
{
    private readonly IClinicalTrialsMetadataRepository _clinicalTrialsMetadataRepository;

    public GetClinicalTrailsMetadataQueryHandler(IClinicalTrialsMetadataRepository clinicalTrialsMetadataRepository)
    {
        _clinicalTrialsMetadataRepository = clinicalTrialsMetadataRepository;
    }

    public async Task<ClinicalTrialMetadata[]> Handle(GetClinicalTrialsMetadataQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<ClinicalTrialMetadata, bool>> filter = metadata =>
            (string.IsNullOrEmpty(request.Title) || metadata.Title.Contains(request.Title)) &&
            (string.IsNullOrEmpty(request.Status) || metadata.Status == request.Status);

        var result = await _clinicalTrialsMetadataRepository.GetAsync(filter, cancellationToken);

        return result;
    }
}