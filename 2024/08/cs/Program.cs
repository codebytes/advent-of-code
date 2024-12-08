//https://adventofcode.com/2024/day/8
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
var antennas = lines
    .SelectMany((line, y) => line.Select((c, x) => (x, y, c)))
    .Where(t => char.IsLetterOrDigit(t.c))
    .Select(t => (t.x, t.y, t.c))
    .ToList();

var antinodes = new HashSet<(int x, int y)>();
var antinodesPart2 = new HashSet<(int x, int y)>();

Func<int, int, int> findGcd = (a, b) =>
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return a;
};

Func<int, int, bool> isValidCoordinate = (x, y) => 
    x >= 0 && x < lines[0].Length 
    && y >= 0 && y < lines.Length;

IEnumerable<(int x, int y)> generateAntinodesAlongLine(int x, int y, int dx, int dy)
{
    while (isValidCoordinate(x, y))
    {
        yield return (x, y);
        x += dx;
        y += dy;
    }
}

foreach (var (x1, y1, freq1) in antennas)
{
    foreach (var (x2, y2, freq2) in antennas)
    {
        if (freq1 == freq2 && (x1 != x2 || y1 != y2))
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            if (dx % 2 == 0 && dy % 2 == 0)
            {
                int mx = x1 + dx / 2;
                int my = y1 + dy / 2;
                antinodes.Add((mx, my));
            }

            if (isValidCoordinate(x1 - dx, y1 - dy))
                antinodes.Add((x1 - dx, y1 - dy));
            if (isValidCoordinate(x2 + dx, y2 + dy))
                antinodes.Add((x2 + dx, y2 + dy));

            int gcd = findGcd(Math.Abs(dx), Math.Abs(dy));
            dx /= gcd;
            dy /= gcd;

            foreach (var antinode in generateAntinodesAlongLine(x1, y1, dx, dy))
                antinodesPart2.Add(antinode);
            foreach (var antinode in generateAntinodesAlongLine(x1, y1, -dx, -dy))
                antinodesPart2.Add(antinode);
        }
    }
}

Console.WriteLine($"Number of unique antinode locations (Part 1): {antinodes.Count}");
Console.WriteLine($"Number of unique antinode locations (Part 2): {antinodesPart2.Count}");

