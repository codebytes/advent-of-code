//https://adventofcode.com/2024/day/14
using System.Text.RegularExpressions;

var input = await File.ReadAllTextAsync("../input.txt");
//var input = await File.ReadAllTextAsync("../sample.txt");

Robot[] ParseInput(string input) => input.Split('\n')
    .Select(line => {
        var matches = robotRegex().Match(line);
        return new Robot(
            int.Parse(matches.Groups[2].Value),
            int.Parse(matches.Groups[1].Value),
            int.Parse(matches.Groups[4].Value),
            int.Parse(matches.Groups[3].Value)
        );
    })
    .ToArray();

int mod(int value, int mod)
{
    return (value % mod + mod) % mod;
}

Robot MoveRobot(Robot robot, int maxY, int maxX)
{
    var nextY = mod(robot.Y + robot.Dy, maxY);
    var nextX = mod(robot.X + robot.Dx, maxX);
    return robot with { Y = nextY, X = nextX };
}

void PrintGrid(Robot[] robots, int maxY, int maxX)
{
    for (int y = 0; y < maxY; y++)
    {
        var line = "";
        for (int x = 0; x < maxX; x++)
        {
            var robotCounts = robots.Count(robot => robot.Y == y && robot.X == x);
            line += robotCounts > 0 ? robotCounts.ToString() : ".";
        }
        Console.WriteLine(line);
    }
}

int CalculateSafetyCount(Robot[] robots, int maxY, int maxX)
{
     var midY = maxY / 2;
    var midX = maxX / 2;
    var q1Count = robots.Count(robot => robot.Y < midY && robot.X < midX);
    var q2Count = robots.Count(robot => robot.Y < midY && robot.X > midX);
    var q3Count = robots.Count(robot => robot.Y > midY && robot.X < midX);
    var q4Count = robots.Count(robot => robot.Y > midY && robot.X > midX);

    return q1Count * q2Count * q3Count * q4Count;
}

int CalculatePart1(Robot[] robots)
{
    var maxY = robots.Length < 13 ? 7 : 103;
    var maxX = robots.Length < 13 ? 11 : 101;
    
    for (int tick = 1; tick <= 100; tick++)
    {
        for (int i = 0; i < robots.Length; i++)
        {
            robots[i] = MoveRobot(robots[i], maxY, maxX);
        }
    }

    return CalculateSafetyCount(robots, maxY, maxX);
}

int CalculatePart2(Robot[] robots)
{
    var maxY = robots.Length < 13 ? 7 : 103;
    var maxX = robots.Length < 13 ? 11 : 101;
  
    int minSafety = int.MaxValue;
    int minSeconds = 0;

    for (int second = 1; second <= 100000; second++)
    {
        for (int i = 0; i < robots.Length; i++)
        {
            robots[i] = MoveRobot(robots[i], maxY, maxX);
        }

        var safety = CalculateSafetyCount(robots, maxY, maxX);
        if (safety < minSafety)
        {
            minSafety = safety;
            minSeconds = second;
        }
    }

    return minSeconds;
}

var robots = ParseInput(input);
Console.WriteLine($"Part 1: {CalculatePart1(robots)}");

robots = ParseInput(input);
Console.WriteLine($"Part 2: {CalculatePart2(robots)}");

record Robot(int Y, int X, int Dy, int Dx);

internal partial class Program
{
    [GeneratedRegex(@"p=(\d+),(\d+) v=(-?\d+),(-?\d+)")]
    private static partial Regex robotRegex();
}