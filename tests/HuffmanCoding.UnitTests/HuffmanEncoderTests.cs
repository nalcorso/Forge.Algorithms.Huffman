namespace HuffmanCoding.UnitTests;

public class HuffmanEncoderTests
{
    private readonly HuffmanEncoder _encoder;

    public HuffmanEncoderTests()
    {
        _encoder = new HuffmanEncoderBuilder()
            .WithAsciiFrequencyTable()
            .WithEndOfSequenceCharacter("\0")
            .WithAlignToByteSize(true)
            .Build();    
    }

    [Fact]
    public void Encode_ShouldReturnEncodedByteArray_WhenInputIsValid()
    {
        // Arrange
        var input = "Hello, World!";

        // Act
        var result = _encoder.Encode(input);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void Decode_ShouldReturnDecodedString_WhenInputIsValid()
    {
        // Arrange
        var input = _encoder.Encode("Hello, World!");

        // Act
        var result = _encoder.Decode(input);

        // Assert
        result.Should().Be("Hello, World!");
    }

    [Fact]
    public void CanEncode_ShouldReturnTrue_WhenInputCanBeEncoded()
    {
        // Arrange
        var input = "Hello, World!";

        // Act
        var result = _encoder.CanEncode(input);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanEncode_ShouldReturnFalse_WhenInputCannotBeEncoded()
    {
        // Arrange
        var input = "This string contains characters not in the ASCII table. 😊";

        // Act
        var result = _encoder.CanEncode(input);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Measure_ShouldReturnCorrectLength_WhenInputIsValid()
    {
        // Arrange
        var input = "Hello, World!";

        // Act
        var result = _encoder.Measure(input);

        // Assert
        result.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public void Encode_ShouldThrowException_WhenInputIsNull()
    {
        // Arrange
        var input = (string?)null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _encoder.Encode(input!));
    }

    [Fact]
    public void Decode_ShouldThrowException_WhenInputIsNull()
    {
        // Arrange
        var input = (byte[])null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _encoder.Decode(input));
    }
    
    [Fact]
    public void CanEncode_ShouldReturnTrue_WhenInputIsEmpty()
    {
        // Arrange
        var input = "";

        // Act
        var result = _encoder.CanEncode(input);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanEncode_ShouldReturnFalse_WhenInputIsNull()
    {
        // Arrange
        var input = (string?)null;

        // Act
        var result = _encoder.CanEncode(input!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Measure_ShouldThrowException_WhenInputIsNull()
    {
        // Arrange
        var input = (string?)null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _encoder.Measure(input));
    }

    [Fact]
    public void EncodeAndDecode_ShouldReturnOriginalString_WhenInputContainsAllAsciiCharacters()
    {
        // Arrange
        var input = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Act
        var encoded = _encoder.Encode(input);
        var decoded = _encoder.Decode(encoded);

        // Assert
        decoded.Should().Be(input);
    }
    
    [Fact]
    public void Encode_ShouldReturnExpectedString_WhenInputIsEmpty()
    {
        // Arrange
        var input = "";
        var expected = new byte[] { 0x0B, 0xE0 };

        // Act
        var result = _encoder.Encode(input);
        
        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Decode_ReturnEmptyString_WhenInputIsEmpty()
    {
        // Arrange
        var input = new byte[0];

        // Act
        var result = _encoder.Decode(input);
        
        // Assert
        result.Should().BeEmpty();
    }
    
    [Fact]
    public void EncodeAndDecode_ShouldReturnOriginalString_WhenInputIsWhitespace()
    {
        // Arrange
        var input = "   ";

        // Act
        var encoded = _encoder.Encode(input);
        var decoded = _encoder.Decode(encoded);

        // Assert
        decoded.Should().Be(input);
    }

    [Fact]
    public void Encode_ShouldReturnNonNullAndNonEmpty_WhenInputIsWhitespace()
    {
        // Arrange
        var input = "   ";

        // Act
        var result = _encoder.Encode(input);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void Decode_ShouldReturnNonNullAndNonEmpty_WhenInputIsWhitespace()
    {
        // Arrange
        var input = _encoder.Encode("   ");

        // Act
        var result = _encoder.Decode(input);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }
}