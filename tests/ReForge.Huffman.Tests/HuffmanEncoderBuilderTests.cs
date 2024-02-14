namespace ReForge.Huffman.Tests;

public class HuffmanEncoderBuilderTests
{
    [Fact]
    public void WithRoot_ShouldSetRoot()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var rootNode = new HuffmanNode();

        // Act
        var result = builder.WithRoot(rootNode);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void WithFrequencyTable_ShouldSetFrequencyTable()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var frequencyTable = new HuffmanFrequencyTable();

        // Act
        var result = builder.WithFrequencyTable(frequencyTable);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void WithFixedLength_ShouldSetFixedLength()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var fixedLength = 5;

        // Act
        var result = builder.WithFixedLength(fixedLength);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void WithEndOfSequenceCharacter_ShouldSetEndOfSequenceCharacter()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var eosCharacter = "E";

        // Act
        var result = builder.WithEndOfSequenceCharacter(eosCharacter);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Build_WithoutRootAndFrequencyTable_ShouldThrowException()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();

        // Act
        Action act = () => builder.Build();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Either a frequency table or a root node must be provided.");
    }

    [Fact]
    public void Build_WithRootNotContainingEndOfSequenceCharacter_ShouldThrowException()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var rootNode = new HuffmanNode();
        builder.WithRoot(rootNode).WithEndOfSequenceCharacter("E");

        // Act
        Action act = () => builder.Build();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("The end of sequence character must be in the Huffman tree.");
    }

    [Fact]
    public void Build_WithValidRootAndEndOfSequenceCharacter_ShouldReturnHuffmanEncoder()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var rootNode = new HuffmanNode { Sequence = "E" };
        builder.WithRoot(rootNode).WithEndOfSequenceCharacter("E");

        // Act
        var result = builder.Build();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<HuffmanEncoder>();
    }
    
    [Fact]
    public void WithRoot_Null_ShouldThrowArgumentNullException()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();

        // Act
        Action act = () => builder.WithRoot(null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WithFrequencyTable_Null_ShouldThrowArgumentNullException()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();

        // Act
        Action act = () => builder.WithFrequencyTable(null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WithFixedLength_NegativeOrZero_ShouldNotThrowException()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();

        // Act
        Action actZero = () => builder.WithFixedLength(0);
        Action actNegative = () => builder.WithFixedLength(-1);

        // Assert
        actZero.Should().NotThrow();
        actNegative.Should().NotThrow();
    }

    [Fact]
    public void WithEndOfSequenceCharacter_NullOrEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();

        // Act
        Action act = () => builder.WithEndOfSequenceCharacter(null);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Build_WithFrequencyTableOnly_ShouldReturnHuffmanEncoder()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var frequencyTable = new HuffmanFrequencyTable();

        // Act
        var result = builder.WithFrequencyTable(frequencyTable).Build();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<HuffmanEncoder>();
    }

    [Fact]
    public void Build_WithRootAndFrequencyTable_ShouldReturnHuffmanEncoder()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var rootNode = new HuffmanNode();
        var frequencyTable = new HuffmanFrequencyTable();

        // Act
        var result = builder.WithRoot(rootNode).WithFrequencyTable(frequencyTable).Build();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<HuffmanEncoder>();
    }
    
    [Fact]
    public void WithAsciiFrequencyTable_ShouldSetAsciiFrequencyTable()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();

        // Act
        var result = builder.WithAsciiFrequencyTable();

        // Assert
        result.Should().NotBeNull();
    }
    
    [Fact]
    public void WithAsciiFrequencyTable_ShouldEncodeAsciiCharacters()
    {
        // Arrange
        var builder = new HuffmanEncoderBuilder();
        var asciiCharacters = new string(Enumerable.Range(0, 128).Select(x => (char)x).ToArray());

        // Act
        var encoder = builder.WithAsciiFrequencyTable().Build();

        // Assert
        foreach (var character in asciiCharacters)
        {
            encoder.CanEncode(character.ToString()).Should().BeTrue();
        }
    }
}
