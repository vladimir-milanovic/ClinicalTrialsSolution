using ClinicalTrials.Application.Interfaces;
using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Domain.Entities;
using ClinicalTrials.IntegrationTests.Server;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace ClinicalTrials.IntegrationTests;

public class GetMetadataTests
{
    private ScopedFixture _fixture;
    private HttpClient _client;
    private readonly Mock<IClinicalTrialsMetadataRepository> _clinicalTrialsMetadataRepositoryMock = new();
    private readonly Mock<IJsonService> _jsonService = new();
    private readonly Mock<ILogger> _loggerMock = new();
    private readonly Mock<IMediator> _mediatorMock = new();


    public GetMetadataTests()
    {
        SetupHappyPath();

        if (_fixture == null || _client == null)
        {
            throw new InvalidOperationException("The test is not initialized.");
        }
    }

    private void SetupHappyPath()
    {
        var metadata = new ClinicalTrialMetadata
        {
            TrialId = "trial-123",
            Title = "Test Trial",
            Status = "Ongoing",
            Participants = 100,
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = null
        };

        _clinicalTrialsMetadataRepositoryMock.Setup(r => 
                r.GetAsync(It.IsAny<Expression<Func<ClinicalTrialMetadata, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] { metadata });

        var mockServices = new Dictionary<Type, object>
        {
            { typeof(IClinicalTrialsMetadataRepository), _clinicalTrialsMetadataRepositoryMock.Object },
            { typeof(IJsonService), _jsonService.Object },
            { typeof(ILogger), _loggerMock.Object },
            { typeof(IMediator), _mediatorMock.Object }
        };

        _fixture = new ScopedFixture(mockServices, Guid.NewGuid().ToString());

        _client = _fixture.CreateClient();
    }

    [Fact]
    public async Task GivenHappyPathSetup_BasicInfoStudentEndpoint_ShouldReturn_InternalServerError()
    {
        // Arrange
        var requestUri = "clinical-trials/metadata";

        // Act
        var response = await _client.GetAsync(requestUri, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
