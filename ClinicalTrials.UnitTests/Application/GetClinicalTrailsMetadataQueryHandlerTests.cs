using ClinicalTrials.Application.Metadata.Queries;
using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Domain.Entities;
using Moq;
using System.Linq.Expressions;

namespace ClinicalTrials.UnitTests.Application;

public class GetClinicalTrailsMetadataQueryHandlerTests
{
    private readonly Mock<IClinicalTrialsMetadataRepository> _repositoryMock;
    private readonly GetClinicalTrailsMetadataQueryHandler _handler;

    public GetClinicalTrailsMetadataQueryHandlerTests()
    {
        _repositoryMock = new Mock<IClinicalTrialsMetadataRepository>();
        _handler = new GetClinicalTrailsMetadataQueryHandler(_repositoryMock.Object);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task Handle_ReturnsMetadata_WhenMetadataExists(
        ClinicalTrialMetadata[] metadataArray)
    {
        // Arrange
        metadataArray[0].TrialId = "trial-123";
        metadataArray[0].Title = "Test Trial";
        metadataArray[0].Status = "Ongoing";
        metadataArray[1].Status = "Not Started";
        metadataArray[2].Status = "Completed";

        var metadata = new[]         {
            metadataArray[0]
        };
        _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<ClinicalTrialMetadata, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(metadata);

        var query = new GetClinicalTrialsMetadataQuery("Test Trial", "Ongoing");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("trial-123", result[0].TrialId);
        Assert.Equal("Test Trial", result[0].Title);
        Assert.Equal("Ongoing", result[0].Status);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyArray_WhenNoMetadataExists()
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<ClinicalTrialMetadata, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<ClinicalTrialMetadata>());

        var query = new GetClinicalTrialsMetadataQuery("Nonexistent Trial", "Completed");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task Handle_ReturnsAllMetadata_WhenNoFiltersAreProvided(
        ClinicalTrialMetadata[] metadataArray)
    {
        // Arrange
        _repositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<ClinicalTrialMetadata, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(metadataArray);

        var query = new GetClinicalTrialsMetadataQuery(null, null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Length);
    }
}
