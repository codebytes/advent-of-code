//https://adventofcode.com/2024/day/13
using System.Text.RegularExpressions;

//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var buttonPrizes = input.Split("\n\n")
                        .Select(Button.Parse)
                        .ToList();

var minTokens = buttonPrizes.Select(bp => CalculateMinTokens(bp))
                            .Where(cost => cost.HasValue)
                            .Select(cost => cost ?? 0)
                            .Sum();

Console.WriteLine($"Part 1: Minimum tokens required: {minTokens}");

const long offset = 10000000000000;

var adjustedButtonPrizes = buttonPrizes.Select(bp => new Button
{
    AX = bp.AX,
    AY = bp.AY,
    BX = bp.BX,
    BY = bp.BY,
    PX = bp.PX + offset,
    PY = bp.PY + offset
}).ToList();

var minTokensPart2 = adjustedButtonPrizes.Select(bp => CalculateMinTokens(bp))
                                         .Where(cost => cost.HasValue)
                                         .Select(cost => cost ?? 0)
                                         .Sum();

Console.WriteLine($"Part 2: Minimum tokens required after adjustment: {minTokensPart2}");

long? CalculateMinTokens(Button bp)
{
    // Coefficients for the equations
    long aX = bp.AX;
    long bX = bp.BX;
    long pX = bp.PX;

    long aY = bp.AY;
    long bY = bp.BY;
    long pY = bp.PY;

    long D  = aX * bY - aY * bX;
    long Dx = pX * bY - pY * bX;
    long Dy = aX * pY - aY * pX;

    if (D == 0)
    {
        if (Dx != 0 || Dy != 0)
            return null; // No solution

        // Infinite solutions; find minimal non-negative integers
        long minCost = long.MaxValue;
        for (long a = 0; a <= 100000; a++)
        {
            if (aX * a > pX)
                break;
            if ((pX - aX * a) % bX != 0)
                continue;
            long b = (pX - aX * a) / bX;
            if (b < 0)
                continue;
            if (aY * a + bY * b == pY)
            {
                long cost = a * 3 + b;
                if (cost < minCost)
                    minCost = cost;
            }
        }
        return minCost == long.MaxValue ? null : minCost;
    }
    else
    {
        if (Dx % D != 0 || Dy % D != 0)
            return null; // No integer solution

        long a = Dx / D;
        long b = Dy / D;

        if (a < 0 || b < 0)
            return null;

        long cost = a * 3 + b;
        return cost;
    }
}

long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);

(long x, long y) GCDExtended(long a, long b)
    {
        if (b == 0)
            return (1, 0);

        var (x1, y1) = GCDExtended(b, a % b);
        return (y1, x1 - (a / b) * y1);
    }

class Button
{
    public long AX { get; set; }
    public long AY { get; set; }
    public long BX { get; set; }
    public long BY { get; set; }
    public long PX { get; set; }
    public long PY { get; set; }

    public static Button Parse(string input)
    {
        var regex = new Regex(@"Button A: X\+(?<ax>\d+), Y\+(?<ay>\d+)\s+Button B: X\+(?<bx>\d+), Y\+(?<by>\d+)\s+Prize: X=(?<px>\d+), Y=(?<py>\d+)");
        var match = regex.Match(input);
        if (!match.Success) throw new FormatException("Input string is not in the correct format.");

        return new Button
        {
            AX = long.Parse(match.Groups["ax"].Value),
            AY = long.Parse(match.Groups["ay"].Value),
            BX = long.Parse(match.Groups["bx"].Value),
            BY = long.Parse(match.Groups["by"].Value),
            PX = long.Parse(match.Groups["px"].Value),
            PY = long.Parse(match.Groups["py"].Value)
        };
    }
}
