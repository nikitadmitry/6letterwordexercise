using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace StringUtilities.Tests {
  public class WordCombinatorTests {
    private readonly IWordCombinator _subject = new WordCombinator();

    [Fact]
    public void GetCombinations_WordExists_ReturnsCombination() {
      var lines = new List<string>
      {
        "foobar",
        "fo",
        "obar",
        "foo",
        "bar",
        "ob",
        "ar"
      };

      var combinations = _subject.GetCombinations(lines, 6).ToList();

      var expectedCombinations = new List<string[]>
      {
        new []{"fo", "obar"},
        new []{"fo", "ob", "ar"},
        new []{"foo", "bar"}
      };
      combinations.Should().BeEquivalentTo(expectedCombinations);
    }

    [Fact]
    public void GetCombinations_WordDoesntExist_ReturnsNothing() {
      var lines = new List<string>
      {
        "fo",
        "obar",
        "foo",
        "bar",
        "ob",
        "ar"
      };

      var combinations = _subject.GetCombinations(lines, 6).ToList();

      combinations.Should().BeEmpty();
    }

    [Fact]
    public void GetCombinations_HasNoCompleteCombinations_ReturnsNothing() {
      var lines = new List<string>
      {
        "foobar",
        "foo"
      };

      var combinations = _subject.GetCombinations(lines, 6).ToList();

      combinations.Should().BeEmpty();
    }

    [Fact]
    public void GetCombinations_HasEmptyLines_IgnoresThose() {
      var lines = new List<string>
      {
        "foobar",
        "foo",
        "",
        "bar"
      };

      var combinations = _subject.GetCombinations(lines, 6).ToList();

      combinations.Should().ContainSingle();
      combinations.Single().Should().BeEquivalentTo("foo", "bar");
    }

    [Fact]
    public void GetCombinations_WordExistsMultipleTimes_ReturnsSingleCombination() {
      var lines = new List<string>
      {
        "foobar",
        "foobar",
        "foo",
        "bar"
      };

      var combinations = _subject.GetCombinations(lines, 6).ToList();

      combinations.Should().ContainSingle();
      combinations.Single().Should().BeEquivalentTo("foo", "bar");
    }

    [Fact]
    public void GetCombinations_HasRepeatedLetters_ReturnsCombination() {
      var lines = new List<string>
      {
        "zambia",
        "z",
        "a",
        "m",
        "b",
        "i"
      };

      var combinations = _subject.GetCombinations(lines, 6).ToList();

      combinations.Should().ContainSingle();
      combinations.Single().Should().BeEquivalentTo("z", "a", "m", "b", "i", "a");
    }

    [Fact]
    public void GetCombinations_HasRepetitiveSequences_ReturnsDistinctCombinations() {
      var lines = new List<string>
      {
        "aaaa",
        "a",
        "aa",
        "aaa"
      };

      var combinations = _subject.GetCombinations(lines, 4).ToList();

      var expectedCombinations = new object[]
      {
        new[] {"a", "aaa"},
        new[] {"a", "a", "aa"},
        new[] {"a", "a", "a", "a"},
        new[] {"a", "aa", "a"},
        new[] {"aa", "aa"},
        new[] {"aa", "a", "a"},
        new[] {"aaa", "a"}
      };
      combinations.Should().BeEquivalentTo(expectedCombinations);
    }

    [Fact]
    public async Task GetCombinations_UseProvidedInput_ReturnsValidCombinations() {
      var lines = await File.ReadAllLinesAsync("combinations.txt");
      const int wordLength = 6;

      var combinations = _subject.GetCombinations(lines, wordLength).ToList();

      combinations.Should().NotBeEmpty();
      combinations.Should().OnlyHaveUniqueItems(x => x.Aggregate((a, b) => $"{a},{b}"));
      foreach (var word in combinations.Select(x => x.Aggregate((a, b) => a + b))) {
        word.Length.Should().Be(wordLength);
      }
    }

    [Fact]
    public void GetCombinations_NegativeWordLengthPassed_ThrowsArgumentException() {
      Assert.Throws<ArgumentException>(() => _subject.GetCombinations(new List<string>(), -42).ToList());
    }

    [Fact]
    public void GetCombinations_CollectionIsNull_ThrowsArgumentNullException() {
      Assert.Throws<ArgumentNullException>(() => _subject.GetCombinations(null, 42).ToList());
    }
  }
}
