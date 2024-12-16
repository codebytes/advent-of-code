//https://adventofcode.com/2024/day/12
//var input = await File.ReadAllLinesAsync("../sample.txt");
var input = await File.ReadAllLinesAsync("../input.txt");

// Parse the input into a 2D array
char[,] map = ParseInput(input);

Console.WriteLine($"Part 1: {CalculateTotalPrice(map, GroupPlot, (m, g) => g.Sum(coord => CalculatePerimeterForCoord(m, coord)))}");
Console.WriteLine($"Part 2: {CalculateTotalPrice(map, BreadthFirstSearch, CalculateSides)}");

char[,] ParseInput(string[] input)
{
    int rows = input.Length;
    int cols = input[0].Length;
    char[,] map = new char[rows, cols];

    for (int i = 0; i < rows; i++)
    {
        for (int j = 0; j < cols; j++)
        {
            map[i, j] = input[i][j];
        }
    }

    return map;
}

int CalculateTotalPrice(char[,] map, Action<char[,], int, int, bool[,], List<(int, int)>> groupFunc, Func<char[,], List<(int, int)>, int> priceFunc) =>
    FindGroups(map, groupFunc)
        .Sum(group => group.Count * priceFunc(map, group));

List<List<(int, int)>> FindGroups(char[,] map, Action<char[,], int, int, bool[,], List<(int, int)>> groupFunc)
{
    bool[,] visited = new bool[map.GetLength(0), map.GetLength(1)];
    return Enumerable.Range(0, map.GetLength(0))
        .SelectMany(i => Enumerable.Range(0, map.GetLength(1)), (i, j) => (i, j))
        .Where(coord => !visited[coord.i, coord.j])
        .Select(coord =>
        {
            var group = new List<(int, int)>();
            groupFunc(map, coord.i, coord.j, visited, group);
            return group;
        })
        .ToList();
}

void BreadthFirstSearch(char[,] map, int x, int y, bool[,] visited, List<(int, int)> group)
{
    Queue<(int, int)> queue = new();
    queue.Enqueue((x, y));
    visited[x, y] = true;
    group.Add((x, y));

    while (queue.Count > 0)
    {
        var (cx, cy) = queue.Dequeue();
        foreach (var (nx, ny) in GetAdjacentCoords(cx, cy))
        {
            if (IsValidPosition(map, nx, ny) && !visited[nx, ny] && map[nx, ny] == map[cx, cy])
            {
                queue.Enqueue((nx, ny));
                visited[nx, ny] = true;
                group.Add((nx, ny));
            }
        }
    }
}

IEnumerable<(int x, int y)> GetAdjacentCoords(int x, int y) =>
    new[] { (x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1) };

int CalculateSides(char[,] map, List<(int, int)> group) =>
    group.Sum(coord => CountCorners(group, coord, true) + CountCorners(group, coord, false));

int CountCorners(List<(int, int)> group, (int x, int y) coord, bool concave)
{
    var directions = new[] { (-1, -1), (-1, 1), (1, -1), (1, 1) };
    return directions.Count(dir =>
    {
        var (dx, dy) = dir;
        var adjacent1 = (coord.x + dx, coord.y);
        var adjacent2 = (coord.x, coord.y + dy);
        var corner = (coord.x + dx, coord.y + dy);
        return concave
            ? group.Contains(adjacent1) && group.Contains(adjacent2) && !group.Contains(corner)
            : !group.Contains(adjacent1) && !group.Contains(adjacent2);
    });
}

void GroupPlot(char[,] map, int x, int y, bool[,] visited, List<(int, int)> group)
{
    if (visited[x, y]) return;

    visited[x, y] = true;
    group.Add((x, y));
    foreach (var (nx, ny) in GetAdjacentCoords(x, y))
    {
        if (IsValidPosition(map, nx, ny) && map[nx, ny] == map[x, y] && !visited[nx, ny])
        {
            GroupPlot(map, nx, ny, visited, group);
        }
    }
}

int CalculatePerimeterForCoord(char[,] map, (int x, int y) coord) =>
    GetAdjacentCoords(coord.x, coord.y)
        .Count(pos => !IsValidPosition(map, pos.x, pos.y) || map[pos.x, pos.y] != map[coord.x, coord.y]);

bool IsValidPosition(char[,] map, int x, int y) =>
    x >= 0 && y >= 0 && x < map.GetLength(0) && y < map.GetLength(1);
