using ClinicalTrials.Api.Extensions;
using ClinicalTrials.Api.Validators;
using ClinicalTrials.Application.Interfaces;
using ClinicalTrials.Application.Metadata.Commands;
using ClinicalTrials.Application.Metadata.Queries;
using ClinicalTrials.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace ClinicalTrials.Api.Controllers;

[ApiController]
[Route("clinical-trials")]
public class ClinicalTrialsMetadataController : ControllerBase
{
    private readonly ILogger<ClinicalTrialsMetadataController> _logger;
    private readonly IJsonService _jsonService;
    private readonly IMediator _mediator;

    public ClinicalTrialsMetadataController(
        ILogger<ClinicalTrialsMetadataController> logger,
        IJsonService jsonService,
        IMediator mediator)
    {
        _logger = logger;
        _jsonService = jsonService;
        _mediator = mediator;
    }

    [HttpGet("metadata")]
    public async Task<Results<Ok<ClinicalTrialMetadata[]>, NotFound<string>>> Get(
        [FromQuery] GetClinicalTrialsMetadataQuery request, 
        CancellationToken cancellationToken)
    {
        var metadataArray = await _mediator.Send(request, cancellationToken);

        if (metadataArray == null || metadataArray.Length == 0)
            return TypedResults.NotFound("No metadata found");

        return TypedResults.Ok(metadataArray);
    }

    [HttpGet("metadata/{id}")]
    public async Task<Results<Ok<ClinicalTrialMetadata>, NotFound<string>> >GetById(
        [FromRoute] string id, 
        CancellationToken cancellationToken)
    {
        var query = new GetClinicalTrialsMetadataByIdQuery(id);

        var metadata = await _mediator.Send(query, cancellationToken);

        if (metadata == null)
            return TypedResults.NotFound("Metadata not found");

        return TypedResults.Ok(metadata);
    }

    [HttpPost("metadata")]
    public async Task<Results<Ok<ClinicalTrialMetadata>, BadRequest<string>, Conflict<string>>> Upload(
        [FromForm] UploadRequest file,
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(file.File.OpenReadStream());
        var jsonString = await reader.ReadToEndAsync(cancellationToken);

        var assembly = Assembly.GetEntryAssembly();
        var schema = assembly?.GetJsonSchemaContent();

        if (string.IsNullOrEmpty(jsonString) || !_jsonService.IsValid(jsonString, schema))
            return TypedResults.BadRequest("Invalid JSON content");

        var metadataRequest = _jsonService.Deserialize<AddClinicalTrialsMetadataCommand>(jsonString);

        var metadata = await _mediator.Send(metadataRequest, cancellationToken);

        if (metadata == null)
            return TypedResults.Conflict("Cannot add metadata in DB");

        return TypedResults.Ok(metadata);
    }
}
