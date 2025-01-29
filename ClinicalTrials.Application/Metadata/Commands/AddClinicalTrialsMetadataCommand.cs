using ClinicalTrials.Application.Extensions;
using ClinicalTrials.Application.JsonConverters;
using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Domain.Entities;
using MediatR;
using Serilog;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ClinicalTrials.Application.Metadata.Commands;

public class AddClinicalTrialsMetadataCommand : IRequest<ClinicalTrialMetadata?>
{
    public string TrialId { get; init; }
    public string Title { get; init; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; init; }
    public int Participants { get; init; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly StartDate { get; init; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly? EndDate { get; init; }
}

public enum Status
{
    [EnumMember(Value = "Not Started")]
    NotStarted,
    [EnumMember(Value = "Ongoing")]
    Ongoing,
    [EnumMember(Value = "Completed")]
    Completed
}

public class AddClinicalTrialsMetadataCommandHandler : IRequestHandler<AddClinicalTrialsMetadataCommand, ClinicalTrialMetadata?>
{
    private readonly IClinicalTrialsMetadataRepository _clinicalTrialsMetadataRepository;

    public AddClinicalTrialsMetadataCommandHandler(IClinicalTrialsMetadataRepository clinicalTrialsMetadataRepository)
    {
        _clinicalTrialsMetadataRepository = clinicalTrialsMetadataRepository;
    }

    public async Task<ClinicalTrialMetadata?> Handle(AddClinicalTrialsMetadataCommand request, CancellationToken cancellationToken)
    {
        try
        {
            DateOnly? endDate;
            if (!request.EndDate.HasValue)
            {
                endDate = request.Status == Status.Ongoing ? request.StartDate.AddMonths(1) : null;
            }
            else
            {
                endDate = request.EndDate;
            }

            var metadata = new ClinicalTrialMetadata
            {
                TrialId = request.TrialId,
                Title = request.Title,
                Status = request.Status.ToEnumMemberAttributeValue(),
                Participants = request.Participants,
                StartDate = request.StartDate,
                EndDate = endDate,
                Duration = endDate.HasValue ? endDate.Value.DayNumber - request.StartDate.DayNumber : null
            };

            var result = await _clinicalTrialsMetadataRepository.CreateAsync(metadata, cancellationToken);

            return result ? metadata : null;

            // return metadata;
        }
        catch (Exception ex)
        {
            Log.Error($"AddClinicalTrialsMetadataCommandHandler (Handle): {ex.Message}");

            return null;
        }
    }
}
