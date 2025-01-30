using ClinicalTrials.Application.Services;

namespace ClinicalTrials.UnitTests.Application;

public class JsonServiceTests
{
    private readonly JsonService _jsonService;

    public JsonServiceTests()
    {
        _jsonService = new JsonService();
    }

    [Fact]
    public void Deserialize_ReturnsObject_WhenJsonIsValid()
    {
        // Arrange
        var json = "{\"Name\":\"Test\"}";

        // Act
        var result = _jsonService.Deserialize<TestObject>(json);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public void Deserialize_ReturnsNull_WhenJsonIsInvalid()
    {
        // Arrange
        var json = "{\"Name\":}";

        // Act
        var result = _jsonService.Deserialize<TestObject>(json);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Serialize_ReturnsJsonString_WhenObjectIsValid()
    {
        // Arrange
        var obj = new TestObject { Name = "Test" };

        // Act
        var result = _jsonService.Serialize(obj);

        // Assert
        Assert.Equal("{\"Name\":\"Test\"}", result);
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenJsonIsValid()
    {
        // Arrange
        var json = "{\"Name\":\"Test\"}";
        var schema = "{\"type\":\"object\",\"properties\":{\"Name\":{\"type\":\"string\"}},\"required\":[\"Name\"]}";

        // Act
        var result = _jsonService.IsValid(json, schema);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenJsonIsInvalid()
    {
        // Arrange
        var json = "{\"Name\":123}";
        var schema = "{\"type\":\"object\",\"properties\":{\"Name\":{\"type\":\"string\"}},\"required\":[\"Name\"]}";

        // Act
        var result = _jsonService.IsValid(json, schema);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenSchemaIsInvalid()
    {
        // Arrange
        var json = "{\"Name\":\"Test\"}";
        var schema = "{\"type\":\"object\",\"properties\":{\"Name\":{\"type\":\"number\"}},\"required\":[\"Name\"]}";

        // Act
        var result = _jsonService.IsValid(json, schema);

        // Assert
        Assert.False(result);
    }

    private class TestObject
    {
        public string Name { get; set; }
    }
}
