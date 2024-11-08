//https://adventofcode.com/2015/day/7
using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("../input.txt");

string quote = "\"";
string backslash = "\\";
string escapedQuote = "\\\"";
string escapedBackslash = "\\\\";

Func<string, string> Decode = s => hexCodeRegex().Replace(s[1..^1]
         .Replace(escapedQuote, quote)
         .Replace(escapedBackslash, "."), ".");

Func<string, string> Encode = s => $"{quote}{s.Replace(backslash, escapedBackslash).Replace(quote, escapedQuote)}{quote}";

var results = input
    .Select(i => new { Encoded = i, Unencoded = Decode(i) })
    .Sum(s => s.Encoded.Length - s.Unencoded.Length);
Console.WriteLine(results);

var results2 = input
    .Select(i => new { Unencoded = i, Encoded = Encode(i) })
    .Sum(s => s.Encoded.Length - s.Unencoded.Length);
Console.WriteLine(results2);

internal partial class Program
{
    [GeneratedRegex(@"\\x[0-9a-fA-F]{2}")]
    private static partial Regex hexCodeRegex();
}