namespace HuffmanCoding.UnitTests;

public class HuffmanEncodingHelperTests
{
    [Theory]
    [InlineData("01010101", HuffmanEncodingHelper.EncodingType.Bin)]
    [InlineData("9ABCDEF", HuffmanEncodingHelper.EncodingType.Hex)]
    [InlineData("SGVsbG8gd29ybGQ=", HuffmanEncodingHelper.EncodingType.Base64)]
    [InlineData("Hello World", HuffmanEncodingHelper.EncodingType.Unknown)]
    public void DetectEncoding_ShouldReturnCorrectEncodingType(string input, HuffmanEncodingHelper.EncodingType expected)
    {
        // Arrange
        // No arrangement needed as we are directly passing the inputs to the method

        // Act
        var result = HuffmanEncodingHelper.DetectEncoding(input);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void DetectEncoding_ShouldReturnBin_WhenInputIsAmbiguous()
    {
        // Arrange
        var input = "01010101"; // This could be interpreted as Bin or Base64

        // Act
        var result = HuffmanEncodingHelper.DetectEncoding(input);

        // Assert
        result.Should().Be(HuffmanEncodingHelper.EncodingType.Bin);
    }
    
    [Fact]
    public void DetectEncoding_ShouldReturnUnknown_WhenInputIsNull()
    {
        // Arrange
        string input = null;

        // Act
        var result = HuffmanEncodingHelper.DetectEncoding(input);

        // Assert
        result.Should().Be(HuffmanEncodingHelper.EncodingType.Unknown);
    }
    
    [Fact]
    public void DetectEncoding_ShouldReturnUnknown_WhenInputIsEmpty()
    {
        // Arrange
        var input = "";

        // Act
        var result = HuffmanEncodingHelper.DetectEncoding(input);

        // Assert
        result.Should().Be(HuffmanEncodingHelper.EncodingType.Unknown);
    }

    [Fact]
    public void DetectEncoding_ShouldReturnUnknown_WhenInputIsOnlyPaddingCharacters()
    {
        // Arrange
        var input = "===";

        // Act
        var result = HuffmanEncodingHelper.DetectEncoding(input);

        // Assert
        result.Should().Be(HuffmanEncodingHelper.EncodingType.Unknown);
    }

    [Fact]
    public void DetectEncoding_ShouldReturnUnknown_WhenInputContainsInvalidCharacters()
    {
        // Arrange
        var input = "Hello World!";

        // Act
        var result = HuffmanEncodingHelper.DetectEncoding(input);

        // Assert
        result.Should().Be(HuffmanEncodingHelper.EncodingType.Unknown);
    }
}