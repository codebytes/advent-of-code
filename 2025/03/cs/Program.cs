//https://adventofcode.com/2025/day/3
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var banks = input
	.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
	.Select(line => line.Trim())
	.Where(line => line.Length > 0)
	.ToArray();

const int Part1Digits = 2;
const int Part2Digits = 12;

Func<ReadOnlySpan<char>, int, long> computeMaxJoltage = (bank, digitsNeeded) =>
{
	Span<char> selection = digitsNeeded <= 32 ? stackalloc char[digitsNeeded] : new char[digitsNeeded];
	int selected = 0;
	int toRemove = bank.Length - digitsNeeded;

	for (int i = 0; i < bank.Length; i++)
	{
		char digitChar = bank[i];
	
		while (selected > 0 && selection[selected - 1] < digitChar && toRemove > 0)
		{
			selected--;
			toRemove--;
		}

		if (selected < digitsNeeded)
		{
			selection[selected++] = digitChar;
		}
		else if (toRemove > 0)
		{
			toRemove--;
		}
	}

	long value = 0;
	for (int i = 0; i < digitsNeeded; i++)
	{
		value = value * 10 + (selection[i] - '0');
	}

	return value;
};

var (part1Total, part2Total) = banks.Aggregate((Part1: 0L, Part2: 0L), (acc, bank) =>
{
	var span = bank.AsSpan();
	return (
		acc.Part1 + computeMaxJoltage(span, Part1Digits),
		acc.Part2 + computeMaxJoltage(span, Part2Digits));
});

Console.WriteLine($"Part 1: {part1Total}");
Console.WriteLine($"Part 2: {part2Total}");
