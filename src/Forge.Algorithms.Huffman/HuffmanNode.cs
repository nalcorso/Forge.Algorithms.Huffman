namespace Forge.Algorithms.Huffman;

/// <summary>
/// Represents a node in a Huffman tree.
/// </summary>
internal class HuffmanNode
{
    /// <summary>
    /// Gets or sets the sequence of characters represented by this node.
    /// </summary>
    public string Sequence { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the frequency of occurrence of the sequence in the input data.
    /// </summary>
    public int Frequency { get; set; }

    /// <summary>
    /// Gets or sets the left child of this node in the Huffman tree.
    /// </summary>
    public HuffmanNode? Left { get; set; }

    /// <summary>
    /// Gets or sets the right child of this node in the Huffman tree.
    /// </summary>
    public HuffmanNode? Right { get; set; }

    /// <summary>
    /// Gets a value indicating whether this node is a leaf node (i.e., has no children).
    /// </summary>
    public bool IsLeaf => Left is null && Right is null;
}