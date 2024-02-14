namespace ReForge.Huffman.Tests;

public class HuffmanNodeTests
{
    [Fact]
    public void Sequence_ShouldBeEmpty_WhenNodeIsCreated()
    {
        // Arrange
        var node = new HuffmanNode();

        // Act
        var sequence = node.Sequence;

        // Assert
        sequence.Should().BeEmpty();
    }

    [Fact]
    public void Frequency_ShouldBeZero_WhenNodeIsCreated()
    {
        // Arrange
        var node = new HuffmanNode();

        // Act
        var frequency = node.Frequency;

        // Assert
        frequency.Should().Be(0);
    }

    [Fact]
    public void IsLeaf_ShouldBeTrue_WhenNodeHasNoChildren()
    {
        // Arrange
        var node = new HuffmanNode();

        // Act
        var isLeaf = node.IsLeaf;

        // Assert
        isLeaf.Should().BeTrue();
    }

    [Fact]
    public void IsLeaf_ShouldBeFalse_WhenNodeHasChildren()
    {
        // Arrange
        var node = new HuffmanNode
        {
            Left = new HuffmanNode(),
            Right = new HuffmanNode()
        };

        // Act
        var isLeaf = node.IsLeaf;

        // Assert
        isLeaf.Should().BeFalse();
    }
    
    [Fact]
    public void Sequence_ShouldBeSetCorrectly()
    {
        // Arrange
        var node = new HuffmanNode();
        var expectedSequence = "abc";

        // Act
        node.Sequence = expectedSequence;

        // Assert
        node.Sequence.Should().Be(expectedSequence);
    }

    [Fact]
    public void Frequency_ShouldBeSetCorrectly()
    {
        // Arrange
        var node = new HuffmanNode();
        var expectedFrequency = 5;

        // Act
        node.Frequency = expectedFrequency;

        // Assert
        node.Frequency.Should().Be(expectedFrequency);
    }

    [Fact]
    public void LeftChild_ShouldBeSetCorrectly()
    {
        // Arrange
        var node = new HuffmanNode();
        var expectedChild = new HuffmanNode();

        // Act
        node.Left = expectedChild;

        // Assert
        node.Left.Should().Be(expectedChild);
    }

    [Fact]
    public void RightChild_ShouldBeSetCorrectly()
    {
        // Arrange
        var node = new HuffmanNode();
        var expectedChild = new HuffmanNode();

        // Act
        node.Right = expectedChild;

        // Assert
        node.Right.Should().Be(expectedChild);
    }
}