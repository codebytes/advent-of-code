//https://adventofcode.com/2022/day/3
var input = await File.ReadAllLinesAsync("../input.txt");

//convert each line of input to a list of ints with a=1 and Z=52
var inputList = input.Select(x => x.Select(y =>
{
	return y switch
	{
		>= 'a' and <= 'z' => y - 96,
		>= 'A' and <= 'Z' => y - 38,
		_ => 0
	};}
	).ToList()).ToList();

var items = inputList
	.Select(x=>x.Chunk(x.Count/2))
	.SelectMany(x=>x.First().Intersect(x.Last()));

//part 1
Console.WriteLine($"Sum of prioritized items: {items.Sum()}");

var badges = inputList
	.Chunk(3)
	.SelectMany(x=>x[0].Intersect(x[1]).Intersect(x[2]));
Console.WriteLine($"Sum of prioritized badges: {badges.Sum()}");

