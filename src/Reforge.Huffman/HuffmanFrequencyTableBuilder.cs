namespace ReForge.Huffman;

/// <summary>
/// The HuffmanFrequencyTableBuilder class is used to build a frequency table for Huffman coding.
/// </summary>
public class HuffmanFrequencyTableBuilder
{
    private string _eosCharacter = string.Empty;
    private int _maxNGramLength = 1;
    private double _lengthWeight = 1.0;
    private IEnumerable<string> _sequences = Enumerable.Empty<string>();
    
    /// <summary>
    /// Sets the maximum length of n-grams to be considered in the frequency table.
    /// </summary>
    /// <param name="maxNGramLength">The maximum length of n-grams.</param>
    /// <returns>The current HuffmanFrequencyTableBuilder instance.</returns>
    public HuffmanFrequencyTableBuilder WithMaxNGramLength(int maxNGramLength)
    {
        _maxNGramLength = maxNGramLength;
        return this;
    }
    
    /// <summary>
    /// Sets the sequences to be considered in the frequency table.
    /// </summary>
    /// <param name="sequences">The sequences to be considered.</param>
    /// <returns>The current HuffmanFrequencyTableBuilder instance.</returns>
    public HuffmanFrequencyTableBuilder WithSequences(IEnumerable<string> sequences)
    {
        _sequences = sequences;
        return this;
    }
    
    /// <summary>
    /// Sets the weight of the length of n-grams in the frequency table.
    /// </summary>
    /// <param name="lengthWeight">The weight of the length of n-grams.</param>
    /// <returns>The current HuffmanFrequencyTableBuilder instance.</returns>
    public HuffmanFrequencyTableBuilder WithLengthWeight(double lengthWeight)
    {
        _lengthWeight = lengthWeight;
        return this;
    }
    
    /// <summary>
    /// Sets the character to be used as the end of sequence marker in the frequency table.
    /// </summary>
    /// <param name="eosCharacter">The end of sequence character.</param>
    /// <returns>The current HuffmanFrequencyTableBuilder instance.</returns>
    public HuffmanFrequencyTableBuilder WithEndOfSequenceCharacter(string eosCharacter)
    {
        _eosCharacter = eosCharacter;
        return this;
    }
    
    /// <summary>
    /// Builds the Huffman frequency table based on the provided parameters.
    /// </summary>
    /// <returns>The built Huffman frequency table.</returns>
    public HuffmanFrequencyTable Build()
    {
        var result = GenerateFrequencyTable();
        result = OptimiseFrequencyTable(result);
        return result;
    }

    /// <summary>
    /// Generates a frequency table based on the sequences and parameters provided.
    /// </summary>
    /// <returns>A HuffmanFrequencyTable with the frequencies of n-grams in the sequences.</returns>
    private HuffmanFrequencyTable GenerateFrequencyTable()
    {
        var result = new HuffmanFrequencyTable();
        if (!string.IsNullOrEmpty(_eosCharacter))
            result[_eosCharacter] = 0;

        foreach (var sequence in _sequences)
        {
            for (var ngramLength = _maxNGramLength; ngramLength >= 1; ngramLength--)
            {
                for (var i = 0; i <= sequence.Length - ngramLength; i++)
                {
                    var ngram = sequence.Substring(i, ngramLength);
                    if (!result.TryAdd(ngram, 1))
                    {
                        result[ngram]++;
                    }
                }
            }
            
            if (!string.IsNullOrEmpty(_eosCharacter))
                result[_eosCharacter]++;
        }
        
        return result;
    }
    
    /// <summary>
    /// Optimises the frequency table by sorting n-grams by a weight function
    /// </summary>
    /// <param name="frequencyTable">The initial frequency table to be optimised.</param>
    /// <returns>An optimised HuffmanFrequencyTable.</returns>
    private HuffmanFrequencyTable OptimiseFrequencyTable(HuffmanFrequencyTable frequencyTable)
    {
        var sortedNGrams = frequencyTable
            .OrderByDescending(nf => CalculateWeight(nf.Key.Length, nf.Value))
            .ToList();
       
        var result = new HuffmanFrequencyTable();
        
        if (!string.IsNullOrEmpty(_eosCharacter))
            result[_eosCharacter] = frequencyTable[_eosCharacter];
        
        foreach (var sequence in _sequences)
        {
            var tempSequence = sequence;
            var ngramRepresentation = new List<string>();

            foreach (var ngram in sortedNGrams)
            {
                while (tempSequence.Contains(ngram.Key))
                {
                    ngramRepresentation.Add(ngram.Key);
                    tempSequence = tempSequence.Replace(ngram.Key, string.Empty);
                }

                if (string.IsNullOrEmpty(tempSequence))
                {
                    break;
                }
            }

            foreach (var ngram in ngramRepresentation)
            {
                if (!result.TryAdd(ngram, 1))
                {
                    result[ngram]++;
                }
            }
        }
        
        return result;
    }
    
    /// <summary>
    /// Calculates the weight of an n-gram based on its length and frequency.
    /// </summary>
    /// <param name="length">The length of the n-gram.</param>
    /// <param name="frequency">The frequency of the n-gram.</param>
    /// <returns>The calculated weight of the n-gram.</returns>
    private double CalculateWeight(int length, double frequency)
    {
        return length * _lengthWeight + frequency;
    }
}