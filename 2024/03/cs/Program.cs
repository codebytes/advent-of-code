using System.Text.RegularExpressions;

//https://adventofcode.com/2024/day/2
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

int SumOfMultiplicationsPart1(string input)
{
    var matches = Regex.Matches(input, @"mul\((\d+),(\d+)\)");
    int sum = 0;
    foreach (Match match in matches)
    {
        int x = int.Parse(match.Groups[1].Value);
        int y = int.Parse(match.Groups[2].Value);
        sum += x * y;
    }
    return sum;
}

int SumOfMultiplicationsPart2(string input)
{
    var matches = Regex.Matches(input, @"(mul\((\d+),(\d+)\))|(do\(\))|(don't\(\))");
    int sum = 0;
    bool isEnabled = true;
    foreach (Match match in matches)
    {
        switch (match.Value)
        {
            case "do()":
                isEnabled = true;
                break;
            case "don't()":
                isEnabled = false;
                break;
            case { } s when isEnabled && s.StartsWith("mul("):
                int x = int.Parse(match.Groups[2].Value);
                int y = int.Parse(match.Groups[3].Value);
                sum += x * y;
                break;
        }
    }
    return sum;
}

Console.WriteLine("Part 1: " + SumOfMultiplicationsPart1(input));
Console.WriteLine("Part 2: " + SumOfMultiplicationsPart2(input));

