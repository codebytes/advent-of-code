//https://adventofcode.com/2022/day/4
var input = await File.ReadAllLinesAsync("../input.txt");

var pairs = input
	.Select(x=>x.Split(","))
	.Select(x => x.Select(y => y.Split("-").Select(z => int.Parse(z))))
	.Select(x => x.Select(y => new { Start = y.First(), End = y.Skip(1).First() }))
	.Select(x => new {First= x.First(), Second= x.Skip(1).First()});

//part 1
var fullyOverlappedCount = pairs
	.Count(x=>x.First.Start <= x.Second.Start && 
			  x.First.Start <= x.Second.End && 
			  x.Second.Start <= x.First.End &&
			  x.Second.End <= x.First.End ||
			  x.Second.Start <= x.First.Start &&
			  x.Second.Start <= x.First.End &&
			  x.First.Start <= x.Second.End &&
			  x.First.End <= x.Second.End);

Console.WriteLine($"Full Overlapped Results: {fullyOverlappedCount}");

//part 2

var overlappedCount = pairs
.Count(x=>x.First.Start <= x.Second.End && 
			  x.Second.Start <= x.First.End ||
			  x.Second.Start <= x.First.End &&
			  x.First.Start <= x.Second.End);
Console.WriteLine($"Overlapped Results: {overlappedCount}");