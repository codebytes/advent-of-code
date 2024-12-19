//https://adventofcode.com/2024/day/19
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var sections = input.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
var patterns = sections[0].Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
var designs = sections[1].Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

var memo = new Dictionary<string, long>();

bool CanMakeDesign(string design, string[] patterns)
{
    return CountWaysToMakeDesign(design, patterns) > 0;
}

long CountWaysToMakeDesign(string design, string[] patterns)
{
    if (design == "") return 1;
    if (memo.ContainsKey(design)) return memo[design];

    long count = 0;
    foreach (var pattern in patterns)
    {
        if (design.StartsWith(pattern))
        {
            count += CountWaysToMakeDesign(design.Substring(pattern.Length), patterns);
        }
    }

    memo[design] = count;
    return count;
}

int possibleDesignsCount = designs.Count(design => CanMakeDesign(design, patterns));
Console.WriteLine($"Number of possible designs: {possibleDesignsCount}");

long totalWays = designs.Sum(design => CountWaysToMakeDesign(design, patterns));
Console.WriteLine($"Total number of ways to make all designs: {totalWays}");
