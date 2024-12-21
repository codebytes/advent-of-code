//https://adventofcode.com/2024/day/20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

var input = await File.ReadAllLinesAsync("../input.txt");

Console.WriteLine("Part 1: " + RunPart1(input));
Console.WriteLine("Part 2: " + RunPart2(input));

int RunPart1(string[] input) => Race(input, 2, 100);

int RunPart2(string[] input) => Race(input, 20, 100);

int Race(string[] input, int maxCheatDuration, int minTimeSaved)
{
    var (walls, start) = ParseInput(input);
    var dijkstraAdjacencyMatrix = DijkstraAdjacency(walls, start);
    var total = Cheat(dijkstraAdjacencyMatrix, maxCheatDuration, minTimeSaved);
    return total;
}

(HashSet<(int x, int y)> walls, (int x, int y) start) ParseInput(string[] input)
{
    var walls = new HashSet<(int x, int y)>();
    var start = (-1, -1);

    for (int y = 0; y < input.Length; ++y)
    {
        for (int x = 0; x < input[y].Length; ++x)
        {
            switch (input[y][x])
            {
                case 'S':
                    start = (x, y);
                    break;
                case '#':
                    walls.Add((x, y));
                    break;
            }
        }
    }

    // Add extra walls around border to make algorithm simpler
    walls.UnionWith(Enumerable.Range(-1, input.Length + 1)
        .SelectMany(x1 => new[] { (-1, x1), (input[0].Length, x1) }));

    walls.UnionWith(Enumerable.Range(-1, input[0].Length + 1)
        .SelectMany(y1 => new[] { (y1, -1), (y1, input.Length) }));

    return (walls, start);
}

int ManhattanDistance((int x, int y) a, (int x, int y) b) => 
    Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

int Cheat(Dictionary<(int x, int y), int> dist, int maxCheatDuration, int minTimeSaved) => dist
        .SelectMany(p1 => dist.Where(p2 => p2.Value > p1.Value)
        .Select(p2 => (p1.Key, p2.Key, Saved: p2.Value - p1.Value - ManhattanDistance(p1.Key, p2.Key))))
        .Count(t => t.Saved >= minTimeSaved && ManhattanDistance(t.Item1, t.Item2) <= maxCheatDuration);

IEnumerable<(int x, int y)> Neighbors((int x, int y) p) => 
    new[] 
    {
        (p.x, p.y - 1),
        (p.x + 1, p.y),
        (p.x, p.y + 1),
        (p.x - 1, p.y)
    };

Dictionary<(int x, int y), int> DijkstraAdjacency(HashSet<(int x, int y)> walls, (int x, int y) start)
{
    var dist = new Dictionary<(int x, int y), int> { { start, 0 } };
    var queue = new PriorityQueue<(int x, int y), int>();
    queue.Enqueue(start, 0);

    while (queue.TryDequeue(out var cur, out _))
    {
        foreach (var next in Neighbors(cur).Where(p => !walls.Contains(p)))
        {
            var newDist = dist[cur] + 1;
            if (newDist < dist.GetValueOrDefault(next, int.MaxValue))
            {
                dist[next] = newDist;
                queue.Enqueue(next, newDist);
            }
        }
    }

    return dist;
}

