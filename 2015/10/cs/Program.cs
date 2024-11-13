//https://adventofcode.com/2015/day/10
using System.Text;
using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("../input.txt");

// Today, the Elves are playing a game called look-and-say. They take turns making sequences by reading aloud the previous sequence and using that reading as the next sequence. For example, 211 is read as "one two, two ones", which becomes 1221 (1 2, 2 1s).

// Look-and-say sequences are generated iteratively, using the previous value as input for the next step. For each step, take the previous value, and replace each run of digits (like 111) with the number of digits (3) followed by the digit itself (1).

// For example:

// 1 becomes 11 (1 copy of digit 1).
// 11 becomes 21 (2 copies of digit 1).
// 21 becomes 1211 (one 2 followed by one 1).
// 1211 becomes 111221 (one 1, one 2, and two 1s).
// 111221 becomes 312211 (three 1s, two 2s, and one 1).

var start = input.First();
var times = 50;

var results = new Dictionary<int, int>();
for (var i = 0; i < times; i++)
{
    start = LookAndSay(start);
    results.Add(i, start.Length);
}

Console.WriteLine($"40: {results[39]}");
Console.WriteLine($"50: {results[49]}");

string LookAndSay(string input)
{
    var result = new StringBuilder();

    int count = 1;
    for (int i = 1; i < input.Length; i++)
    {
        if (input[i] == input[i - 1])
        {
            count++;
        }
        else
        {
            result.Append(count);
            result.Append(input[i - 1]);
            count = 1;
        }
    }

    result.Append(count);
    result.Append(input[input.Length - 1]);

    return result.ToString();
}
