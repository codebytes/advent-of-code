// https://adventofcode.com/2024/day/18
var input = await File.ReadAllTextAsync("../input.txt");
// var input = await File.ReadAllTextAsync("../sample.txt");

var lines = input.Split(Environment.NewLine);
var incomingBytes = lines.Select(line => line.Split(',').Select(int.Parse).ToArray()).ToList();
int gridSize = incomingBytes.Count > 25 ? 71 : 7;
int iterations = incomingBytes.Count > 25 ? 1024 : 12;

var grid = new int[gridSize, gridSize];
for (int i = 0; i < gridSize; i++) {
    for (int j = 0; j < gridSize; j++) {
        grid[i, j] = 1;
    }
}

for (int i = 0; i < iterations; i++) {
    var nextByte = incomingBytes[0];
    incomingBytes.RemoveAt(0);
    grid[nextByte[1], nextByte[0]] = int.MaxValue;
}

var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

int result = ShortestPath(grid, gridSize, directions);
Console.WriteLine(result);

var initialLength = incomingBytes.Count;
for (int i = 0; i < initialLength; i++) {
    var nextByte = incomingBytes[0];
    incomingBytes.RemoveAt(0);
    grid[nextByte[1], nextByte[0]] = int.MaxValue;

    int pathLength = ShortestPath(grid, gridSize, directions);
    if (pathLength == -1) {
        Console.WriteLine($"{nextByte[0]},{nextByte[1]}");
        break;
    }
}

int ShortestPath(int[,] grid, int size, (int, int)[] directions) {
    var queue = new Queue<(int, int, int)>();
    var visited = new bool[size, size];
    queue.Enqueue((0, 0, 0));
    visited[0, 0] = true;

    while (queue.Count > 0) {
        var (x, y, steps) = queue.Dequeue();
        if (x == size - 1 && y == size - 1) return steps;

        foreach (var (dx, dy) in directions) {
            int nx = x + dx, ny = y + dy;
            if (nx >= 0 && nx < size && ny >= 0 && ny < size && !visited[ny, nx] && grid[ny, nx] != int.MaxValue) {
                queue.Enqueue((nx, ny, steps + 1));
                visited[ny, nx] = true;
            }
        }
    }

    return -1; // No path found
}
