using System.Collections;

namespace HuffmanCoding.UnitTests;

public class HuffmanOutputEncoderTests
{
    private readonly Random _rng = new Random(42);
    
    [Theory]
    [InlineData("01010101010101010101010101010101", HuffmanStringEncoding.Bin, "01010101010101010101010101010101")]
    [InlineData("01010101010101010101010101010101", HuffmanStringEncoding.Hex, "AAAAAAAA")]
    public void Encode_ShouldReturnCorrectEncoding(string input, HuffmanStringEncoding encoding, string expected)
    {
        // Arrange
        var bitArray = new BitArray(input.Select(c => c == '1').ToArray());
        var encoder = HuffmanOutputEncoder.GetEncoder(encoding);
        
        // Act
        var result = encoder.Encode(bitArray);
        
        // Assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public void Encode_ShouldReturnCorrectBinFormat()
    {
        // Arrange
        var input = new BitArray(Enumerable.Range(0, 128).Select(_ => _rng.Next(2) == 0).ToArray());
        var encoder = HuffmanOutputEncoder.Bin;
        
        // Act
        var result = encoder.Encode(input);
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().MatchRegex("^[01]+$");
        
        var decoded = encoder.Decode(result);
        decoded.Should().BeEquivalentTo(input);
    }
    
    [Theory]
    [InlineData("00000001", "80")]
    public void Encode_ShouldUseCorrectByteOrder_WhenEncodingHex(string input, string expected)
    {
        // Arrange
        var bitArray = new BitArray(input.Select(c => c == '1').ToArray());
        var encoder = HuffmanOutputEncoder.Hex;
        
        // Act
        var result = encoder.Encode(bitArray);
        
        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void Decode_ShouldReturnCorrectBinFormat()
    {
    }

    [Fact]
    public void Encode_ShouldReturnCorrectHexFormat()
    {
    }

    [Fact]
    public void Decode_ShouldReturnCorrectHexFormat()
    {
    }

    [Fact]
    public void Encode_ShouldReturnCorrectBase64Format()
    {
    }

    [Fact]
    public void Decode_ShouldReturnCorrectBase64Format()
    {
    }
}