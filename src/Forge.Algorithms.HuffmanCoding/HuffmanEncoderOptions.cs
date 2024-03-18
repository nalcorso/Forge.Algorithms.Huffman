using System.Collections;

namespace Forge.Algorithms.HuffmanCoding;

public enum HuffmanStringEncoding
{
    Auto,
    Bin,
    Hex,
    Base64
}

public class HuffmanEncoderOptions
{
    /// <summary>
    /// Output the encoded string aligned to byte size.
    /// </summary>
    public int FixedLengthOutputInBytes { get; init; } = 0;
    
    /// <summary>
    /// Use the specified character as the end of sequence marker in the encoded string. The default is "\0".
    /// </summary>
    /// <remarks>This value is ignored if the FixedSizeOutput property is set to 0.</remarks>
    public string EndOfSequence { get; init; } = "\0";

    /// <summary>
    /// Use a custom frequency table for the Huffman encoding.
    /// </summary>
    /// <remarks>If this property is not set then the default frequency table is used.</remarks>
    public HuffmanFrequencyTable? FrequencyTable { get; init; } = HuffmanFrequencyTable.ASCII;
    
    /// <summary>
    /// The format of the input string. The default is Auto.
    /// </summary>
    public HuffmanStringEncoding InputEncoding { get; init; } = HuffmanStringEncoding.Auto;
    
    public HuffmanStringEncoding OutputEncoding { get; init; } = HuffmanStringEncoding.Hex;

    public bool IsByteAlignmentRequired => FixedLengthOutputInBytes > 0 || OutputEncoding != HuffmanStringEncoding.Bin;
    
    
    private HuffmanNode? _lazyRoot;
    
    internal HuffmanNode _root => LazyInitializer.EnsureInitialized(ref _lazyRoot, () => FrequencyTable!.BuildTree());
    
    private Dictionary<string, BitArray>? _lazyHuffmanCode;
    
    internal Dictionary<string, BitArray> _huffmanCode => LazyInitializer.EnsureInitialized(ref _lazyHuffmanCode, () => FrequencyTable!.GenerateHuffmanCode(_root));
}