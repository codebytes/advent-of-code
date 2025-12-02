//https://adventofcode.com/2025/day/2
var input = await File.ReadAllTextAsync("../sample.txt");
// var input = await File.ReadAllTextAsync("../input.txt");

var ranges = input
	.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
	.Select(token => token.Split('-', 2, StringSplitOptions.TrimEntries))
	.Select(parts => (
		Start: long.Parse(parts[0]),
		End: long.Parse(parts[1])
	))
	.ToArray();

var part1 = ranges
	.SelectMany(range => EnumerateInvalidIds(range.Start, range.End, 2, 2))
	.Sum();
Console.WriteLine($"Part 1: {part1}");

var part2 = ranges
	.SelectMany(range => EnumerateInvalidIds(range.Start, range.End, 2))
	.Sum();
Console.WriteLine($"Part 2: {part2}");


static IEnumerable<long> EnumerateInvalidIds(long start, long end, int minRepeats, int? maxRepeats = null)
{
	if (start > end)
	{
		yield break;
	}

	var minDigits = CountDigits(start);
	var maxDigits = CountDigits(end);
	var yielded = new HashSet<long>();

	for (var digits = minDigits; digits <= maxDigits; digits++)
	{
		foreach (var (baseDigits, repeats) in ValidPatterns(digits, minRepeats, maxRepeats))
		{
			var multiplier = BuildMultiplier(baseDigits, repeats);
			var prefixMin = Pow10(baseDigits - 1);
			var prefixMax = Pow10(baseDigits) - 1;

			var rangeMin = Math.Max(prefixMin, CeilDiv(start, multiplier));
			var rangeMax = Math.Min(prefixMax, end / multiplier);

			for (var prefix = rangeMin; prefix <= rangeMax; prefix++)
			{
				var candidate = prefix * multiplier;

				if (yielded.Add(candidate))
				{
					yield return candidate;
				}
			}
		}
	}
}

static IEnumerable<(int BaseDigits, int Repeats)> ValidPatterns(int digits, int minRepeats, int? maxRepeats) =>
	Enumerable
		.Range(1, digits / 2)
		.Where(baseDigits => digits % baseDigits == 0)
		.Select(baseDigits => (BaseDigits: baseDigits, Repeats: digits / baseDigits))
		.Where(pattern => pattern.Repeats >= minRepeats)
		.Where(pattern => !maxRepeats.HasValue || pattern.Repeats <= maxRepeats.Value);

static long CeilDiv(long value, long divisor)
{
	if (value <= 0)
	{
		return 0;
	}

	var quotient = Math.DivRem(value, divisor, out var remainder);
	return remainder == 0 ? quotient : quotient + 1;
}

static int CountDigits(long value)
{
	value = Math.Abs(value);

	if (value == 0)
	{
		return 1;
	}

	var digits = 0;

	while (value > 0)
	{
		digits++;
		value /= 10;
	}

	return digits;
}

static long Pow10(int exponent)
{
	if (exponent <= 0)
	{
		return 1;
	}

	var result = 1L;

	for (var i = 0; i < exponent; i++)
	{
		result *= 10;
	}

	return result;
}

static long BuildMultiplier(int baseDigits, int repeats)
{
	var factor = Pow10(baseDigits);
	return Enumerable.Range(0, repeats).Aggregate(0L, (value, _) => value * factor + 1);
}
