using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using StringUtilities;

namespace SixLetterWordExercise {
  class Program {
    static async Task Main(string[] args) {
      var lines = await File.ReadAllLinesAsync("input.txt");

      var combinations = new WordCombinator().GetCombinations(lines, 6);

      using (var outputStreamWriter = File.CreateText("output.txt")) {
        foreach (var combination in combinations.Select(x => x.ToArray())) {
          var combinationOutput = $"{combination.Aggregate((a, b) => $"{a}+{b}")}={combination.Aggregate((a, b) => a + b)}";
          
          Console.WriteLine(combinationOutput);
          outputStreamWriter.WriteLine(combinationOutput);
        }
      }
    }
  }
}
