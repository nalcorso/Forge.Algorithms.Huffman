using System.Text;

namespace ReForge.Huffman.Tests;

public class HuffmanFrequencyTableTests
{
    [Fact]
    public void Create_ShouldReturnNewInstanceOfHuffmanFrequencyTableBuilder()
    {
        // Arrange
        // No arrangement needed as we are testing a static method

        // Act
        var result = HuffmanFrequencyTable.Create();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<HuffmanFrequencyTableBuilder>();
    }

    [Fact]
    public void FromJson_WithValidJson_ShouldReturnHuffmanFrequencyTable()
    {
        // Arrange
        var validJson = "{\"A\": 0.5, \"B\": 0.5}";

        // Act
        var result = HuffmanFrequencyTable.FromJson(validJson);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<HuffmanFrequencyTable>();
        result.Should().ContainKey("A").WhoseValue.Should().Be(0.5);
        result.Should().ContainKey("B").WhoseValue.Should().Be(0.5);
    }

    [Fact]
    public void FromJson_WithInvalidJson_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidJson = "Invalid JSON";

        // Act
        Action act = () => HuffmanFrequencyTable.FromJson(invalidJson);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid JSON format.");
    }

    [Fact]
    public void ToJson_ShouldReturnByteArray()
    {
        // Arrange
        var frequencyTable = new HuffmanFrequencyTable { { "A", 0.5 }, { "B", 0.5 } };

        // Act
        var result = frequencyTable.ToJson();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<byte[]>();
    }
    
    [Fact]
    public void FromJson_WithNullJson_ShouldThrowArgumentException()
    {
        // Arrange
        string? nullJson = null;

        // Act
        Action act = () => HuffmanFrequencyTable.FromJson(nullJson!);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The provided JSON string is null.");
    }

    [Fact]
    public void FromJson_WithNotValidJsonString_ShouldThrowArgumentException()
    {
        // Arrange
        string notValidJson = "{Not a valid JSON string}";

        // Act
        Action act = () => HuffmanFrequencyTable.FromJson(notValidJson);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid JSON format.");
    }

    [Fact]
    public void FromJson_WithUnsupportedConversion_ShouldThrowArgumentException()
    {
        // Arrange
        string unsupportedConversionJson = "{\"A\": \"not a double\"}";

        // Act
        Action act = () => HuffmanFrequencyTable.FromJson(unsupportedConversionJson);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid JSON format.");
    }

    [Fact]
    public void FromJson_WithEmptyJson_ShouldReturnEmptyHuffmanFrequencyTable()
    {
        // Arrange
        string emptyJson = "{}";

        // Act
        var result = HuffmanFrequencyTable.FromJson(emptyJson);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<HuffmanFrequencyTable>();
        result.Count.Should().Be(0);
    }

    [Fact]
    public void ToJson_WithEmptyHuffmanFrequencyTable_ShouldReturnEmptyByteArray()
    {
        // Arrange
        var emptyFrequencyTable = new HuffmanFrequencyTable();

        // Act
        var result = emptyFrequencyTable.ToJson();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<byte[]>();
        result.Length.Should().Be(2);
        var jsonString = Encoding.UTF8.GetString(result);
        jsonString.Should().Be("{}");
    }
}