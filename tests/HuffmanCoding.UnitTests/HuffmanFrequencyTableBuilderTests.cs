using Forge.Algorithms.Huffman;

namespace HuffmanCoding.UnitTests;

public class HuffmanFrequencyTableBuilderTests
{
    [Fact]
    public void WithMaxNGramLength_ShouldSetMaxNGramLength()
    {
        // Arrange
        var builder = new HuffmanFrequencyTableBuilder();

        // Act
        var result = builder.WithMaxNGramLength(3);

        // Assert
        result.Should().BeOfType<HuffmanFrequencyTableBuilder>();
    }

    [Fact]
    public void WithSequences_ShouldSetSequences()
    {
        // Arrange
        var builder = new HuffmanFrequencyTableBuilder();
        var sequences = new List<string> { "abc", "def" };

        // Act
        var result = builder.WithSequences(sequences);

        // Assert
        result.Should().BeOfType<HuffmanFrequencyTableBuilder>();
    }

    [Fact]
    public void WithLengthWeight_ShouldSetLengthWeight()
    {
        // Arrange
        var builder = new HuffmanFrequencyTableBuilder();

        // Act
        var result = builder.WithLengthWeight(0.5);

        // Assert
        result.Should().BeOfType<HuffmanFrequencyTableBuilder>();
    }

    [Fact]
    public void WithEndOfSequenceCharacter_ShouldSetEndOfSequenceCharacter()
    {
        // Arrange
        var builder = new HuffmanFrequencyTableBuilder();

        // Act
        var result = builder.WithEndOfSequenceCharacter("e");

        // Assert
        result.Should().BeOfType<HuffmanFrequencyTableBuilder>();
    }

    [Fact]
    public void Build_ShouldReturnHuffmanFrequencyTable()
    {
        // Arrange
        var builder = new HuffmanFrequencyTableBuilder()
            .WithMaxNGramLength(3)
            .WithSequences(new List<string> { "abc", "def" });

        // Act
        var result = builder.Build();

        // Assert
        result.Should().BeOfType<HuffmanFrequencyTable>();
    }
    
    [Fact]
    public void Build_ShouldCalculateCorrectFrequencies()
    {
        // Arrange
        var builder = new HuffmanFrequencyTableBuilder()
            .WithMaxNGramLength(3)
            .WithSequences(new List<string> { "abc", "abc", "def" });

        // Act
        var result = builder.Build();

        // Assert
        result["abc"].Should().Be(2);
        result["def"].Should().Be(1);
    }
}