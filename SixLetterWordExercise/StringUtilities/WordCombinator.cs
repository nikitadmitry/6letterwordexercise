using System;
using System.Collections.Generic;
using System.Linq;

namespace StringUtilities {
  public class WordCombinator : IWordCombinator {
    public IEnumerable<IEnumerable<string>> GetCombinations(IEnumerable<string> lines, int wordLength) {
      if (lines == null) {
        throw new ArgumentNullException(nameof(lines));
      }
      if (wordLength <= 0) {
        throw new ArgumentException(nameof(wordLength));
      }

      var sequenceBuckets = GetSequenceBuckets(lines, wordLength);

      var combinations = sequenceBuckets[wordLength]
        .Select(word => GetCombinationsForWord(sequenceBuckets, word))
        .SelectMany(x => x);

      return combinations;
    }

    private static Dictionary<int, HashSet<string>> GetSequenceBuckets(IEnumerable<string> lines, int wordLength) {
      var sequenceBuckets = Enumerable.Range(1, wordLength)
        .Select(x => (Key: x, Value: new HashSet<string>()))
        .ToDictionary(x => x.Key, x => x.Value);

      foreach (var line in lines) {
        if (line.Length <= wordLength && line.Length > 0) {
          sequenceBuckets[line.Length].Add(line);
        }
      }

      return sequenceBuckets;
    }

    private static IEnumerable<IEnumerable<string>> GetCombinationsForWord(
      IDictionary<int, HashSet<string>> sequenceBuckets, string sequence,
      bool isSubsequence = false) {
      if (isSubsequence && sequenceBuckets[sequence.Length].Contains(sequence)) {
        yield return new[] { sequence };
      }

      for (var i = 1; i < sequence.Length; ++i) {
        var subsequence = sequence.Substring(0, i);
        var subsequenceExists = sequenceBuckets[subsequence.Length].Contains(subsequence);
        if (!subsequenceExists) {
          continue;
        }

        var nextSubsequence = sequence.Substring(i, sequence.Length - i);
        var nextCombinations = GetCombinationsForWord(sequenceBuckets, nextSubsequence, true);
        foreach (var combination in nextCombinations) {
          yield return new[] { subsequence }.Concat(combination);
        }
      }
    }
  }
}
