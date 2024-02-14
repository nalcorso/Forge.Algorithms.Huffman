# ReForge.Huffman

ReForge.Huffman is a C# library for encoding and decoding strings using Huffman coding. It provides a flexible and efficient way to compress and decompress data, and is particularly useful for applications that need to handle large amounts of text data.

## Installation

To install the ReForge.Huffman library, you can download the source code from this repository and build it using your preferred C# compiler.

## Usage

Here is a basic example of how to use the ReForge.Huffman library:

```csharp
using ReForge.Huffman;

// Create a frequency table
var frequencyTable = HuffmanFrequencyTable.Create()
    .WithMaxNGramLength(3)
    .WithSequences(sequences)
    .WithLengthWeight(10.0)
    .WithEndOfSequenceCharacter("\0")
    .Build();

// Create a Huffman encoder
var huffmanEncoder = HuffmanEncoder.Create()
    .WithFrequencyTable(frequencyTable)
    .WithFixedLength(64)
    .WithEndOfSequenceCharacter("\0")
    .Build();

// Encode a string
var encoded = huffmanEncoder.Encode("hello world");

// Decode a string
var decoded = huffmanEncoder.Decode(encoded);
```

## Documentation

For more detailed information about the classes and methods provided by the ReForge.Huffman library, please refer to the source code comments.

## Contributing

Contributions to the ReForge.Huffman project are welcome. Please submit a pull request or create an issue if you have any improvements or bug fixes.

## License

The ReForge.Huffman library is licensed under the MIT License. Please see the LICENSE file for more details.