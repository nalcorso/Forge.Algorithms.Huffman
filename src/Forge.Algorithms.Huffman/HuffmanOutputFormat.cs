using System.Collections;
using System.Text;

namespace Forge.Algorithms.HuffmanCoding;

public abstract class HuffmanOutputEncoder
{
    public static HuffmanOutputEncoder Auto { get; } = new AutoFormat();
    public static HuffmanOutputEncoder Bin { get; } = new BinFormat();
    public static HuffmanOutputEncoder Hex { get; } = new HexFormat();
    public static HuffmanOutputEncoder Base64 { get; } = new Base64Format();

    public abstract string Encode(BitArray input);
    public abstract BitArray Decode(string input);

    private sealed class AutoFormat : HuffmanOutputEncoder
    {
        public override string Encode(BitArray input)
        {
            return HuffmanOutputEncoder.Hex.Encode(input);
        }

        public override BitArray Decode(string input)
        {
            // If the string is a valid binary string, decode it as binary.
            if (input.All(c => c == '0' || c == '1'))
            {
                return HuffmanOutputEncoder.Bin.Decode(input);
            }
            
            // If the input is a valid hex string, decode it as hex.
            if (input.All(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')))
            {
                return HuffmanOutputEncoder.Hex.Decode(input);
            }
            
            // Otherwise, decode it as base64.
            return HuffmanOutputEncoder.Base64.Decode(input);
        }
    }

    private sealed class BinFormat : HuffmanOutputEncoder
    {
        public override string Encode(BitArray input)
        {
            return string.Join("", input.Cast<bool>().Select(bit => bit ? "1" : "0"));
        }
        
        public override BitArray Decode(string input)
        {
            return new BitArray(input.Select(c => c == '1').ToArray());
        }
    }

    private sealed class HexFormat : HuffmanOutputEncoder
    {
        public override string Encode(BitArray input)
        {
            return BitConverter.ToString(ToBytes(input)).Replace("-", "");
        }
        
        public override BitArray Decode(string input)
        {
            var numBytes = input.Length / 2;
            var bytes = new byte[numBytes];
            for (var i = 0; i < numBytes; i++)
            {
                bytes[i] = Convert.ToByte(input.Substring(i * 2, 2), 16);
            }
            return new BitArray(bytes);
        }
    }
    
    private sealed class Base64Format : HuffmanOutputEncoder
    {
        public override string Encode(BitArray input)
        {
            return Convert.ToBase64String(ToBytes(input)).Replace("-", "");
        }
        
        public override BitArray Decode(string input)
        {
            return new BitArray(Convert.FromBase64String(input));
        }
    }
    
    private static byte[] ToBytes(BitArray bitArray)
    {
        if (bitArray.Length % 8 != 0)
            throw new ArgumentException("BitArray length must be multiple of 8", nameof(bitArray));

        var bytes = new byte[bitArray.Length / 8];
        bitArray.CopyTo(bytes, 0);
        
        return bytes;
    }
}