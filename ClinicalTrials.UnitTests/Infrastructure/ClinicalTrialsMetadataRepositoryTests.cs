using ClinicalTrials.Domain.Entities;
using ClinicalTrials.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ClinicalTrials.UnitTests.Infrastructure;

public class ClinicalTrialsMetadataRepositoryTests : BaseRepositoryTests
{
    private readonly ClinicalTrialsMetadataRepository _repository;

    public ClinicalTrialsMetadataRepositoryTests()
        : base(Guid.NewGuid().ToString())
    {
        _repository = new ClinicalTrialsMetadataRepository(_contextMock);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task CreateAsync_ReturnsTrue_WhenMetadataIsAdded(
        ClinicalTrialMetadata metadata)
    {
        // Arrange
        metadata.TrialId = "trial-123";
        var dbSetMock = new Mock<DbSet<ClinicalTrialMetadata>>();
        _contextMock.ClinicalTrialMetadata = dbSetMock.Object;
        _contextMock.ClinicalTrialMetadata.Add(metadata);

        // Act
        var result = await _repository.CreateAsync(metadata, CancellationToken.None);

        // Assert
        Assert.False(result);
        dbSetMock.Verify(m => m.AddAsync(metadata, It.IsAny<CancellationToken>()), Times.Once);
        //_contextMock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task CreateAsync_ReturnsFalse_WhenSaveChangesFails(
        ClinicalTrialMetadata metadata)
    {
        // Arrange
        metadata.TrialId = "trial-123";
        var dbSetMock = new Mock<DbSet<ClinicalTrialMetadata>>();
        _contextMock.ClinicalTrialMetadata = dbSetMock.Object;

        // Act
        var result = await _repository.CreateAsync(metadata, CancellationToken.None);

        // Assert
        Assert.False(result);
        dbSetMock.Verify(m => m.AddAsync(metadata, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task GetAsync_ReturnsEmptyMetadataArray_WhenMetadataDoNotExists(
        ClinicalTrialMetadata metadata)
    {
        // Arrange
        metadata.TrialId = "trial-1234";
        var metadataArray = new[]
        {
            metadata
        }.AsQueryable();

        var dbSetMock = new Mock<DbSet<ClinicalTrialMetadata>>();
        dbSetMock.As<IQueryable<ClinicalTrialMetadata>>().Setup(m => m.Provider).Returns(metadataArray.Provider);
        dbSetMock.As<IQueryable<ClinicalTrialMetadata>>().Setup(m => m.Expression).Returns(metadataArray.Expression);
        dbSetMock.As<IQueryable<ClinicalTrialMetadata>>().Setup(m => m.ElementType).Returns(metadataArray.ElementType);
        dbSetMock.As<IQueryable<ClinicalTrialMetadata>>().Setup(m => m.GetEnumerator()).Returns(metadataArray.GetEnumerator());

        // Act
        var result = await _repository.GetAsync(m => m.TrialId == "trial-123", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task GetByIdAsync_ReturnsMetadata_WhenMetadataExists(
        ClinicalTrialMetadata metadata)
    {
        // Arrange
        metadata.TrialId = "trial-123";
        var dbSetMock = new Mock<DbSet<ClinicalTrialMetadata>>();
        dbSetMock.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>())).ReturnsAsync(metadata);

        _contextMock.ClinicalTrialMetadata.Add(metadata);

        // Act
        var result = await _repository.GetByIdAsync("trial-123", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("trial-123", result.TrialId);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenMetadataDoesNotExist()
    {
        // Arrange
        var dbSetMock = new Mock<DbSet<ClinicalTrialMetadata>>();
        dbSetMock.Setup(m => m.FindAsync(It.IsAny<object[]>(), It.IsAny<CancellationToken>())).ReturnsAsync((ClinicalTrialMetadata)null);

        // Act
        var result = await _repository.GetByIdAsync("trial-123", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
