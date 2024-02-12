using System.Text;

namespace Reforge.Huffman;

public class HuffmanEncoder
{
    private HuffmanNode _root;
    
    public HuffmanEncoder(HuffmanNode root)
    {
        _root = root;
    }
    
    public static HuffmanEncoderBuilder Create()
    {
        return new HuffmanEncoderBuilder();
    }
    
    public string Encode(string input, int fixedLength = 0)
    {
        var huffmanCode = GenerateHuffmanCode(_root);
        var encoded = new StringBuilder();

        foreach (var character in input)
        {
            encoded.Append(huffmanCode[character.ToString()]);
        }
        encoded.Append(huffmanCode["\0"]);

        return fixedLength > 0 ? encoded.ToString().PadRight(fixedLength, '0') : encoded.ToString();
    }
    
    public string EncodeHex(string input, int fixedLength = 0)
    {
        var encoded = Encode(input, fixedLength);
        var result = new StringBuilder();

        for (int i = 0; i < encoded.Length; i += 4)
        {
            var hex = encoded.Substring(i, 4);
            result.Append(Convert.ToInt32(hex, 2).ToString("X"));
        }

        return result.ToString();
    }
    
    public string Decode(string input)
    {
        var decoded = new StringBuilder();
        var currentNode = _root;

        foreach (var bit in input)
        {
            currentNode = bit == '0' ? currentNode.Left : currentNode.Right;

            if (currentNode.IsLeaf)
            {
                if (currentNode.Sequence == "\0")
                    break;
                
                decoded.Append(currentNode.Sequence);
                currentNode = _root;
            }
        }

        return decoded.ToString();
    }
 
    public string DecodeHex(string input)
    {
        var decoded = new StringBuilder();
        var binary = new StringBuilder();

        foreach (var character in input)
        {
            var bitString = Convert.ToString(Convert.ToInt32(character.ToString(), 16), 2).PadLeft(4, '0');
            binary.Append(bitString);
        }
        
        return Decode(binary.ToString());
    }
    
    private Dictionary<string, string> GenerateHuffmanCode(HuffmanNode? node, string code = "")
    {
        if (node is null)
        {
            return new Dictionary<string, string>();
        }

        if (node.IsLeaf)
        {
            return new Dictionary<string, string> { { node.Sequence, code } };
        }

        var leftCode = GenerateHuffmanCode(node.Left, code + "0");
        var rightCode = GenerateHuffmanCode(node.Right, code + "1");

        return leftCode.Concat(rightCode).ToDictionary(x => x.Key, x => x.Value);
    }
}