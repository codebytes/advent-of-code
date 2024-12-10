//https://adventofcode.com/2024/day/10
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var map = input.Split('\n')
    .Select(line => line.Trim().Select(c => c - '0').ToArray())
    .ToArray();

int rows = map.Length;
int cols = map[0].Length;

(int dRow, int dCol)[] directions = { (-1, 0), (1, 0), (0, -1), (0, 1) };

bool IsInside(int row, int col) => row >= 0 && row < rows && col >= 0 && col < cols;

long[,] paths = new long[rows, cols];

foreach (var (row, col) in from r in Enumerable.Range(0, rows)
                           from c in Enumerable.Range(0, cols)
                           where map[r][c] == 9
                           select (r, c))
{
    paths[row, col] = 1;
}

foreach (int height in Enumerable.Range(0, 9).Reverse())
{
    foreach (var (row, col) in from r in Enumerable.Range(0, rows)
                               from c in Enumerable.Range(0, cols)
                               where map[r][c] == height
                               select (r, c))
    {
        foreach (var (dR, dC) in directions)
        {
            int newRow = row + dR;
            int newCol = col + dC;
            if (IsInside(newRow, newCol) && map[newRow][newCol] == height + 1)
            {
                paths[row, col] += paths[newRow, newCol];
            }
        }
    }
}

int totalScore = 0;

var trailheads = from r in Enumerable.Range(0, rows)
                 from c in Enumerable.Range(0, cols)
                 where map[r][c] == 0
                 select (r, c);

foreach (var (row, col) in trailheads)
{
    var visited = new bool[rows, cols];
    var queue = new Queue<(int row, int col)>();
    var reachableNines = new HashSet<(int, int)>();
    queue.Enqueue((row, col));
    visited[row, col] = true;

    while (queue.Count > 0)
    {
        var (curRow, curCol) = queue.Dequeue();

        switch (map[curRow][curCol])
        {
            case 9:
                reachableNines.Add((curRow, curCol));
                break;
            default:
                foreach (var (dR, dC) in directions)
                {
                    int newRow = curRow + dR;
                    int newCol = curCol + dC;
                    if (IsInside(newRow, newCol) && !visited[newRow, newCol] &&
                        map[newRow][newCol] == map[curRow][curCol] + 1)
                    {
                        queue.Enqueue((newRow, newCol));
                        visited[newRow, newCol] = true;
                    }
                }
                break;
        }
    }

    totalScore += reachableNines.Count;
}

long totalRating = trailheads.Sum(pos => paths[pos.r, pos.c]);

Console.WriteLine($"Total score of all trailheads (Part 1): {totalScore}");
Console.WriteLine($"Total sum of trailhead ratings (Part 2): {totalRating}");
