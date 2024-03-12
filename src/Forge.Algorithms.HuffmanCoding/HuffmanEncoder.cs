using System.Collections;
using System.Text;

namespace Forge.Algorithms.HuffmanCoding;

/// <summary>
/// The HuffmanEncoder class is used for encoding and decoding strings using Huffman coding.
/// </summary>
public class HuffmanEncoder
{
    private readonly HuffmanNode _root;
    
    /// <summary>
    /// Gets or initializes the fixed length in bits for the encoded string.
    /// </summary>
    public int FixedLengthInBits { get; init; }
    
    /// <summary>
    /// Gets or initializes the end of sequence character for the encoded string.
    /// </summary>
    public string EndOfSequence { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or initializes a value indicating whether the output should be aligned to byte size.
    /// </summary>
    public bool AlignToByteSize { get; init; } = false;
    
    /// <summary>
    /// Initializes a new instance of the HuffmanEncoder class.
    /// </summary>
    /// <param name="root">The root node of the Huffman tree.</param>
    public HuffmanEncoder(HuffmanNode root)
    {
        _root = root;
    }
    
    /// <summary>
    /// Creates a new HuffmanEncoderBuilder.
    /// </summary>
    /// <returns>A new HuffmanEncoderBuilder.</returns>
    public static HuffmanEncoderBuilder Create()
    {
        return new HuffmanEncoderBuilder();
    }
    
    /// <summary>
    /// Encodes a string into a byte array using Huffman coding.
    /// </summary>
    /// <param name="input">The string to encode.</param>
    /// <returns>The encoded string as a byte array.</returns>
    public byte[] Encode(string input)
    {
        return BinToHex(EncodeBitArray(input));
    }
    
    /// <summary>
    /// Decodes a byte array into a string using Huffman coding.
    /// </summary>
    /// <param name="input">The byte array to decode.</param>
    /// <returns>The decoded string.</returns>
    public string Decode(byte[] input)
    {
        return DecodeBitArray(HexToBin(input));
    }
    
    /// <summary>
    /// Checks if the input string can be encoded using the current Huffman tree.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <returns>True if the string can be encoded, false otherwise.</returns>
    public bool CanEncode(string input)
    {
        if (input is null)
        {
            return false;
        }
        
        var huffmanCode = GenerateHuffmanCode(_root);
        foreach (var character in input)
        {
            if (!huffmanCode.ContainsKey(character.ToString()))
            {
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Measures the length of the encoded string in bits.
    /// </summary>
    /// <param name="input">The string to measure.</param>
    /// <returns>The length of the encoded string in bits.</returns>
    /// <remarks>Ignores the FixedLengthInBits property when measuring the length of the encoded string.</remarks>
    public int Measure(string input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        
        var huffmanCode = GenerateHuffmanCode(_root);
        var result = 0;
        foreach (var character in input)
        {
            result += huffmanCode[character.ToString()].Length;
        }
        
        if (!string.IsNullOrEmpty(EndOfSequence))
        {
            result += huffmanCode[EndOfSequence].Length;
        }
        
        return result;
    }
    
    /// <summary>
    /// Encodes a string into a BitArray using Huffman coding.
    /// </summary>
    /// <param name="input">The string to encode.</param>
    /// <returns>The encoded string as a BitArray.</returns>
    private BitArray EncodeBitArray(string input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        
        if ((AlignToByteSize || FixedLengthInBits > 0) && string.IsNullOrEmpty(EndOfSequence))
        {
            throw new InvalidOperationException("The EndOfSequence property must be set when the AlignToByteSize or FixedLengthInBits property is set.");
        }
        
        if (AlignToByteSize && FixedLengthInBits % 8 != 0)
        {
            throw new InvalidOperationException("The AlignToByteSize property cannot be true when the FixedLengthInBits property is not a multiple of 8.");
        }
        
        var huffmanCode = GenerateHuffmanCode(_root);
        
        if (!string.IsNullOrEmpty(EndOfSequence) && !huffmanCode.ContainsKey(EndOfSequence))
        {
            throw new InvalidOperationException("The EndOfSequence property must be in the Huffman tree.");
        }
        
        var result = new List<bool>();

        foreach (var character in input)
        {
            var bitArray = huffmanCode[character.ToString()];
            foreach (bool bit in bitArray)
            {
                result.Add(bit);
            }
        }
        
        if (!string.IsNullOrEmpty(EndOfSequence))
        {
            var eosArray = huffmanCode[EndOfSequence];
            foreach (bool bit in eosArray)
            {
                result.Add(bit);
            }
        }
        
        if (AlignToByteSize)
        {
            while (result.Count % 8 != 0)
            {
                result.Add(false);
            }
        }
        
        if (FixedLengthInBits > 0)
        {
            while (result.Count < FixedLengthInBits)
            {
                result.Add(false);
            }
        }

        return new BitArray(result.ToArray());
    }
    
    /// <summary>
    /// Decodes a BitArray into a string using Huffman coding.
    /// </summary>
    /// <param name="input">The BitArray to decode.</param>
    /// <returns>The decoded string.</returns>
    private string DecodeBitArray(BitArray input)
    {
        var result = new StringBuilder();
        var currentNode = _root;

        foreach (bool bit in input)
        {
            currentNode = bit ? currentNode.Right : currentNode.Left;

            if (currentNode.IsLeaf)
            {
                if (!string.IsNullOrEmpty(EndOfSequence) && currentNode.Sequence == EndOfSequence)
                    break;
                
                result.Append(currentNode.Sequence);
                currentNode = _root;
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Converts a BitArray into a byte array.
    /// </summary>
    /// <param name="input">The BitArray to convert.</param>
    /// <returns>The converted byte array.</returns>
    private static byte[] BinToHex(BitArray input)
    {
        if (input.Length % 8 != 0)
            throw new ArgumentException("The length of the BitArray must be a multiple of 8.", nameof(input));
        
        byte[] byteArray = new byte[input.Length / 8];
        for (int i = 0; i < input.Length; i += 8)
        {
            byte byteValue = 0;
            for (int j = 0; j < 8; j++)
            {
                byteValue += (input[i + j] ? (byte)(1 << (7 - j)) : (byte)0);
            }
            byteArray[i / 8] = byteValue;
        }

        return byteArray;
    }

    /// <summary>
    /// Converts a byte array into a BitArray.
    /// </summary>
    /// <param name="input">The byte array to convert.</param>
    /// <returns>The converted BitArray.</returns>
    private static BitArray HexToBin(byte[] input)
    {
        string hex = BitConverter.ToString(input).Replace("-", "");
        var binary = new StringBuilder();
        foreach (var hexDigit in hex)
        {
            binary.Append(Convert.ToString(Convert.ToInt32(hexDigit.ToString(), 16), 2).PadLeft(4, '0'));
        }

        var bitArray = new BitArray(binary.Length);
        for (int i = 0; i < binary.Length; i++)
        {
            bitArray[i] = binary[i] == '1';
        }

        return bitArray;
    }
    
    /// <summary>
    /// Generates a Huffman code for a given HuffmanNode.
    /// </summary>
    /// <param name="node">The HuffmanNode to generate the code for.</param>
    /// <param name="code">The current BitArray code.</param>
    /// <returns>A dictionary mapping sequences to their corresponding BitArray codes.</returns>
    private Dictionary<string, BitArray> GenerateHuffmanCode(HuffmanNode? node, BitArray? code = null)
    {
        var huffmanCode = new Dictionary<string, BitArray>();

        if (node == null)
        {
            return huffmanCode;
        }

        if (code == null)
        {
            code = new BitArray(0);
        }

        if (node.IsLeaf)
        {
            huffmanCode[node.Sequence] = code;
        }
        else
        {
            if (node.Left != null)
            {
                var leftCode = new BitArray(code.Length + 1);
                for (int i = 0; i < code.Length; i++)
                {
                    leftCode[i] = code[i];
                }
                leftCode[leftCode.Length - 1] = false;
                foreach (var pair in GenerateHuffmanCode(node.Left, leftCode))
                {
                    huffmanCode[pair.Key] = pair.Value;
                }
            }

            if (node.Right != null)
            {
                var rightCode = new BitArray(code.Length + 1);
                for (int i = 0; i < code.Length; i++)
                {
                    rightCode[i] = code[i];
                }
                rightCode[rightCode.Length - 1] = true;
                foreach (var pair in GenerateHuffmanCode(node.Right, rightCode))
                {
                    huffmanCode[pair.Key] = pair.Value;
                }
            }
        }

        return huffmanCode;
    }
}