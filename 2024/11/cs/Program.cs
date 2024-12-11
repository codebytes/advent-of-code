//https://adventofcode.com/2024/day/11
var input = await File.ReadAllTextAsync("../input.txt");

var stones = input.Trim().Split(' ').Select(long.Parse);

int blinks = 75;

var stoneCounts = stones.GroupBy(s => s).ToDictionary(g => g.Key, g => (long)g.Count());

long totalStones = 0;
for (int i = 0; i < blinks; i++)
{
    stoneCounts = stoneCounts
        .SelectMany(kv => TransformStone(kv.Key, kv.Value))
        .GroupBy(t => t.Item1)
        .ToDictionary(g => g.Key, g => g.Sum(t => t.Item2));

    if (i + 1 == 25 || i + 1 == 75)
    {
        totalStones = stoneCounts.Values.Sum();
        Console.WriteLine($"After {i + 1} cycles: {totalStones}");
    }
}

IEnumerable<(long, long)> TransformStone(long stone, long count) =>
    stone switch
    {
        0 => new[] { (1L, count) },
        _ when stone.ToString().Length % 2 == 0 => SplitStone(stone).Select(s => (s, count)),
        _ => new[] { (stone * 2024, count) }
    };

IEnumerable<long> SplitStone(long stone)
{
    var stoneStr = stone.ToString();
    int halfLength = stoneStr.Length / 2;
    var leftStr = stoneStr[..halfLength].TrimStart('0');
    var rightStr = stoneStr[halfLength..].TrimStart('0');

    long left = long.TryParse(leftStr, out var l) ? l : 0;
    long right = long.TryParse(rightStr, out var r) ? r : 0;

    return new[] { left, right };
}
