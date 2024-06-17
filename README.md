# Forge.Algorithms.Huffman

Forge.Algorithms.Huffman is a C# library for encoding and decoding strings using Huffman coding. It provides a flexible and efficient way to compress and decompress data, and is particularly useful for applications that need to handle large amounts of text data.

## Installation

To install the Forge.Algorithms.Huffman library, you can download the source code from this repository and build it using your preferred C# compiler.

## Usage

Here is a basic example of how to use the Forge.Algorithms.Huffman library:

```csharp
using Forge.Algorithms.Huffman;

// Encode using the default Huffman encoder
var encoded = HuffmanEncoder.Encode(inputString);

// Encode using custom encoding parameters
var encoded = HuffmanEncoder.Encode(inputString, new HuffmanEncoderOptions()
{
    OutputEncoding = HuffmanStringEncoding.Base64
});

// Decode using the default Huffman decoder
var decoded = HuffmanDecoder.Decode(encoded);

// Decode using custom decoding parameters
var decoded = HuffmanDecoder.Decode(encoded, new HuffmanDecoderOptions()
{
    InputEncoding = HuffmanStringEncoding.Base64
});
```

## Documentation

For more detailed information about the classes and methods provided by the Forge.Algorithms.Huffman library, please refer to the source code comments.

## Contributing

Contributions to the Forge.Algorithms.Huffman project are welcome. Please submit a pull request or create an issue if you have any improvements or bug fixes.

## License

The Forge.Algorithms.Huffman library is licensed under the MIT License. Please see the LICENSE file for more details.