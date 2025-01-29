using ClinicalTrials.Api.Controllers;
using ClinicalTrials.Api.Validators;
using ClinicalTrials.Application.Interfaces;
using ClinicalTrials.Application.Metadata.Commands;
using ClinicalTrials.Application.Metadata.Queries;
using ClinicalTrials.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace ClinicalTrials.UnitTests.Api;

public class ClinicalTrialsMetadataControllerTests
{
    private readonly Mock<ILogger<ClinicalTrialsMetadataController>> _loggerMock;
    private readonly Mock<IJsonService> _jsonServiceMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ClinicalTrialsMetadataController _controller;

    public ClinicalTrialsMetadataControllerTests()
    {
        _loggerMock = new Mock<ILogger<ClinicalTrialsMetadataController>>();
        _jsonServiceMock = new Mock<IJsonService>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new ClinicalTrialsMetadataController(_loggerMock.Object, _jsonServiceMock.Object, _mediatorMock.Object);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task Get_ReturnsOk_WhenMetadataExists(
        ClinicalTrialMetadata[] metadataArray)
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetClinicalTrialsMetadataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(metadataArray);

        // Act
        var result = await _controller.Get(It.IsAny<GetClinicalTrialsMetadataQuery>(), CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<ClinicalTrialMetadata[]>>(result.Result);
        Assert.Equal(metadataArray, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNoMetadataExists()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetClinicalTrialsMetadataQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClinicalTrialMetadata[])null);

        // Act
        var result = await _controller.Get(It.IsAny<GetClinicalTrialsMetadataQuery>(), CancellationToken.None);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task GetById_ReturnsOk_WhenMetadataExists(
        ClinicalTrialMetadata metadata)
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetClinicalTrialsMetadataByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(metadata);

        // Act
        var result = await _controller.GetById("test-id", CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<ClinicalTrialMetadata>>(result.Result);
        Assert.Equal(metadata, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenMetadataDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetClinicalTrialsMetadataByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClinicalTrialMetadata)null);

        // Act
        var result = await _controller.GetById("test-id", CancellationToken.None);

        // Assert
        Assert.IsType<NotFound<string>>(result.Result);
    }

    [Theory]
    [AutoMoqTestData]
    public async Task Upload_ReturnsOk_WhenMetadataIsAdded(
        ClinicalTrialMetadata metadata)
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "file content";
        var fileName = "test.json";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(stream.Length);

        var uploadRequest = new UploadRequest(fileMock.Object);

        _jsonServiceMock.Setup(js => js.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _jsonServiceMock.Setup(js => js.Deserialize<AddClinicalTrialsMetadataCommand>(It.IsAny<string>()))
            .Returns(new AddClinicalTrialsMetadataCommand());

        _mediatorMock.Setup(m => m.Send(It.IsAny<AddClinicalTrialsMetadataCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(metadata);

        // Act
        var result = await _controller.Upload(uploadRequest, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<Ok<ClinicalTrialMetadata>>(result.Result);
    }

    [Fact]
    public async Task Upload_ReturnsBadRequest_WhenJsonIsInvalid()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "file content";
        var fileName = "test.json";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(stream.Length);

        var uploadRequest = new UploadRequest(fileMock.Object);

        _jsonServiceMock.Setup(js => js.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        var result = await _controller.Upload(uploadRequest, CancellationToken.None);

        // Assert
        Assert.IsType<BadRequest<string>>(result.Result);
    }

    [Fact]
    public async Task Upload_ReturnsConflict_WhenMetadataCannotBeAdded()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var content = "file content";
        var fileName = "test.json";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        fileMock.Setup(_ => _.OpenReadStream()).Returns(stream);
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(stream.Length);

        var uploadRequest = new UploadRequest(fileMock.Object);

        _jsonServiceMock.Setup(js => js.IsValid(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _jsonServiceMock.Setup(js => js.Deserialize<AddClinicalTrialsMetadataCommand>(It.IsAny<string>()))
            .Returns(new AddClinicalTrialsMetadataCommand());

        _mediatorMock.Setup(m => m.Send(It.IsAny<AddClinicalTrialsMetadataCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClinicalTrialMetadata)null);

        // Act
        var result = await _controller.Upload(uploadRequest, CancellationToken.None);

        // Assert
        Assert.IsType<Conflict<string>>(result.Result);
    }
}
