using ClinicalTrials.Application.Extensions;
using ClinicalTrials.Application.Metadata.Commands;
using ClinicalTrials.Application.Repositories;
using ClinicalTrials.Domain.Entities;
using Moq;

namespace ClinicalTrials.UnitTests.Application;

public class AddClinicalTrialsMetadataCommandHandlerTests
{
    private readonly Mock<IClinicalTrialsMetadataRepository> _repositoryMock;
    private readonly AddClinicalTrialsMetadataCommandHandler _handler;

    public AddClinicalTrialsMetadataCommandHandlerTests()
    {
        _repositoryMock = new Mock<IClinicalTrialsMetadataRepository>();
        _handler = new AddClinicalTrialsMetadataCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsMetadata_WhenCommandIsValid()
    {
        // Arrange
        var command = new AddClinicalTrialsMetadataCommand
        {
            TrialId = "trial-123",
            Title = "Test Trial",
            Status = Status.Ongoing,
            Participants = 100,
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = null
        };

        _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<ClinicalTrialMetadata>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.TrialId, result.TrialId);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Status.ToEnumMemberAttributeValue(), result.Status);
        Assert.Equal(command.Participants, result.Participants);
        Assert.Equal(command.StartDate, result.StartDate);
        Assert.Equal(command.StartDate.AddMonths(1), result.EndDate);
        Assert.Equal(31, result.Duration);
    }

    [Fact]
    public async Task Handle_ReturnsMetadata_WithEndDate_WhenEndDateIsProvided()
    {
        // Arrange
        var command = new AddClinicalTrialsMetadataCommand
        {
            TrialId = "trial-123",
            Title = "Test Trial",
            Status = Status.Completed,
            Participants = 100,
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = new DateOnly(2023, 2, 1)
        };

        _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<ClinicalTrialMetadata>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.TrialId, result.TrialId);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Status.ToEnumMemberAttributeValue(), result.Status);
        Assert.Equal(command.Participants, result.Participants);
        Assert.Equal(command.StartDate, result.StartDate);
        Assert.Equal(command.EndDate, result.EndDate);
        Assert.Equal(31, result.Duration);
    }

    [Fact]
    public async Task Handle_ReturnsNull_WhenExceptionIsThrown()
    {
        // Arrange
        var command = new AddClinicalTrialsMetadataCommand
        {
            TrialId = "trial-123",
            Title = "Test Trial",
            Status = Status.Ongoing,
            Participants = 100,
            StartDate = new DateOnly(2023, 1, 1),
            EndDate = null
        };

        _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<ClinicalTrialMetadata>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
