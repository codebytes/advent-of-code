//https://adventofcode.com/2024/day/6
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
int rows = lines.Length;
int cols = lines[0].Length;

char[,] map = InitializeMap(lines, rows, cols);
var guardPosition = FindGuardPosition(map, rows, cols);

(int x, int y, char direction) = guardPosition;
map[x, y] = '.';

HashSet<(int, int)> visited = TrackGuardPath(map, rows, cols, ref x, ref y, ref direction);

Console.WriteLine($"Distinct positions visited: {visited.Count}");

map = InitializeMap(lines, rows, cols);
guardPosition = FindGuardPosition(map, rows, cols);
(x, y, direction) = guardPosition;

List<(int x, int y)> possibleObstructions = FindObstructionPositions(map, (x, y), direction);

Console.WriteLine($"Possible positions for new obstruction: {possibleObstructions.Count}");

char[,] InitializeMap(string[] lines, int rows, int cols)
{
    char[,] map = new char[rows, cols];
    Enumerable.Range(0, rows).ToList().ForEach(i =>
        Enumerable.Range(0, cols).ToList().ForEach(j => map[i, j] = lines[i][j])
    );
    return map;
}

(int, int, char) FindGuardPosition(char[,] map, int rows, int cols)
{
    return (from i in Enumerable.Range(0, rows)
            from j in Enumerable.Range(0, cols)
            where "^>v<".Contains(map[i, j])
            select (i, j, direction: map[i, j])).First();
}

HashSet<(int, int)> TrackGuardPath(char[,] map, int rows, int cols, ref int x, ref int y, ref char direction)
{
    HashSet<(int, int)> visited = new HashSet<(int, int)>();
    visited.Add((x, y));

    while (x >= 0 && x < rows && y >= 0 && y < cols)
    {
        var (nx, ny) = MoveForward((x, y), direction);

        if (nx < 0 || nx >= rows || ny < 0 || ny >= cols)
        {
            break;
        }

        if (map[nx, ny] != '#')
        {
            x = nx;
            y = ny;
            visited.Add((x, y));
        }
        else
        {
            direction = TurnRight(direction);
        }
    }

    return visited;
}

bool IsGuardStuck(char[,] map, (int x, int y) startPosition, char startDirection)
{
    var position = startPosition;
    var direction = startDirection;
    var visited = new HashSet<(int, int, char)>();

    while (true)
    {
        if (visited.Contains((position.x, position.y, direction)))
        {
            return true;
        }
        visited.Add((position.x, position.y, direction));

        var nextPosition = MoveForward(position, direction);
        if (nextPosition.x < 0 || nextPosition.x >= map.GetLength(0) || nextPosition.y < 0 || nextPosition.y >= map.GetLength(1))
        {
            return false;
        }

        if (map[nextPosition.x, nextPosition.y] == '#')
        {
            direction = TurnRight(direction);
        }
        else
        {
            position = nextPosition;
        }
    }
}

List<(int x, int y)> FindObstructionPositions(char[,] map, (int x, int y) guardPosition, char guardDirection)
{
    var possibleObstructions = new List<(int x, int y)>();

    for (int i = 0; i < map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            if (map[i, j] == '.' && (i, j) != guardPosition)
            {
                map[i, j] = '#';
                if (IsGuardStuck(map, guardPosition, guardDirection))
                {
                    possibleObstructions.Add((i, j));
                }
                map[i, j] = '.';
            }
        }
    }

    return possibleObstructions;
}

(int x, int y) MoveForward((int x, int y) position, char direction)
{
    return direction switch
    {
        '^' => (position.x - 1, position.y),
        '>' => (position.x, position.y + 1),
        'v' => (position.x + 1, position.y),
        '<' => (position.x, position.y - 1),
        _ => position
    };
}

char TurnRight(char direction)
{
    return direction switch
    {
        '^' => '>',
        '>' => 'v',
        'v' => '<',
        '<' => '^',
        _ => direction
    };
}
