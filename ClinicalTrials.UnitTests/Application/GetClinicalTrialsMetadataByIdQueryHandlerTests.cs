using ClinicalTrials.Application.Metadata.Queries;
using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Domain.Entities;
using Moq;

namespace ClinicalTrials.UnitTests.Application;

public class GetClinicalTrialsMetadataByIdQueryHandlerTests
{
    private readonly Mock<IClinicalTrialsMetadataRepository> _repositoryMock;
    private readonly GetClinicalTrialsMetadataByIdQueryHandler _handler;

    public GetClinicalTrialsMetadataByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IClinicalTrialsMetadataRepository>();
        _handler = new GetClinicalTrialsMetadataByIdQueryHandler(_repositoryMock.Object);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task Handle_ReturnsMetadata_WhenMetadataExists(
        ClinicalTrialMetadata metadata)
    {
        // Arrange
        metadata.TrialId = "trial-123";

        _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(metadata);

        var query = new GetClinicalTrialsMetadataByIdQuery("trial-123");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("trial-123", result.TrialId);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenMetadataDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClinicalTrialMetadata)null);

        var query = new GetClinicalTrialsMetadataByIdQuery("trial-123");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
