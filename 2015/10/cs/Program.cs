//https://adventofcode.com/2015/day/10
using System.Text;

var input = await File.ReadAllLinesAsync("../input.txt");

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
