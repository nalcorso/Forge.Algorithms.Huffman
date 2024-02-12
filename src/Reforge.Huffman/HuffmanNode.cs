namespace Reforge.Huffman;

public class HuffmanNode
{
    public string Sequence { get; set; } = string.Empty;
    public int Frequency { get; set; }
    public HuffmanNode? Left { get; set; }
    public HuffmanNode? Right { get; set; }
    
    public bool IsLeaf => Left is null && Right is null;
}