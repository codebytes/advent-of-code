//https://adventofcode.com/2015/day/18
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var lines = System.Linq.Enumerable.ToArray(
	System.Linq.Enumerable.Select(
		input.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries),
		line => line.Trim()));

var grid = System.Linq.Enumerable.ToArray(
	System.Linq.Enumerable.Select(
		lines,
		line => System.Linq.Enumerable.ToArray(
			System.Linq.Enumerable.Select(line, cell => cell == '#'))));
var isSample = grid.Length == 6 && grid[0].Length == 6;
var part1Steps = isSample ? 4 : 100;
var part2Steps = isSample ? 5 : 100;

var part1 = RunSimulation(CloneGrid(grid), part1Steps, stuckCorners: false);
Console.WriteLine("Part 1: " + part1);

var part2 = RunSimulation(CloneGrid(grid), part2Steps, stuckCorners: true);
Console.WriteLine("Part 2: " + part2);

static bool[][] CloneGrid(bool[][] source) =>
	System.Array.ConvertAll(source, row => (bool[])row.Clone());

static int RunSimulation(bool[][] initial, int steps, bool stuckCorners)
{
	var current = initial;
	var next = System.Array.ConvertAll(current, row => new bool[row.Length]);
	var height = current.Length;
	var width = current[0].Length;
	var offsets = new (int dr, int dc)[]
	{
		(-1, -1), (-1, 0), (-1, 1),
		(0, -1),            (0, 1),
		(1, -1),  (1, 0),  (1, 1)
	};

	if (stuckCorners)
	{
		ForceCornersOn(current);
	}

	for (var step = 0; step < steps; step++)
	{
		for (var row = 0; row < height; row++)
		{
			for (var col = 0; col < width; col++)
			{
				var neighborsOn = 0;

				foreach (var (dr, dc) in offsets)
				{
					var nr = row + dr;
					var nc = col + dc;

					if (nr >= 0 && nr < height && nc >= 0 && nc < width && current[nr][nc])
					{
						neighborsOn++;
					}
				}

				next[row][col] = current[row][col]
					? neighborsOn == 2 || neighborsOn == 3
					: neighborsOn == 3;
			}
		}

		(current, next) = (next, current);

		if (stuckCorners)
		{
			ForceCornersOn(current);
		}
	}

	return CountOn(current);
}

static int CountOn(bool[][] grid)
{
	var total = 0;
	for (var row = 0; row < grid.Length; row++)
	{
		for (var col = 0; col < grid[row].Length; col++)
		{
			if (grid[row][col])
			{
				total++;
			}
		}
	}
	return total;
}

static void ForceCornersOn(bool[][] grid)
{
	var lastRow = grid.Length - 1;
	var lastCol = grid[0].Length - 1;
	grid[0][0] = true;
	grid[0][lastCol] = true;
	grid[lastRow][0] = true;
	grid[lastRow][lastCol] = true;
}

