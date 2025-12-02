//https://adventofcode.com/2015/day/20
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var target = int.Parse(input.Trim());

var part1 = FindHouse(target, presentMultiplier: 10, maxVisitsPerElf: null);
Console.WriteLine($"Part 1: {part1}");

var part2 = FindHouse(target, presentMultiplier: 11, maxVisitsPerElf: 50);
Console.WriteLine($"Part 2: {part2}");

static int FindHouse(int target, int presentMultiplier, int? maxVisitsPerElf)
{
	for (var limit = Math.Max(1, target / presentMultiplier); ; limit *= 2)
	{
		var houses = new int[limit + 1];

		for (var elf = 1; elf <= limit; elf++)
		{
			var maxHouse = maxVisitsPerElf.HasValue
				? Math.Min(limit, elf * maxVisitsPerElf.Value)
				: limit;

			for (var house = elf; house <= maxHouse; house += elf)
			{
				houses[house] += elf * presentMultiplier;
			}
		}

		for (var house = 1; house <= limit; house++)
		{
			if (houses[house] >= target)
			{
				return house;
			}
		}
	}
}

