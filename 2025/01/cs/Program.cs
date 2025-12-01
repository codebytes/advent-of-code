// https://adventofcode.com/2025/day/1
// var inputPath = "../sample.txt";
var inputPath = "../input.txt";

var instructions = await File.ReadAllLinesAsync(inputPath);

const int dialSize = 100;
const int startPosition = 50;

var parsedInstructions = instructions
    .Select(line => line?.Trim())
    .Where(line => !string.IsNullOrWhiteSpace(line))
    .Select(line =>
    {
        var directionSign = line![0] == 'L' ? -1 : 1;

        if (!long.TryParse(line.AsSpan(1), out var distance))
        {
            throw new InvalidDataException($"Invalid distance in instruction '{line}'");
        }

        var normalizedDistance = (int)(distance % dialSize);

        return (DirectionSign: directionSign, Distance: distance, NormalizedDistance: normalizedDistance);
    })
    .ToArray();

var part1Password = Part1(parsedInstructions, startPosition, dialSize);
Console.WriteLine($"Part 1 Password: {part1Password}");

var part2Password = Part2(parsedInstructions, startPosition, dialSize);
Console.WriteLine($"Part 2 Password: {part2Password}");

static int Part1((int DirectionSign, long Distance, int NormalizedDistance)[] instructions, int startPosition, int dialSize)
{
    var position = startPosition;
    var zeroHits = 0;

    foreach (var instruction in instructions)
    {
        var nextPosition = (position + instruction.DirectionSign * instruction.NormalizedDistance) % dialSize;
        position = nextPosition < 0 ? nextPosition + dialSize : nextPosition;

        if (position == 0)
        {
            zeroHits++;
        }
    }

    return zeroHits;
}

static long Part2((int DirectionSign, long Distance, int NormalizedDistance)[] instructions, int startPosition, int dialSize)
{
    long absolutePosition = startPosition;
    long zeroClicks = 0;

    foreach (var instruction in instructions)
    {
        var newAbsolutePosition = absolutePosition + instruction.DirectionSign * instruction.Distance;
        zeroClicks += CountZeroCrossings(absolutePosition, newAbsolutePosition, dialSize);
        absolutePosition = newAbsolutePosition;
    }

    return zeroClicks;
}

static long CountZeroCrossings(long start, long end, int dialSize)
{
    if (start == end)
    {
        return 0;
    }

    return end > start
        ? FloorDiv(end, dialSize) - FloorDiv(start, dialSize)
        : FloorDiv(start - 1, dialSize) - FloorDiv(end - 1, dialSize);
}

static long FloorDiv(long value, int dialSize)
{
    var quotient = System.Math.DivRem(value, dialSize, out var remainder);
    return remainder < 0 ? quotient - 1 : quotient;
}
