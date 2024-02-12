using System.Text.Json;

namespace Reforge.Huffman;

public class HuffmanFrequencyTable : Dictionary<string, double>
{
    public static HuffmanFrequencyTableBuilder Create()
    {
        return new HuffmanFrequencyTableBuilder();
    }
    
    public byte[] ToJson()
    {
        return JsonSerializer.SerializeToUtf8Bytes(this);
    }
}