namespace HuffmanCoding.UnitTests;

public class HuffmanEncoderTests
{
    [Fact]
    public void Encode_ShouldReturnEncodedByteArray_WhenInputIsValid()
    {
        // Arrange
        var input = "Hello, World!";

        // Act
        var result = HuffmanEncoder.Encode(input);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void Decode_ShouldReturnDecodedString_WhenInputIsValid()
    {
        // Arrange
        var input = HuffmanEncoder.Encode("Hello, World!");

        // Act
        var result = HuffmanEncoder.Decode(input);

        // Assert
        result.Should().Be("Hello, World!");
    }

    [Fact]
    public void CanEncode_ShouldReturnTrue_WhenInputCanBeEncoded()
    {
        // Arrange
        var input = "Hello, World!";

        // Act
        var result = HuffmanEncoder.CanEncode(input);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanEncode_ShouldReturnFalse_WhenInputCannotBeEncoded()
    {
        // Arrange
        var input = "This string contains characters not in the ASCII table. 😊";

        // Act
        var result = HuffmanEncoder.CanEncode(input);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Measure_ShouldReturnCorrectLength_WhenInputIsValid()
    {
        // Arrange
        var input = "Hello, World!";

        // Act
        var result = HuffmanEncoder.Measure(input);

        // Assert
        result.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public void Encode_ShouldThrowException_WhenInputIsNull()
    {
        // Arrange
        var input = (string?)null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HuffmanEncoder.Encode(input!));
    }

    [Fact]
    public void Decode_ShouldThrowException_WhenInputIsNull()
    {
        // Arrange
        var input = (string)null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HuffmanEncoder.Decode(input));
    }
    
    [Fact]
    public void CanEncode_ShouldReturnTrue_WhenInputIsEmpty()
    {
        // Arrange
        var input = "";

        // Act
        var result = HuffmanEncoder.CanEncode(input);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanEncode_ShouldReturnFalse_WhenInputIsNull()
    {
        // Arrange
        var input = (string)null!;

        // Act
        var result = HuffmanEncoder.CanEncode(input!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Measure_ShouldThrowException_WhenInputIsNull()
    {
        // Arrange
        var input = (string)null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => HuffmanEncoder.Measure(input));
    }

    [Fact]
    public void EncodeAndDecode_ShouldReturnOriginalString_WhenInputContainsAllAsciiCharacters()
    {
        // Arrange
        var input = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Act
        var encoded = HuffmanEncoder.Encode(input);
        var decoded = HuffmanEncoder.Decode(encoded);

        // Assert
        decoded.Should().Be(input);
    }
    
    [Fact]
    public void Encode_ShouldReturnExpectedString_WhenInputIsEmpty()
    {
        //FIXME: This test is a placeholder until I formalise the EOS sequence handling
        // Arrange
        var input = "";
        var expected = "D007";

        // Act
        var result = HuffmanEncoder.Encode(input);
        
        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Decode_ReturnEmptyString_WhenInputIsEmpty()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var result = HuffmanEncoder.Decode(input);
        
        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void EncodeAndDecode_ShouldReturnOriginalString_WhenInputIsWhitespace()
    {
        // Arrange
        var input = "   ";

        // Act
        var encoded = HuffmanEncoder.Encode(input);
        var decoded = HuffmanEncoder.Decode(encoded);

        // Assert
        decoded.Should().Be(input);
    }

    [Fact]
    public void Encode_ShouldReturnNonNullAndNonEmpty_WhenInputIsWhitespace()
    {
        // Arrange
        var input = "   ";

        // Act
        var result = HuffmanEncoder.Encode(input);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void Decode_ShouldReturnNonNullAndNonEmpty_WhenInputIsWhitespace()
    {
        // Arrange
        var input = HuffmanEncoder.Encode("   ");

        // Act
        var result = HuffmanEncoder.Decode(input);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }
}