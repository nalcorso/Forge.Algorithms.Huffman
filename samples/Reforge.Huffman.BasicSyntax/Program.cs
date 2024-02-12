// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json;
using Dumpify;
using Reforge.Huffman;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("ReForge Huffman Encoder/Decoder");

// Load the sequences from the json file using system.text.json
var sequences1 = JsonSerializer.Deserialize<string[]>(File.ReadAllBytes("collector_numbers.json"));
if (sequences1 is null)
{
    Console.WriteLine("Failed to load sequences");
    return;
}

var sequences2 = JsonSerializer.Deserialize<string[]>(File.ReadAllBytes("set_codes.json"));
if (sequences2 is null)
{
    Console.WriteLine("Failed to load sequences");
    return;
}

// Trim and make lowercase
for (int i = 0; i < sequences1.Length; i++)
{
    sequences1[i] = sequences1[i].Trim().ToLower();
}

for (int i = 0; i < sequences2.Length; i++)
{
    sequences2[i] = sequences2[i].Trim().ToLower();
}

var combined = new List<string>(sequences1);
combined.AddRange(sequences2);
Console.WriteLine("Loaded " + combined.Count + " sequences");


var _frequencyTable = HuffmanFrequencyTable.Create()
    .WithMaxNGramLength(3)
    .WithSequences(combined)
    .WithLengthWeight(10.0)
    .WithNullCharacter()
    .Build();

// Save the frequency table to a json file
File.WriteAllBytes("frequency_table.json", _frequencyTable.ToJson());

Console.WriteLine("Frequency Table: Count = " + _frequencyTable.Count);
_frequencyTable.Dump();

var _huffmanEncoder = HuffmanEncoder.Create()
    .WithFrequencyTable(_frequencyTable)
    .Build();

var maxBits = 0;
foreach (var sequence in combined)
{
    var encodedBin = _huffmanEncoder.Encode(sequence);
    maxBits = Math.Max(maxBits, encodedBin.Length);
}

Console.WriteLine("Max bits: " + maxBits);

// Round up maxBits to the nearest byte
maxBits = (maxBits + 7) / 8 * 8;

Console.WriteLine("Max bytes: " + maxBits / 8);

foreach (var sequence in combined)
{
    var encodedHex = _huffmanEncoder.EncodeHex(sequence, 64);
    var encodedBin = _huffmanEncoder.Encode(sequence, 64);
    
    var decodedBin = _huffmanEncoder.Decode(encodedBin);
    var decodedHex = _huffmanEncoder.DecodeHex(encodedHex);
    if (sequence != decodedBin)
    {
        Console.WriteLine("Failed to encode/decode binary sequence: " + sequence);
        Console.WriteLine("Encoded: " + encodedBin + " (" + encodedBin.Length + " bits)");
        Console.WriteLine("Decoded: " + decodedBin);
        return;
    }
    if (sequence != decodedHex)
    {
        Console.WriteLine("Failed to encode/decode hexadecimal sequence: " + sequence);
        Console.WriteLine("Encoded: " + encodedHex + " (" + encodedHex.Length + " bytes)");
        Console.WriteLine("Decoded: " + decodedHex);
        return;
    }
}


