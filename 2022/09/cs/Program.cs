//https://adventofcode.com/2022/day/9
var input = await File.ReadAllLinesAsync("../input.txt");
var moves = input.Select(x => ParseMove(x));

int ropeSize = 10;
List<Point> rope = new List<Point>();
var start = new Point(0, 0);
rope.AddRange(Enumerable.Range(0, ropeSize).Select(x => start));

List<Point> tailHistory = new List<Point>() { rope[0] };
List<Point> longTailHistory = new List<Point>() { rope[0] };

var current = start;
foreach (var move in moves)
{
	//move the head
	current = MovePoint(current, move);
	rope[^1] = current;
	bool segmentMoved;
	do
	{
		//for each segment, check if it needs to move
		segmentMoved = false;
		for (int i = ropeSize - 1; i > 0; i--)
		{
			var head = rope[i];
			var tail = rope[i - 1];
			if (Math.Sqrt(Math.Pow(head.x - tail.x, 2) + Math.Pow(head.y - tail.y, 2)) >= 2.0)
			{
				rope[i - 1] = new Point(tail.x + GetStepSize(head.x, tail.x), tail.y + GetStepSize(head.y, tail.y));
				segmentMoved = true;
			}
		}
		tailHistory.Add(rope[ropeSize - 2]);
		longTailHistory.Add(rope[0]);
	} while (segmentMoved);
}

Console.WriteLine($"Part1 {tailHistory.Distinct().Count()}");
Console.WriteLine($"Part2 {longTailHistory.Distinct().Count()}");

Point MovePoint(Point p, (string dir, int dist) move)
{
	(int x, int y) = move.dir switch
	{
		"U" => (0, 1),
		"D" => (0, -1),
		"L" => (-1, 0),
		"R" => (1, 0),
		_ => (0, 0)
	};
	return p with
	{
		x = p.x + move.dist * x,
		y = p.y + move.dist * y
	};
}

int GetStepSize(int a, int b) => a == b ? 0
        : (int)(Math.Abs(a - b) / (a - b));
		
(string dir, int dist) ParseMove(string move)
{
	var parts = move.Split(" ");
	return (parts[0], int.Parse(parts[1]));
}

public record Point(int x, int y);
