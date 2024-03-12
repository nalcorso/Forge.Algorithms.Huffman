namespace Forge.Algorithms.HuffmanCoding;

/// <summary>
/// A builder class for creating a HuffmanEncoder.
/// </summary>
public class HuffmanEncoderBuilder
{
    private HuffmanNode? _root;
    private HuffmanFrequencyTable? _frequencyTable;
    private int _fixedLength;
    private string _eosCharacter = string.Empty;
    private bool _alignToByteSize = false;
    
    /// <summary>
    /// Sets the root node of the Huffman tree.
    /// </summary>
    /// <param name="root">The root node of the Huffman tree.</param>
    /// <returns>The builder instance.</returns>
    public HuffmanEncoderBuilder WithRoot(HuffmanNode root)
    {
        ArgumentNullException.ThrowIfNull(root, nameof(root));
        _root = root;
        return this;
    }
    
    /// <summary>
    /// Sets the frequency table for the Huffman tree.
    /// </summary>
    /// <param name="frequencyTable">The frequency table for the Huffman tree.</param>
    /// <returns>The builder instance.</returns>
    public HuffmanEncoderBuilder WithFrequencyTable(HuffmanFrequencyTable frequencyTable)
    {
        ArgumentNullException.ThrowIfNull(frequencyTable, nameof(frequencyTable));
        _frequencyTable = frequencyTable;
        return this;
    }
    
    /// <summary>
    /// Sets the frequency table for the Huffman tree with a generic ASCII frequency table.
    /// </summary>
    /// <returns>The builder instance.</returns>
    /// <remarks>
    /// This method generates a frequency table where AlphaNumeric > Whitespace > Special Characters > Non-Printable Characters.
    /// </remarks>
    public HuffmanEncoderBuilder WithAsciiFrequencyTable()
    {
        _frequencyTable = new HuffmanFrequencyTable();
        
        foreach (var c in Enumerable.Range(0, 128).Select(x => (char)x))
        {
            if (char.IsLetterOrDigit(c))
                _frequencyTable[c.ToString()] = 1;
            else if (char.IsWhiteSpace(c))
                _frequencyTable[c.ToString()] = 0.5;
            else if (char.IsSymbol(c) || char.IsPunctuation(c))
                _frequencyTable[c.ToString()] = 0.25;
            else
                _frequencyTable[c.ToString()] = 0.1;
        }
        
        return this;
    }
    
    /// <summary>
    /// Sets the fixed length for the Huffman encoding.
    /// </summary>
    /// <param name="fixedLength">The fixed length for the Huffman encoding.</param>
    /// <returns>The builder instance.</returns>
    /// <remarks>A fixed length of 0 or a negative value indicates that the length of the encoded sequence is not fixed.</remarks>
    public HuffmanEncoderBuilder WithFixedLength(int fixedLength)
    {
        _fixedLength = fixedLength;
        return this;
    }

    /// <summary>
    /// Sets the end of sequence character for the Huffman encoding.
    /// </summary>
    /// <param name="eosCharacter">The end of sequence character for the Huffman encoding.</param>
    /// <returns>The builder instance.</returns>
    public HuffmanEncoderBuilder WithEndOfSequenceCharacter(string eosCharacter)
    {
        ArgumentNullException.ThrowIfNull(eosCharacter, nameof(eosCharacter));
        _eosCharacter = eosCharacter;
        return this;
    }
    
    /// <summary>
    /// Sets a value indicating whether the output should be aligned to byte size.
    /// </summary>
    /// <param name="alignToByteSize">A value indicating whether the output should be aligned to byte size.</param>
    /// <returns>The builder instance.</returns>
    public HuffmanEncoderBuilder WithAlignToByteSize(bool alignToByteSize)
    {
        _alignToByteSize = alignToByteSize;
        return this;
    }
    
    /// <summary>
    /// Builds the HuffmanEncoder using the provided parameters.
    /// </summary>
    /// <returns>The built HuffmanEncoder.</returns>
    /// <exception cref="InvalidOperationException">Thrown when neither a frequency table nor a root node is provided, or when the end of sequence character is not in the Huffman tree.</exception>
    /// <remarks>If a frequency table is provided, the Huffman tree is generated from it. If a root node is provided, the frequency table is ignored.</remarks>
    public HuffmanEncoder Build()
    {
        if (_root is null && _frequencyTable is null)
            throw new InvalidOperationException("Either a frequency table or a root node must be provided.");
        
        if (_root is null)
            _root = GenerateHuffmanTree(_frequencyTable!);
        
        if (!string.IsNullOrEmpty(_eosCharacter) && !_root.Sequence.Contains(_eosCharacter))
            throw new InvalidOperationException("The end of sequence character must be in the Huffman tree.");
        
        return new HuffmanEncoder(_root)
        {
            FixedLengthInBits = _fixedLength,
            EndOfSequence = _eosCharacter,
            AlignToByteSize = _alignToByteSize
        };
    }
    
    /// <summary>
    /// Generates a Huffman tree from a given frequency table.
    /// </summary>
    /// <param name="frequencyTable">The frequency table to generate the Huffman tree from.</param>
    /// <returns>The root node of the generated Huffman tree.</returns>
    private static HuffmanNode GenerateHuffmanTree(HuffmanFrequencyTable frequencyTable)
    {
        if (frequencyTable.Count == 0)
            return new HuffmanNode();
        
        var nodes = frequencyTable.Select(x => new HuffmanNode
        {
            Sequence = x.Key,
            Frequency = (int)x.Value
        }).ToList();
        
        while (nodes.Count > 1)
        {
            nodes = nodes.OrderBy(x => x.Frequency).ToList();
            var left = nodes[0];
            var right = nodes[1];
            var parent = new HuffmanNode
            {
                Sequence = left.Sequence + right.Sequence,
                Frequency = left.Frequency + right.Frequency,
                Left = left,
                Right = right
            };
            nodes.Remove(left);
            nodes.Remove(right);
            nodes.Add(parent);
        }
        
        return nodes.Single();
    }
    
}