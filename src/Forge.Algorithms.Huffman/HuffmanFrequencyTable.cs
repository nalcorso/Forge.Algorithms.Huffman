using System.Collections;
using System.Text.Json;

namespace Forge.Algorithms.HuffmanCoding;

/// <summary>
/// Represents a frequency table for Huffman coding, which is a dictionary mapping symbols to their frequencies.
/// </summary>
public class HuffmanFrequencyTable : Dictionary<string, double>
{
    public static HuffmanFrequencyTable ASCII => CreateAsciiFrequencyTable();
    
    private static HuffmanFrequencyTable CreateAsciiFrequencyTable()
    {
        var frequencyTable = new HuffmanFrequencyTable();

        //const string characterFrequencyJsonString = """[{"Char": 32,"Freq": 0.167564443682168},{"Char": 101,"Freq": 0.08610229517681191},{"Char": 116,"Freq": 0.0632964962389326},{"Char": 97,"Freq": 0.0612553996079051},{"Char": 110,"Freq": 0.05503703643138501},{"Char": 105,"Freq": 0.05480626188138746},{"Char": 111,"Freq": 0.0541904405334676},{"Char": 115,"Freq": 0.0518864979648296},{"Char": 114,"Freq": 0.051525029341199825},{"Char": 108,"Freq": 0.03218192615049607},{"Char": 100,"Freq": 0.03188948073064199},{"Char": 104,"Freq": 0.02619237267611581},{"Char": 99,"Freq": 0.02500268898936656},{"Char": 10,"Freq": 0.019578060965172565},{"Char": 117,"Freq": 0.019247776378510318},{"Char": 109,"Freq": 0.018140172626462205},{"Char": 112,"Freq": 0.017362092874808832},{"Char": 102,"Freq": 0.015750347191785568},{"Char": 103,"Freq": 0.012804659959943725},{"Char": 46,"Freq": 0.011055184780313847},{"Char": 121,"Freq": 0.010893686962847832},{"Char": 98,"Freq": 0.01034644514338097},{"Char": 119,"Freq": 0.009565830104169261},{"Char": 44,"Freq": 0.008634492219614468},{"Char": 118,"Freq": 0.007819143740853554},{"Char": 48,"Freq": 0.005918945715880591},{"Char": 107,"Freq": 0.004945712204424292},{"Char": 49,"Freq": 0.004937789430804492},{"Char": 83,"Freq": 0.0030896915651553373},{"Char": 84,"Freq": 0.0030701064687671904},{"Char": 67,"Freq": 0.002987392712176473},{"Char": 50,"Freq": 0.002756237869045172},{"Char": 56,"Freq": 0.002552781042488694},{"Char": 53,"Freq": 0.0025269211093936652},{"Char": 65,"Freq": 0.0024774830020061096},{"Char": 57,"Freq": 0.002442242504945237},{"Char": 120,"Freq": 0.0023064144740073764},{"Char": 51,"Freq": 0.0021865587546870337},{"Char": 73,"Freq": 0.0020910417959267183},{"Char": 45,"Freq": 0.002076717421222119},{"Char": 54,"Freq": 0.0019199098857390264},{"Char": 52,"Freq": 0.0018385271551164353},{"Char": 55,"Freq": 0.0018243295447897528},{"Char": 77,"Freq": 0.0018134911904778657},{"Char": 66,"Freq": 0.0017387002075069484},{"Char": 34,"Freq": 0.0015754276887500987},{"Char": 39,"Freq": 0.0015078622753204398},{"Char": 80,"Freq": 0.00138908405321239},{"Char": 69,"Freq": 0.0012938206232079082},{"Char": 78,"Freq": 0.0012758834637326799},{"Char": 70,"Freq": 0.001220297284016159},{"Char": 82,"Freq": 0.0011037374385216535},{"Char": 68,"Freq": 0.0010927723198318497},{"Char": 85,"Freq": 0.0010426370083657518},{"Char": 113,"Freq": 0.00100853739070613},{"Char": 76,"Freq": 0.0010044809306127922},{"Char": 71,"Freq": 0.0009310209736100016},{"Char": 74,"Freq": 0.0008814561018445294},{"Char": 72,"Freq": 0.0008752446473266058},{"Char": 79,"Freq": 0.0008210528757671701},{"Char": 87,"Freq": 0.0008048270353938186},{"Char": 106,"Freq": 0.000617596049210692},{"Char": 122,"Freq": 0.0005762708620098124},{"Char": 47,"Freq": 0.000519607185080999},{"Char": 60,"Freq": 0.00044107665296153596},{"Char": 62,"Freq": 0.0004404428310719519},{"Char": 75,"Freq": 0.0003808001912620934},{"Char": 41,"Freq": 0.0003314254660634964},{"Char": 40,"Freq": 0.0003307916441739124},{"Char": 86,"Freq": 0.0002556203680692448},{"Char": 89,"Freq": 0.00025194420110965734},{"Char": 58,"Freq": 0.00012036277683200988},{"Char": 81,"Freq": 0.00010001709417636208},{"Char": 90,"Freq": 0.00008619977698342993},{"Char": 88,"Freq": 0.00006572732994986532},{"Char": 59,"Freq": 0.00000741571610813331},{"Char": 63,"Freq": 0.000004626899793963519},{"Char": 127,"Freq": 0.0000031057272589618137},{"Char": 94,"Freq": 0.0000022183766135441526},{"Char": 38,"Freq": 0.0000020282300466689395},{"Char": 43,"Freq": 0.0000015211725350017046},{"Char": 91,"Freq": 6.97204078542448e-7},{"Char": 93,"Freq": 6.338218895840436e-7},{"Char": 36,"Freq": 5.070575116672349e-7},{"Char": 33,"Freq": 5.070575116672349e-7},{"Char": 42,"Freq": 4.436753227088305e-7},{"Char": 61,"Freq": 2.5352875583361743e-7},{"Char": 126,"Freq": 1.9014656687521307e-7},{"Char": 95,"Freq": 1.2676437791680872e-7},{"Char": 9,"Freq": 1.2676437791680872e-7},{"Char": 123,"Freq": 6.338218895840436e-8},{"Char": 64,"Freq": 6.338218895840436e-8},{"Char": 5,"Freq": 6.338218895840436e-8},{"Char": 27,"Freq": 6.338218895840436e-8},{"Char": 30,"Freq": 6.338218895840436e-8}]""";        
        //var characterFrequencyList = JsonSerializer.Deserialize<List<(char Char, double Freq)>>(characterFrequencyJsonString);
        
        foreach (var c in Enumerable.Range(0, 128).Select(x => (char)x))
        {
            if (char.IsLetterOrDigit(c))
                frequencyTable[c.ToString()] = 1;
            else if (char.IsWhiteSpace(c))
                frequencyTable[c.ToString()] = 0.5;
            else if (char.IsSymbol(c) || char.IsPunctuation(c))
                frequencyTable[c.ToString()] = 0.25;
            else
                frequencyTable[c.ToString()] = 0.1;
        }
        
        return frequencyTable;
    }

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
    
    /// <summary>
    /// Generates a Huffman tree from a given frequency table.
    /// </summary>
    /// <param name="frequencyTable">The frequency table to generate the Huffman tree from.</param>
    /// <returns>The root node of the generated Huffman tree.</returns>
    internal HuffmanNode BuildTree()
    {
        if (this.Count == 0)
            return new HuffmanNode();
        
        var nodes = this.Select(x => new HuffmanNode
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
    
    /// <summary>
    /// Generates a Huffman code for a given HuffmanNode.
    /// </summary>
    /// <param name="node">The HuffmanNode to generate the code for.</param>
    /// <param name="code">The current BitArray code.</param>
    /// <returns>A dictionary mapping sequences to their corresponding BitArray codes.</returns>
    internal Dictionary<string, BitArray> GenerateHuffmanCode(HuffmanNode? node, BitArray? code = null)
    {
        var huffmanCode = new Dictionary<string, BitArray>();

        if (node == null)
        {
            return huffmanCode;
        }

        if (code == null)
        {
            code = new BitArray(0);
        }

        if (node.IsLeaf)
        {
            huffmanCode[node.Sequence] = code;
        }
        else
        {
            if (node.Left != null)
            {
                var leftCode = new BitArray(code.Length + 1);
                for (int i = 0; i < code.Length; i++)
                {
                    leftCode[i] = code[i];
                }
                leftCode[leftCode.Length - 1] = false;
                foreach (var pair in GenerateHuffmanCode(node.Left, leftCode))
                {
                    huffmanCode[pair.Key] = pair.Value;
                }
            }

            if (node.Right != null)
            {
                var rightCode = new BitArray(code.Length + 1);
                for (int i = 0; i < code.Length; i++)
                {
                    rightCode[i] = code[i];
                }
                rightCode[rightCode.Length - 1] = true;
                foreach (var pair in GenerateHuffmanCode(node.Right, rightCode))
                {
                    huffmanCode[pair.Key] = pair.Value;
                }
            }
        }

        return huffmanCode;
    }
}