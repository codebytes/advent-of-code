//https://adventofcode.com/2022/day/8
var input = await File.ReadAllLinesAsync("../input.txt");

var treeGrid = input
	.Select(x => x.Select(y => (int)y-48).ToArray())
	.ToArray();
	

var visible = 0;
var maxScenicScore = 0;
var numRows = treeGrid.Length;
var numCols = treeGrid[0].Length;
for (int r = 0; r < numRows; r++)
{
	for (int c = 0; c < numCols; c++)
	{
		visible += isVisible(r, c, treeGrid) ? 1 : 0;
		maxScenicScore = Math.Max(maxScenicScore, ScenicScore(r, c, treeGrid));
	}
}

bool isVisible(int row, int col, int[][] grid)
{
	if (row == 0
		|| col == 0
		|| row == grid.Length - 1
		|| col == grid[0].Length - 1)
	{
		return true;
	}
	int tree = grid[row][col];
	var treesToLeft = grid[row][..col];
	var treesToRight = grid[row][(col+1)..];
	var (treesAbove, treesBelow) = TreesAboveAndBelow(row, col, grid);
	return tree > treesToLeft.Max() ||
		tree > treesToRight.Max() ||
		tree > treesAbove.Max() ||
		tree > treesBelow.Max();
}

(int[],int[]) TreesAboveAndBelow(int row, int col, int[][] grid)
{
	var above = new List<int>();
	var below = new List<int>();
	for (int r = 0; r < grid[0].Length; r++)
	{
		if (r < row)
		{
			above.Add(grid[r][col]);
		}
		if (r > row)
		{
			below.Add(grid[r][col]);
		}
	}
	above.Reverse();
	return (above.ToArray(), below.ToArray());
}

int ScenicScore(int row, int col, int[][] grid)
{
	int tree = grid[row][col];
	var treesToLeft = grid[row][..col].Reverse().ToArray();
	var treesToRight = grid[row][(col + 1)..];
	var (treesAbove, treesBelow) = TreesAboveAndBelow(row, col, grid);
	return viewDist(tree, treesToLeft) * viewDist(tree, treesToRight) * viewDist(tree, treesAbove) * viewDist(tree, treesBelow);
}

int viewDist(int tree, int[] trees)
{
	int dist = 0;
	do
	{
	} while (dist < trees.Length && tree > trees[dist++]);
	return dist;
}

// part1

Console.WriteLine($"Visible Trees: {visible}");
Console.WriteLine($"Max Scenic Score: {maxScenicScore}");