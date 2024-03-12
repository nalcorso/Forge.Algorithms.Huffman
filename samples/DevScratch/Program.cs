// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using Dumpify;
using Forge.Algorithms.HuffmanCoding;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("ReForge Huffman Encoder/Decoder");

var data_sources = new string[] { "collector_numbers.json", "set_codes.json" };
var sequences = new List<string>();
foreach (var source in data_sources)
{
    var sequence = JsonSerializer.Deserialize<string[]>(File.ReadAllBytes(source));
    if (sequence is null)
    {
        Console.WriteLine("Failed to load sequences");
        return;
    }
    
    sequences.AddRange(sequence.Select(s => s.Trim().ToLower()));
}

Console.WriteLine("Loaded " + sequences.Count + " sequences");


var _frequencyTable = HuffmanFrequencyTable.Create()
    .WithMaxNGramLength(3)
    .WithSequences(sequences)
    .WithLengthWeight(10.0)
    .WithEndOfSequenceCharacter("\0")
    .Build();

// Save the frequency table to a json file
File.WriteAllBytes("frequency_table.json", _frequencyTable.ToJson());

Console.WriteLine("Frequency Table: Count = " + _frequencyTable.Count);
_frequencyTable.Dump();

var _huffmanEncoder = HuffmanEncoder.Create()
    .WithFrequencyTable(_frequencyTable)
    .WithFixedLength(64)
    .WithEndOfSequenceCharacter("\0")
    .Build();

var maxBits = 0;
foreach (var sequence in sequences)
{
    var encodingLength = _huffmanEncoder.Measure(sequence);
    maxBits = Math.Max(maxBits, encodingLength);
}

Console.WriteLine("Max bits: " + maxBits);

// Round up maxBits to the nearest byte
maxBits = (maxBits + 7) / 8 * 8;

Console.WriteLine("Max bytes: " + maxBits / 8);

foreach (var sequence in sequences)
{
    var encoded = _huffmanEncoder.Encode(sequence);
    var decoded = _huffmanEncoder.Decode(encoded);
    
    if (sequence != decoded)
    {
        Console.WriteLine("Failed to encode/decode sequence");
        Console.WriteLine("Original: " + sequence);
        Console.WriteLine("Encoded: " + BitConverter.ToString(encoded).Replace("-", "") + " (" + encoded.Length + " bytes)");
        Console.WriteLine("Decoded: " + decoded);
        return;
    }
}


