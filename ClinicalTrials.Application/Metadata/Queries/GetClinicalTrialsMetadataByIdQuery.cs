using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Domain.Entities;
using MediatR;

namespace ClinicalTrials.Application.Metadata.Queries;

public record GetClinicalTrialsMetadataByIdQuery(string Id) : IRequest<ClinicalTrialMetadata?>;


public class GetClinicalTrialsMetadataByIdQueryHandler : IRequestHandler<GetClinicalTrialsMetadataByIdQuery, ClinicalTrialMetadata?>
{
    // private readonly ILogger<GetClinicalTrialsMetadataQueryHandler> _logger;
    private readonly IClinicalTrialsMetadataRepository _clinicalTrialsMetadataRepository;

    public GetClinicalTrialsMetadataByIdQueryHandler(
        IClinicalTrialsMetadataRepository clinicalTrialsMetadataRepository)
    {
        _clinicalTrialsMetadataRepository = clinicalTrialsMetadataRepository;
    }

    public async Task<ClinicalTrialMetadata?> Handle(GetClinicalTrialsMetadataByIdQuery request, CancellationToken cancellationToken)
    {
        var metadata = await _clinicalTrialsMetadataRepository.GetByIdAsync(request.Id, cancellationToken);

        return metadata;
    }
}