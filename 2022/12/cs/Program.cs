//https://adventofcode.com/2022/day/12
var input = await File.ReadAllLinesAsync("../input.txt");

var grid = new Grid(input);

var part1Goal = (Coord coord, Grid grid) => grid.End.x == coord.x && grid.End.y == coord.y;
var part1IsValidMove = (char current, char next) => next <= current + 1;

var result = pathFind(grid, part1Goal, part1IsValidMove);

Console.WriteLine($"Part1 steps: {result}");

//just set start at the previous End, and find the first a
var part2Goal = (Coord coord, Grid grid) => grid.GetCell(coord) == 'a';
var part2IsValidMove = (char current, char next) => current <= next + 1;
grid.Start = grid.End;
result = pathFind(grid, part2Goal, part2IsValidMove);

Console.WriteLine($"Part2 steps: {result}");

//basic breadth first
int pathFind(Grid grid, Func<Coord, Grid, bool> goal, Func<char, char, bool> isValidMove)
{
	var queue = new Queue<Node>();
	var visitedCoordinates = new bool[grid.Width, grid.Height];

	queue.Enqueue(new Node(grid.Start, 0));
	visitedCoordinates[grid.Start.x, grid.Start.y] = true;

	while (queue.Any())
	{
		Node current = queue.Dequeue();
		if (goal(current.coord, grid))
		{
			return current.steps;
		}

		foreach (var dir in Enum.GetValues(typeof(Direction)))
		{
			var newPos = current.coord with
			{
				x = current.coord.x + dir switch
				{
					Direction.Right => 1,
					Direction.Left => -1,
					_ => 0
				},

				y = current.coord.y + dir switch
				{
					Direction.Up => -1,
					Direction.Down => 1,
					_ => 0
				}
			};
			if (!grid.InBounds(newPos))
			{
				continue;
			}
			if (!visitedCoordinates[newPos.x, newPos.y] && isValidMove(grid.GetCell(current.coord), grid.GetCell(newPos)))
			{
				queue.Enqueue(new Node(newPos, current.steps + 1));
				visitedCoordinates[newPos.x, newPos.y] = true;
			}
		}
	}
	return 0;
}

public enum Direction
{
	Right,
	Up,
	Left,
	Down
};

public record Coord(int x, int y);
public record Node(Coord coord, int steps);
public class Grid
{
	public Coord Start { get; set; } = new Coord(0,0);
	public Coord End { get; set; } = new Coord(0,0);
	public int Width { get; set; }
	public int Height { get; set; }
	List<char> gridData = new List<char>();

	public Grid(string[] input)
	{
		Height = input.Count();
		Width = input[0].Count();
		int height = 0;
		foreach (var line in input)
		{
			for (int i = 0; i < line.Length; i++)
			{
				switch (line[i])
				{
					case 'S':
						Start = new Coord(i, height);
						gridData.Add('a');
						break;
					case 'E':
						End = new Coord(i, height);
						gridData.Add('z');
						break;
					default:
						gridData.Add(line[i]);
						break;
				};
			}
			height++;
		}
	}
	public char GetCell(Coord coord) => gridData[coord.x + coord.y * this.Width];
	public bool InBounds(Coord coord) => coord.x >= 0 && coord.x < Width && coord.y >= 0 && coord.y < Height;
}

