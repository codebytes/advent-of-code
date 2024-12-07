//https://adventofcode.com/2024/day/4
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var grid = input
    .Trim()
    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Trim().ToCharArray())
    .ToArray();

int count = CountOccurrences(grid, "XMAS");
Console.WriteLine($"Total occurrences of 'XMAS': {count}");

int xMasCount = CountXMasOccurrences(grid);
Console.WriteLine($"Total occurrences of 'X-MAS': {xMasCount}");

int CountOccurrences(char[][] grid, string word)
{
    int rows = grid.Length;
    int cols = grid[0].Length;
    int len = word.Length;

    var directions = new (int dx, int dy)[]
    {
        (-1, -1), (-1, 0), (-1, 1),
        (0, -1),          (0, 1),
        (1, -1),  (1, 0),  (1, 1)
    };

    return (
        from r in Enumerable.Range(0, rows)
        from c in Enumerable.Range(0, cols)
        from dir in directions
        let positions = Enumerable.Range(0, len)
            .Select(i => (nr: r + i * dir.dx, nc: c + i * dir.dy, idx: i))
        where positions.All(pos =>
            pos.nr >= 0 && pos.nr < rows &&
            pos.nc >= 0 && pos.nc < cols &&
            grid[pos.nr][pos.nc] == word[pos.idx])
        select 1
    ).Count();
}

static int CountXMasOccurrences(char[][] grid)
{
    int count = 0;
    int rows = grid.Length;
    int cols = grid[0].Length;

    for (int r = 0; r < rows - 2; r++)
    {
        for (int c = 0; c < cols - 2; c++)
        {
            if (IsXMasPattern(grid, r, c))
            {
                count++;
            }
        }
    }

    return count;
}

static bool IsXMasPattern(char[][] grid, int r, int c)
{
    if (grid[r + 1][c + 1] != 'A')
    {
        return false;
    }

    char[] corners = [
        grid[r][c],
        grid[r][c + 2],
        grid[r + 2][c],
        grid[r + 2][c + 2]
    ];

    char[][] patterns = [
        ['M', 'S', 'M', 'S'],
        ['S', 'M', 'S', 'M']
    ];

    var rotations = patterns.SelectMany(pattern => new[]
    {
        new[] { pattern[0], pattern[1], pattern[2], pattern[3] },
        [pattern[2], pattern[0], pattern[3], pattern[1]],
        [pattern[3], pattern[2], pattern[1], pattern[0]],
        [pattern[1], pattern[3], pattern[0], pattern[2]]
    });

    return rotations.Any(rotation => corners.SequenceEqual(rotation));
}
