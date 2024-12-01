using System.Text.RegularExpressions;
//https://adventofcode.com/2015/day/2
var input = await File.ReadAllLinesAsync("../input.txt");

Regex dimensions = new Regex(@"(?<length>\d+)x(?<width>\d+)x(?<height>\d+)", RegexOptions.Compiled);

var paperNeeds = input
	.Select(x=>dimensions.Match(x))
	.Select(x => new { l = int.Parse(x.Groups["length"].Value), w = int.Parse(x.Groups["width"].Value), h = int.Parse(x.Groups["height"].Value) })
	.Select(x => new { area = Area(x.l, x.w, x.h), ribbon = Ribbon(x.l, x.w, x.h), smallest = Math.Min(Math.Min(x.l * x.w, x.w * x.h), x.h * x.l) })
	.Select(x => new { amount = x.area + x.smallest, area = x.area, ribbon = x.ribbon});

Console.WriteLine($"Paper Needs: {paperNeeds.Sum(x=>x.amount)}");
Console.WriteLine($"Ribbon Needs: {paperNeeds.Sum(x=>x.ribbon)}");

int Area(int l, int w, int h)
{
	return 2*l*w + 2*w*h + 2*h*l;
}

int Ribbon(int l, int w, int h)
{
	var sides = new int[] { l, w, h };
	var smallerSides = sides.Order().Take(2);
	return smallerSides.Sum() * 2 + l * w * h;
}