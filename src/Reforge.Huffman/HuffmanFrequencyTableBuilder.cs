namespace Reforge.Huffman;

public class HuffmanFrequencyTableBuilder
{
    private bool _includeNullCharacter = false;
    private int _maxNGramLength = 1;
    private double _lengthWeight = 1.0;
    private IEnumerable<string> _sequences = Enumerable.Empty<string>();
    
    public HuffmanFrequencyTableBuilder WithMaxNGramLength(int maxNGramLength)
    {
        _maxNGramLength = maxNGramLength;
        return this;
    }
    
    public HuffmanFrequencyTableBuilder WithSequences(IEnumerable<string> sequences)
    {
        _sequences = sequences;
        return this;
    }
    
    public HuffmanFrequencyTableBuilder WithLengthWeight(double lengthWeight)
    {
        _lengthWeight = lengthWeight;
        return this;
    }
    
    public HuffmanFrequencyTableBuilder WithNullCharacter()
    {
        _includeNullCharacter = true;
        return this;
    }
    
    public HuffmanFrequencyTable Build()
    {
        var result = GenerateFrequencyTable();
        result = OptimiseFrequencyTable(result);
        
        // ... implementation ...
        
        return result;
    }

    private HuffmanFrequencyTable GenerateFrequencyTable()
    {
        var result = new HuffmanFrequencyTable();
        if (_includeNullCharacter)
            result["\0"] = 0; // Add the null character to the frequency table

        // Generate all possible ngrams and count their frequencies
        foreach (var sequence in _sequences)
        {
            for (int ngramLength = _maxNGramLength; ngramLength >= 1; ngramLength--)
            {
                for (int i = 0; i <= sequence.Length - ngramLength; i++)
                {
                    var ngram = sequence.Substring(i, ngramLength);
                    if (result.ContainsKey(ngram))
                    {
                        result[ngram]++;
                    }
                    else
                    {
                        result[ngram] = 1;
                    }
                }
            }
            
            if (_includeNullCharacter)
                result["\0"]++; // Add the null character to the frequency table
        }

        
        return result;
    }
    
    private HuffmanFrequencyTable OptimiseFrequencyTable(HuffmanFrequencyTable frequencyTable)
    {
        
        //var sortedNGrams = frequencyTable.OrderByDescending(nf => nf.Value).ToList();
        var sortedNGrams = frequencyTable
            .OrderByDescending(nf => CalculateWeight(nf.Key.Length, nf.Value))
            .ToList();
       
        var result = new HuffmanFrequencyTable();
        if (_includeNullCharacter)
            result["\0"] = frequencyTable["\0"]; // Add the null character to the frequency table
        
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

            // Add all of the ngrams from ngramRepresentation to the result
            foreach (var ngram in ngramRepresentation)
            {
                if (result.ContainsKey(ngram))
                {
                    result[ngram]++;
                }
                else
                {
                    result[ngram] = 1;
                }
            }
        }
        
        return result;
    }
    
    private double CalculateWeight(int length, double frequency)
    {
        return length * _lengthWeight + frequency;
    }
}