//https://adventofcode.com/2024/day/25
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var lines = input.Split('\n');
var (locks, keys) = lines
    .Select((line, index) => (line, index))
    .GroupBy(x => x.index / 8)
    .Select(g => g.Select(x => x.line).ToArray())
    .Select(current => current
        .SelectMany((line, r) => line
            .Select((ch, c) => (ch == '#', r, c)))
        .ToArray())
    .Aggregate((locks: new List<uint>(), keys: new List<uint>()), (acc, current) =>
    {
        if (current.First().Item1)
        {
            acc.locks.Add(Enumerable.Range(0, 5).Aggregate(0u, (acc, j) =>
                (acc << 4) | (uint)(Enumerable.Range(1, 6).First(k => !current.First(x => x.r == k && x.c == j).Item1))
            ));
        }
        else
        {
            acc.keys.Add(Enumerable.Range(0, 5).Aggregate(0u, (acc, j) =>
                (acc << 4) | (uint)(Enumerable.Range(0, 6).First(k => !current.First(x => x.r == 5 - k && x.c == j).Item1) + 1)
            ));
        }
        return acc;
    });

int total = keys.Sum(key =>
    locks.Count(lockShape => (key + lockShape & 0x88888) == 0)
);

Console.WriteLine(total);
