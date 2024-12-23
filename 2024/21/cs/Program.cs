using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

//https://adventofcode.com/2024/day/21
//var input = await File.ReadAllLinesAsync("../sample.txt");
var input = await File.ReadAllLinesAsync("../input.txt");

var part1Result = input
        .Select(line => Solve(line, 2))
        .Sum();

Console.WriteLine($"Part 1: {part1Result}");
var part2Result = input
        .Select(line => Solve(line, 25))
        .Sum();
Console.WriteLine($"Part 2: {part2Result}");

long Solve(string line, int depth)
{
    var doorKeypad = "789\n456\n123\n 0A"
        .Split("\n")
        .SelectMany((row, y) => row.Select((ch, x) => new { ch, x, y }))
        .ToDictionary(o => new Pos(o.x, -o.y), o => o.ch);

    var remoteKeypad = " ^A\n<v>"
        .Split("\n")
        .SelectMany((row, y) => row.Select((ch, x) => new { ch, x, y }))
        .ToDictionary(o => new Pos(o.x, -o.y), o => o.ch);

    var keypads = Enumerable.Repeat(remoteKeypad, depth).Prepend(doorKeypad).ToArray();
    var cache = new ConcurrentDictionary<(char currentKey, char nextKey, int depth), long>();
    var num = int.Parse(line[..^1]);
    return num * EncodeKeys(line, keypads, cache);
}

long EncodeKeys(string keys, Dictionary<Pos, char>[] keypads, ConcurrentDictionary<(char currentKey, char nextKey, int depth), long> cache)
{
    if (keypads.Length == 0)
    {
        return keys.Length;
    }
    else
    {
        var currentKey = 'A';
        var length = 0L;
        foreach (var nextKey in keys)
        {
            length += EncodeKey(currentKey, nextKey, keypads, cache);
            currentKey = nextKey;
        }
        Debug.Assert(currentKey == 'A');
        return length;
    }
}

long EncodeKey(char currentKey, char nextKey, Dictionary<Pos, char>[] keypads, ConcurrentDictionary<(char, char, int), long> cache) =>
    cache.GetOrAdd((currentKey, nextKey, keypads.Length), _ =>
    {
        var keypad = keypads[0];
        var currentPos = keypad.Single(kvp => kvp.Value == currentKey).Key;
        var nextPos = keypad.Single(kvp => kvp.Value == nextKey).Key;

        var (dx, dy) = (nextPos.x - currentPos.x, nextPos.y - currentPos.y);
        var horiz = dx switch { > 0 => new string('>', dx), < 0 => new string('<', -dx), _ => "" };
        var vert = dy switch { > 0 => new string('^', dy), < 0 => new string('v', -dy), _ => "" };

        return ComputeMovementCost(horiz, vert, currentPos, nextPos, keypads, cache);
    }
);

long ComputeMovementCost(string horiz, string vert, Pos currentPos, Pos nextPos, Dictionary<Pos, char>[] keypads, ConcurrentDictionary<(char, char, int), long> cache)
{
    var keypad = keypads[0];
    var cost = long.MaxValue;
    if (keypad[new Pos(currentPos.x, nextPos.y)] != ' ')
    {
        cost = Math.Min(cost, EncodeKeys($"{vert}{horiz}A", keypads[1..], cache));
    }
    if (keypad[new Pos(nextPos.x, currentPos.y)] != ' ')
    {
        cost = Math.Min(cost, EncodeKeys($"{horiz}{vert}A", keypads[1..], cache));
    }
    return cost;
}

record struct Pos(int x, int y);
