namespace Forge.Algorithms.Huffman;

public static class HuffmanEncodingHelper
{
    public enum EncodingType
    {
        Unknown,
        Bin,
        Hex,
        Base64
    }
    
    /// <summary>
    /// Detects the encoding of the input string.
    /// </summary>
    /// <param name="input">The input string to analyse.</param>
    /// <returns>The detected encoding type.</returns>
    /// <remarks>This is a weak detection method, as it is possible for the input to be ambiguous. For example, a string
    /// of all 0s and 1s could be binary or base64 encoded. This method will return the first match it finds in the order
    /// of Bin, Hex, Base64, Unknown.</remarks>
    public static EncodingType DetectEncoding(string input)
    {
        if (input.All(c => c == '0' || c == '1'))
        {
            return EncodingType.Bin;
        }

        if (input.All(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')))
        {
            return EncodingType.Hex;
        }

        try
        {
            _ = Convert.FromBase64String(input);
            return EncodingType.Base64;
        }
        catch (FormatException) {/* Not base64 */}
        
        return EncodingType.Unknown;
    }
}