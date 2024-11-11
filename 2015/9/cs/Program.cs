//https://adventofcode.com/2015/day/7
using System.Text.RegularExpressions;

var input = await File.ReadAllLinesAsync("../input.txt");

var graph = new Dictionary<string, Dictionary<string, int>>();

foreach (var line in input)
{
    var match = inputRegex().Match(line);
    if (match.Success)
    {
        var city1 = match.Groups[1].Value;
        var city2 = match.Groups[2].Value;
        var distance = int.Parse(match.Groups[3].Value);

        if (!graph.ContainsKey(city1))
        {
            graph[city1] = new Dictionary<string, int>();
        }
        if (!graph.ContainsKey(city2))
        {
            graph[city2] = new Dictionary<string, int>();
        }

        graph[city1][city2] = distance;
        graph[city2][city1] = distance;
    }
}

var cities = graph.Keys.ToList();
var cache = new Dictionary<(string, string, bool), int>();
var shortestRoute = FindRoute(graph, cities, "", cache, true);
Console.WriteLine($"Shortest route distance: {shortestRoute}");

var longestRoute = FindRoute(graph, cities, "", cache, false);
Console.WriteLine($"Longest route distance: {longestRoute}");

static int FindRoute(Dictionary<string, Dictionary<string, int>> graph, List<string> cities, string currentCity, Dictionary<(string, string, bool), int> cache, bool findShortest)
    {
        if (cities.Count == 0)
        {
            return 0;
        }

        var key = (currentCity, string.Join(",", cities), findShortest);
        if (cache.ContainsKey(key))
        {
            return cache[key];
        }

        int optimalDistance = findShortest ? int.MaxValue : int.MinValue;
        foreach (var city in cities)
        {
            var remainingCities = new List<string>(cities);
            remainingCities.Remove(city);

            int distance = (currentCity == "" ? 0 : graph[currentCity][city]) + FindRoute(graph, remainingCities, city, cache, findShortest);
            if (findShortest)
            {
                optimalDistance = Math.Min(optimalDistance, distance);
            }
            else
            {
                optimalDistance = Math.Max(optimalDistance, distance);
            }
        }

        cache[key] = optimalDistance;
        return optimalDistance;
    }

internal partial class Program
{
    [GeneratedRegex(@"(\w+) to (\w+) = (\d+)")]
    private static partial Regex inputRegex();
}