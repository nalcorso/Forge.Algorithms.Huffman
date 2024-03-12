using System.Text.Json;

namespace Forge.Algorithms.HuffmanCoding;

/// <summary>
/// Represents a frequency table for Huffman coding, which is a dictionary mapping symbols to their frequencies.
/// </summary>
public class HuffmanFrequencyTable : Dictionary<string, double>
{
    /// <summary>
    /// Creates a new instance of the HuffmanFrequencyTableBuilder class.
    /// </summary>
    /// <returns>A new HuffmanFrequencyTableBuilder instance.</returns>
    public static HuffmanFrequencyTableBuilder Create()
    {
        return new HuffmanFrequencyTableBuilder();
    }

    /// <summary>
    /// Deserializes a JSON string to a HuffmanFrequencyTable instance.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>A HuffmanFrequencyTable instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the deserialization results in null, the JSON format is invalid, the conversion is not supported, the provided JSON string is null, or the provided string is not a valid JSON string.</exception>
    public static HuffmanFrequencyTable FromJson(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<HuffmanFrequencyTable>(json)
                   ?? throw new ArgumentException("Deserialization resulted in null.");
        }
        catch (JsonException ex)
        {
            throw new ArgumentException("Invalid JSON format.", ex);
        }
        catch (NotSupportedException ex)
        {
            throw new ArgumentException("The conversion is not supported.", ex);
        }
        catch (ArgumentNullException ex)
        {
            throw new ArgumentException("The provided JSON string is null.", ex);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("The provided string is not a valid JSON string.", ex);
        }
    }

    /// <summary>
    /// Serializes the HuffmanFrequencyTable instance to a byte array in UTF8 format.
    /// </summary>
    /// <returns>A byte array representing the serialized HuffmanFrequencyTable instance.</returns>
    public byte[] ToJson()
    {
        return JsonSerializer.SerializeToUtf8Bytes(this);
    }
}