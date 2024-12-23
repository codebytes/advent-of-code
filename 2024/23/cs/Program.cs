using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

//https://adventofcode.com/2024/day/23
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var graph = GetGraph(input);

Console.WriteLine(Part1(graph));
Console.WriteLine(Part2(graph));

string Part1(Dictionary<string, HashSet<string>> graph)
{
    const string Identifier = "t";
    var seen = graph
        .Where(kvp => kvp.Key.StartsWith(Identifier))
        .SelectMany(kvp => GenerateAllPairs(kvp.Value.ToList())
            .Where(pair => graph[pair.Item1].Contains(pair.Item2))
            .Select(pair => string.Join(",", new[] { kvp.Key, pair.Item1, pair.Item2 }.Order())))
        .ToHashSet();

    return seen.Count.ToString();
}

string Part2(Dictionary<string, HashSet<string>> graph)
{
    var (best, maxLength) = graph
        .Select(kvp =>
        {
            var network = new HashSet<string> { kvp.Key };
            foreach (var pc1 in kvp.Value.OrderByDescending(pcOther => kvp.Value.Intersect(graph[pcOther]).Count()))
            {
                if (network.All(networkPc => graph[pc1].Contains(networkPc)))
                {
                    network.Add(pc1);
                }
            }
            return (network: string.Join(",", network.Order()), count: network.Count);
        })
        .OrderByDescending(result => result.count)
        .First();

    return best;
}

Dictionary<string, HashSet<string>> GetGraph(string input) =>
    input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
         .SelectMany(pair => pair.Split("-") switch { var split => new[] { (split[0], split[1]), (split[1], split[0]) } })
         .GroupBy(pair => pair.Item1)
         .ToDictionary(g => g.Key, g => g.Select(pair => pair.Item2).ToHashSet());

IEnumerable<(T, T)> GenerateAllPairs<T>(List<T> list)
{
    for (int i = 0; i < list.Count; i++)
    {
        for (int j = i + 1; j < list.Count; j++)
        {
            yield return (list[i], list[j]);
        }
    }
}

