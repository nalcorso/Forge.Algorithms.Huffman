using System.Collections;
using System.Text;

namespace Forge.Algorithms.Huffman;

/// <summary>
/// The HuffmanEncoder class is used for encoding and decoding strings using Huffman coding.
/// </summary>
public static class HuffmanEncoder
{
    /// <summary>
    /// Encodes a string into a byte array using Huffman coding.
    /// </summary>
    /// <param name="input">The string to encode.</param>
    /// <param name="options"></param>
    /// <returns>The encoded string as a byte array.</returns>
    public static string Encode(string input, HuffmanEncoderOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        options ??= new HuffmanEncoderOptions();
        
        var encodedBits = EncodeBitArray(input, options);
        return options.OutputEncoding switch
        {
            HuffmanStringEncoding.Auto => HuffmanOutputEncoder.Auto.Encode(encodedBits),
            HuffmanStringEncoding.Bin => HuffmanOutputEncoder.Bin.Encode(encodedBits),
            HuffmanStringEncoding.Hex => HuffmanOutputEncoder.Hex.Encode(encodedBits),
            HuffmanStringEncoding.Base64 => HuffmanOutputEncoder.Base64.Encode(encodedBits),
            _ => throw new ArgumentOutOfRangeException(nameof(options))
        };
    }
    
    /// <summary>
    /// Encodes a string into a typed representation using Huffman coding.
    /// </summary>
    /// <param name="input">The string to encode.</param>
    /// <param name="options">The options to use for encoding.</param>
    /// <typeparam name="T">The type of the output encoding.</typeparam>
    /// <returns>The encoded string as the specified type.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static T EncodeAs<T>(string input, HuffmanEncoderOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        
        var templateImpliedEncoding = typeof(T) switch
        {
            Type t when t == typeof(BitArray) => HuffmanStringEncoding.Bin,
            Type t when t == typeof(byte[]) => HuffmanStringEncoding.Hex,
            Type t when t == typeof(string) => HuffmanStringEncoding.Base64,
            _ => throw new ArgumentOutOfRangeException(nameof(T))
        };
        options ??= new HuffmanEncoderOptions() { OutputEncoding = templateImpliedEncoding };
        
        if (options.OutputEncoding != templateImpliedEncoding)
            throw new ArgumentException("The specified output encoding does not match the type of the output.");
        
        var encodedBits = EncodeBitArray(input, options);
        return templateImpliedEncoding switch
        {
            HuffmanStringEncoding.Bin => (T)(object)encodedBits,
            HuffmanStringEncoding.Hex => (T)(object)HuffmanOutputEncoder.ToBytes(encodedBits),
            HuffmanStringEncoding.Base64 => (T)(object)HuffmanOutputEncoder.Base64.Encode(encodedBits),
            _ => throw new InvalidOperationException("The specified output encoding is not supported.")
        };
    }

    /// <summary>
    /// Decodes a byte array into a string using Huffman coding.
    /// </summary>
    /// <param name="input">The byte array to decode.</param>
    /// <param name="options"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns>The decoded string.</returns>
    public static string Decode(string input, HuffmanEncoderOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        
        if (input.Length == 0)
        {
            return string.Empty;
        }
        
        options ??= new HuffmanEncoderOptions();

        var encodedBits = options.InputEncoding switch
        {
            HuffmanStringEncoding.Auto => HuffmanOutputEncoder.Auto.Decode(input),
            HuffmanStringEncoding.Bin => HuffmanOutputEncoder.Bin.Decode(input),
            HuffmanStringEncoding.Hex => HuffmanOutputEncoder.Hex.Decode(input),
            HuffmanStringEncoding.Base64 => HuffmanOutputEncoder.Base64.Decode(input),
            _ => throw new ArgumentOutOfRangeException(nameof(options))
        };
        return DecodeBitArray(encodedBits, options);
    }

    /// <summary>
    /// Checks if the input string can be encoded using the current Huffman tree.
    /// </summary>
    /// <param name="input">The string to check.</param>
    /// <param name="options"></param>
    /// <returns>True if the string can be encoded, false otherwise.</returns>
    public static bool CanEncode(string? input, HuffmanEncoderOptions? options = null)
    {
        if (input is null)
        {
            return false;
        }
        
        options ??= new HuffmanEncoderOptions();
        
        var huffmanCode = options._huffmanCode;
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
    /// <param name="options"></param>
    /// <returns>The length of the encoded string in bits.</returns>
    /// <remarks>Ignores the FixedLengthInBits property when measuring the length of the encoded string.</remarks>
    public static int Measure(string input, HuffmanEncoderOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        
        options ??= new HuffmanEncoderOptions();
        
        var huffmanCode = options._huffmanCode;
        var result = 0;
        foreach (var character in input)
        {
            result += huffmanCode[character.ToString()].Length;
        }
        
        if (!string.IsNullOrEmpty(options.EndOfSequence))
        {
            result += huffmanCode[options.EndOfSequence].Length;
        }
        
        return result;
    }


    /// <summary>
    /// Encodes a string into a BitArray using Huffman coding.
    /// </summary>
    /// <param name="input">The string to encode.</param>
    /// <param name="options"></param>
    /// <returns>The encoded string as a BitArray.</returns>
    private static BitArray EncodeBitArray(string input, HuffmanEncoderOptions options)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        
        if (options.IsByteAlignmentRequired && string.IsNullOrEmpty(options.EndOfSequence))
        {
            throw new InvalidOperationException("The EndOfSequence property must be set when the AlignToByteSize or FixedLengthInBits property is set.");
        }

        var huffmanCode = options._huffmanCode;//GenerateHuffmanCode(options._root);
        
        if (!string.IsNullOrEmpty(options.EndOfSequence) && !huffmanCode.ContainsKey(options.EndOfSequence))
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
        
        if (!string.IsNullOrEmpty(options.EndOfSequence))
        {
            var eosArray = huffmanCode[options.EndOfSequence];
            foreach (bool bit in eosArray)
            {
                result.Add(bit);
            }
        }
        
        if (options.IsByteAlignmentRequired)
        {
            while (result.Count % 8 != 0)
            {
                result.Add(false);
            }
        }
        
        if (options.FixedLengthOutputInBytes > 0)
        {
            while (result.Count < (options.FixedLengthOutputInBytes * 8))
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
    /// <param name="options"></param>
    /// <returns>The decoded string.</returns>
    private static string DecodeBitArray(BitArray input, HuffmanEncoderOptions options)
    {
        var result = new StringBuilder();
        var currentNode = options._root;

        foreach (bool bit in input)
        {
            currentNode = bit ? currentNode!.Right : currentNode!.Left;

            if (currentNode is not null && currentNode.IsLeaf)
            {
                if (!string.IsNullOrEmpty(options.EndOfSequence) && currentNode.Sequence == options.EndOfSequence)
                    break;
                
                result.Append(currentNode.Sequence);
                currentNode = options._root;
            }
        }

        return result.ToString();
    }
    
}