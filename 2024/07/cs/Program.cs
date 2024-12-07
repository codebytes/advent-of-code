//https://adventofcode.com/2024/day/7
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

bool EvaluateEquation(long target, long[] numbers, int index, IEnumerable<long> currentResults, bool allowConcatenation)
{
    if (index == numbers.Length)
        return currentResults.Contains(target);

    var nextResults = currentResults.SelectMany(result => 
    {
        var results = new List<long> { result + numbers[index], result * numbers[index] };
        if (allowConcatenation)
        {
            results.Add(long.Parse(result.ToString() + numbers[index].ToString()));
        }
        return results;
    });

    return EvaluateEquation(target, numbers, index + 1, nextResults, allowConcatenation);
}

bool CanFormEquation(long target, long[] numbers, bool allowConcatenation) =>
    EvaluateEquation(target, numbers, 1, new List<long> { numbers[0] }, allowConcatenation);

long CalculateTotalCalibrationResult(string input, bool allowConcatenation) =>
    input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
         .Select(line => line.Split(':'))
         .Select(parts => new { Target = long.Parse(parts[0]), Numbers = parts[1].Trim().Split(' ').Select(long.Parse).ToArray() })
         .Where(data => CanFormEquation(data.Target, data.Numbers, allowConcatenation))
         .Sum(data => data.Target);

var part1Result = CalculateTotalCalibrationResult(input, false);
Console.WriteLine($"Part 1 Result: {part1Result}");

var part2Result = CalculateTotalCalibrationResult(input, true);
Console.WriteLine($"Part 2 Result: {part2Result}");
