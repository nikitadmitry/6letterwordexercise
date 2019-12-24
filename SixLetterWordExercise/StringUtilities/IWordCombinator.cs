using System.Collections.Generic;

namespace StringUtilities {
  public interface IWordCombinator {
    /// <summary>
    /// Returns all possible sequences of substrings to build words of a specified length.
    /// </summary>
    /// <param name="lines">Collection of strings and substrings.</param>
    /// <param name="wordLength">Length of the word.</param>
    /// <returns>Collection of sequences of substrings.</returns>
    IEnumerable<IEnumerable<string>> GetCombinations(IEnumerable<string> lines, int wordLength);
  }
}