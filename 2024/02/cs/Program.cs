//https://adventofcode.com/2024/day/2
// var input = await File.ReadAllLinesAsync("../sample.txt");
var input = await File.ReadAllLinesAsync("../input.txt");

var reports = input.Select(x =>
                    x.Split(' ')
                    .Select(int.Parse)
                    .ToArray())
                .ToArray();

bool IsSafe(int[] levels)
{
    var increasing = levels[1] > levels[0];
    return levels.Skip(1)
                 .Select((level, i) => level - levels[i])
                 .All(diff =>
                    Math.Abs(diff) >= 1 &&
                    Math.Abs(diff) <= 3 &&
                    (increasing
                        ? diff >= 0
                        : diff <= 0));
}

var safeReportsPart1 = reports.Count(IsSafe);
Console.WriteLine($"Part 1: {safeReportsPart1}");

var safeReportsPart2 = reports.Count(levels =>
    Enumerable.Range(0, levels.Length)
              .Select(i => levels.Where((_, index) => index != i).ToArray())
              .Any(modifiedLevels => IsSafe(modifiedLevels))
);
Console.WriteLine($"Part 2: {safeReportsPart2}");