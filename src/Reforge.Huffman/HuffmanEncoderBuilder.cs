namespace Reforge.Huffman;

public class HuffmanEncoderBuilder
{
    private HuffmanFrequencyTable _frequencyTable = new HuffmanFrequencyTable();
    
    public HuffmanEncoderBuilder WithFrequencyTable(HuffmanFrequencyTable frequencyTable)
    {
        _frequencyTable = frequencyTable;
        return this;
    }
    
    public HuffmanEncoder Build()
    {
        var root = GenerateHuffmanTree();
        return new HuffmanEncoder(root);
    }
    
    private HuffmanNode GenerateHuffmanTree()
    {
        var nodes = _frequencyTable.Select(x => new HuffmanNode
        {
            Sequence = x.Key,
            Frequency = (int)x.Value
        }).ToList();
        
        while (nodes.Count > 1)
        {
            nodes = nodes.OrderBy(x => x.Frequency).ToList();
            var left = nodes[0];
            var right = nodes[1];
            var parent = new HuffmanNode
            {
                Sequence = left.Sequence + right.Sequence,
                Frequency = left.Frequency + right.Frequency,
                Left = left,
                Right = right
            };
            nodes.Remove(left);
            nodes.Remove(right);
            nodes.Add(parent);
        }
        
        return nodes.Single();
    }
    
}