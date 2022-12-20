//https://adventofcode.com/2022/day/14
var input = await File.ReadAllLinesAsync("../input.txt");

var source = new Coord(500, 0);
var grid = new bool[500][];
var maxY = ParseInput(ref grid, input);
var part1Sand = Sim(ref grid, source, maxY, false);
Console.WriteLine($"Part1: {part1Sand}");

grid = new bool[500][];
maxY = ParseInput(ref grid, input);
var part2Sand = Sim(ref grid, source, maxY, true);
Console.WriteLine($"Part2: {part2Sand}");

int ParseInput(ref bool[][] grid, string[] input)
{
    var maxY = 0;

    foreach (var line in input)
    {
        Coord lastCoord = new Coord(-1, -1);
        var parts = line.Split(" -> ");

        foreach (var part in parts)
        {
            var split = part.Split(",").Select(p => int.Parse(p)).ToArray();
            var point = new Coord(split[0], split[1]);

            ExpandGrid(ref grid, point);

            grid[point.X][point.Y] = true;

            if (lastCoord.X != -1 || lastCoord.Y != -1)
            {
                if (lastCoord.Y == point.Y)
                {
                    for (int i = Math.Min(lastCoord.X, point.X); i < Math.Max(lastCoord.X, point.X); i++)
                    {
                        SetBlock(ref grid, new Coord(i, point.Y));
                    }
                }
                else
                {
                    for (int i = Math.Min(lastCoord.Y, point.Y); i < Math.Max(lastCoord.Y, point.Y); i++)
                    {
                        SetBlock(ref grid, new Coord(point.X, i));
                    }
                }
            }
            lastCoord = point;
            maxY = Math.Max(maxY, point.Y);
        }
    }
    return maxY;
}

int Sim(ref bool[][] grid, Coord source, int maxY, bool blockSource)
{
    Coord sand = source;

    int sandGenerated = 0;
    while (true)
    {
        bool aboveGround = sand.Y + 1 <= maxY + 1;

        if (aboveGround && GetBlock(ref grid, sand with { Y = sand.Y + 1 }) == false)
        {
            sand = sand with { Y = sand.Y + 1 };
        }
        else if (aboveGround && GetBlock(ref grid, sand with { X = sand.X - 1, Y = sand.Y + 1 }) == false)
        {
            sand = sand with { X = sand.X - 1, Y = sand.Y + 1 };
        }
        else if (aboveGround && GetBlock(ref grid, sand with { X = sand.X + 1, Y = sand.Y + 1 }) == false)
        {
            sand = sand with { X = sand.X + 1, Y = sand.Y + 1 };
        }
        else
        {
            if (!blockSource && sand.Y > maxY)
            {
                return sandGenerated;
            }

            if (blockSource && sand.X == source.X && sand.Y == source.Y)
            {
                return sandGenerated + 1;
            }

            SetBlock(ref grid, new Coord(sand.X, sand.Y));
            sandGenerated++;

            sand = source;
        }
    }
}

bool GetBlock(ref bool[][] grid, Coord point)
{
    ExpandGrid(ref grid, new Coord(point.X, point.Y));
    return grid[point.X]?[point.Y] ?? false;
}


void SetBlock(ref bool[][] grid, Coord point)
{
    ExpandGrid(ref grid, new Coord(point.X, point.Y));
    grid[point.X][point.Y] = true;
}

void ExpandGrid(ref bool[][] grid, Coord point)
{
    int growthSize = 8;
    if (grid.Length <= point.X)
    {
        Array.Resize(ref grid, point.X + growthSize);
    }
    if (grid[point.X] is null)
    {
        grid[point.X] = new bool[point.Y];
    }
    if (grid[point.X].Length <= point.Y)
    {
        Array.Resize(ref grid[point.X], point.Y + growthSize);
    }
}

public record Coord(int X, int Y);
