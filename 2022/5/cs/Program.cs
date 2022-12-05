using System.Text.RegularExpressions;

//https://adventofcode.com/2022/day/5
var input = await File.ReadAllLinesAsync("../input.txt");

var chunks = input
  .Split((prev, next) => next == "")
  .Select(x => x
		.Where(y => y != ""));

var graph = chunks.First();
var instructions = chunks.Last();

var parsed = graph
	.Select(x => x.Chunk(4))
	.Select(x=>x.Select(y=>y[1]))
	.Reverse()
	.Skip(1);

var state = new List<Stack<char>>(parsed.First().Count());
parsed
	.ToList()
	.ForEach(x => x
		.Select((y, i) =>
			{
				while(state.Count<=i)
				{
					state.Add(new Stack<char>());
				}
				if (y != ' ')
				{
					state[i].Push(y);
				}
				return y;
			}
		)
        .ToList()
    );


Regex moveRegex = new Regex(@"move (?<count>\d+) from (?<from>\d+) to (?<to>\d+)", RegexOptions.Compiled);

instructions.ToList().ForEach(x =>
{
	var matches = moveRegex.Matches(x);
	var move = matches.Select(x => new { 
	count = int.Parse(x.Groups["count"].Value), 
	from = int.Parse(x.Groups["from"].Value), 
	to = int.Parse(x.Groups["to"].Value)}).First();
    //part1
	// for(int i =0; i<move.count; i++)
	// {
	// 	state[move.to-1].Push(state[move.from-1].Pop());
	// }
    //part2
    Enumerable.Range(0, move.count)
        .Select(x=>state[move.from-1].Pop())
        .Reverse()
        .ToList()
        .ForEach(y=>state[move.to-1].Push(y));
});

state.ForEach(s=>Console.Write(s.Peek()));

public static class Extensions
{
	public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Func<T, T, bool> splitOn)
	{
		using (var e = source.GetEnumerator())
		{
			for (bool more = e.MoveNext(); more;)
			{
				var last = e.Current;
				var group = new List<T> { last };
				while ((more = e.MoveNext()) && !splitOn(last, e.Current))
					group.Add(last = e.Current);
				yield return group;
			}
		}
	}
}