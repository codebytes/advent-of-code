using System.Text.RegularExpressions;

//https://adventofcode.com/2015/day/13
var input = await File.ReadAllTextAsync("../input.txt");

var happinessMap = ParseInput(input);

Dictionary<(string, string), int> ParseInput(string input)
{
    var regex = new Regex(@"(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+)\.");
    return regex.Matches(input)
        .ToDictionary(
            match => (match.Groups[1].Value, match.Groups[4].Value),
            match => int.Parse(match.Groups[3].Value) * (match.Groups[2].Value == "gain" ? 1 : -1)
        );
}

int CalculateHappiness(string[] seating) =>
    seating.Select((person, index) => 
        happinessMap[(person, seating[(index + 1) % seating.Length])] + 
        happinessMap[(person, seating[(index - 1 + seating.Length) % seating.Length])])
    .Sum();

int CalculateMaxHappiness(string[] people) =>
    people.Permutations().Select(CalculateHappiness).Max();


var people = happinessMap.Keys.Select(pair => pair.Item1).Distinct().ToArray();
Console.WriteLine($"Part 1: {CalculateMaxHappiness(people)}");


people = people.Append("Me").ToArray();
foreach (var person in people)
{
    happinessMap[("Me", person)] = 0;
    happinessMap[(person, "Me")] = 0;
}
Console.WriteLine($"Part 2: {CalculateMaxHappiness(people)}");

public static class Extensions
{
    public static IEnumerable<T[]> Permutations<T>(this IEnumerable<T>? source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (source.Count() == 1)
        {
            yield return source.ToArray();
        }
        else
        {
            foreach (var element in source)
            {
                foreach (var permutation in source!.Where(e => !e!.Equals(element)).Permutations())
                {
                    yield return new[] { element }.Concat(permutation).ToArray();
                }
            }
        }
    }
}