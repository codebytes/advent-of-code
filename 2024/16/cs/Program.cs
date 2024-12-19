//https://adventofcode.com/2024/day/16

var input = await File.ReadAllTextAsync("../input.txt");
// var input = await File.ReadAllTextAsync("../sample.txt");

var (table, homeRow, homeCol, goalCell) = ProcessInput(input);
FindPath(table, homeRow, homeCol);

var part1Answer = Math.Min(goalCell.North, Math.Min(goalCell.South, Math.Min(goalCell.East, goalCell.West)));
Console.WriteLine($"Part 1 answer: {part1Answer}");

// Search for paths
TracePath(table, goalCell);

// Count the cells in paths for Part 2 answer
var part2Answer = CountCellsInPaths(table);
Console.WriteLine($"Part 2 answer: {part2Answer}");

(List<List<Cell>>, int, int, Cell) ProcessInput(string input)
{
    var table = new List<List<Cell>>();
    int homeRow = 0, homeCol = 0;
    Cell? goalCell = null;

    var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    int row = -1;

    foreach (var rawLine in lines)
    {
        var line = new List<Cell>();
        table.Add(line);
        row++;

        int col = -1;
        foreach (var c in rawLine.Trim())
        {
            col++;
            var cell = new Cell(row, col, c == '#', int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
            line.Add(cell);

            if (c == 'S') { homeRow = row; homeCol = col; }
            if (c == 'E') { goalCell = cell; }
        }
    }

    return (table, homeRow, homeCol, goalCell!);
}

void FindPath(List<List<Cell>> table, int homeRow, int homeCol)
{
    var homeCell = table[homeRow][homeCol];
    homeCell.East = 0;

    var cellsToWalk = new Stack<Cell>();
    cellsToWalk.Push(homeCell);

    while (cellsToWalk.Count > 0)
    {
        var cell = cellsToWalk.Pop();
        int leastCost = Math.Min(cell.North, Math.Min(cell.South, Math.Min(cell.East, cell.West)));

        ProcessCellNorth(table, cell, leastCost, cellsToWalk);
        ProcessCellSouth(table, cell, leastCost, cellsToWalk);
        ProcessCellEast(table, cell, leastCost, cellsToWalk);
        ProcessCellWest(table, cell, leastCost, cellsToWalk);
    }
}

void ProcessCellNorth(List<List<Cell>> table, Cell previousCell, int leastCost, Stack<Cell> cellsToWalk)
{
    var cell = table[previousCell.Row - 1][previousCell.Col];
    if (cell.Blocked) return;

    int cost = leastCost + 1;

    if (leastCost == previousCell.North)
    {
        // Do nothing
    }
    else if (leastCost == previousCell.East || leastCost == previousCell.West)
    {
        cost += 1000;
    }
    else
    {
        cost += 2000;
    }

    if (cost >= cell.North) return;
    cell.North = cost;
    cellsToWalk.Push(cell);
}

void ProcessCellSouth(List<List<Cell>> table, Cell previousCell, int leastCost, Stack<Cell> cellsToWalk)
{
    var cell = table[previousCell.Row + 1][previousCell.Col];
    if (cell.Blocked) return;

    int cost = leastCost + 1;
    if (leastCost != previousCell.South) cost += (leastCost == previousCell.East || leastCost == previousCell.West) ? 1000 : 2000;

    if (cost >= cell.South) return;
    cell.South = cost;
    cellsToWalk.Push(cell);
}
 
void ProcessCellEast(List<List<Cell>> table, Cell previousCell, int leastCost, Stack<Cell> cellsToWalk)
{
    var cell = table[previousCell.Row][previousCell.Col + 1];
    if (cell.Blocked) return;

    int cost = leastCost + 1;
    if (leastCost != previousCell.East) cost += (leastCost == previousCell.North || leastCost == previousCell.South) ? 1000 : 2000;

    if (cost >= cell.East) return;
    cell.East = cost;
    cellsToWalk.Push(cell);
}

void ProcessCellWest(List<List<Cell>> table, Cell previousCell, int leastCost, Stack<Cell> cellsToWalk)
{
    var cell = table[previousCell.Row][previousCell.Col - 1];
    if (cell.Blocked) return;

    int cost = leastCost + 1;
    if (leastCost != previousCell.West) cost += (leastCost == previousCell.North || leastCost == previousCell.South) ? 1000 : 2000;

    if (cost >= cell.West) return;
    cell.West = cost;
    cellsToWalk.Push(cell);
}

void TracePath(List<List<Cell>> table, Cell goalCell)
{
    goalCell.InPath = true;

    int leastCost = Math.Min(goalCell.North, Math.Min(goalCell.South, Math.Min(goalCell.East, goalCell.West)));

    TracePathFrom(table, table[goalCell.Row - 1][goalCell.Col], leastCost, Direction.South);
    TracePathFrom(table, table[goalCell.Row + 1][goalCell.Col], leastCost, Direction.North);
    TracePathFrom(table, table[goalCell.Row][goalCell.Col + 1], leastCost, Direction.West);
    TracePathFrom(table, table[goalCell.Row][goalCell.Col - 1], leastCost, Direction.East);
}

void TracePathFrom(List<List<Cell>> table, Cell cell, int targetCost, Direction pathDirection)
{
    if (cell.Blocked || cell.InPath) return;

    switch (pathDirection)
    {
        case Direction.North:
            if (cell.North + 1 == targetCost)
                TracePathTo(table, cell, cell.North, Direction.North);
            if (cell.East + 1001 == targetCost)
                TracePathTo(table, cell, cell.East, Direction.East);
            if (cell.West + 1001 == targetCost)
                TracePathTo(table, cell, cell.West, Direction.West);
            break;
        case Direction.South:
            if (cell.South + 1 == targetCost)
                TracePathTo(table, cell, cell.South, Direction.South);
            if (cell.East + 1001 == targetCost)
                TracePathTo(table, cell, cell.East, Direction.East);
            if (cell.West + 1001 == targetCost)
                TracePathTo(table, cell, cell.West, Direction.West);
            break;
        case Direction.East:
            if (cell.East + 1 == targetCost)
                TracePathTo(table, cell, cell.East, Direction.East);
            if (cell.North + 1001 == targetCost)
                TracePathTo(table, cell, cell.North, Direction.North);
            if (cell.South + 1001 == targetCost)
                TracePathTo(table, cell, cell.South, Direction.South);
            break;
        case Direction.West:
            if (cell.West + 1 == targetCost)
                TracePathTo(table, cell, cell.West, Direction.West);
            if (cell.North + 1001 == targetCost)
                TracePathTo(table, cell, cell.North, Direction.North);
            if (cell.South + 1001 == targetCost)
                TracePathTo(table, cell, cell.South, Direction.South);
            break;
    }
}

void TracePathTo(List<List<Cell>> table, Cell cell, int targetCost, Direction pathDirection)
{
    cell.InPath = true;

    TracePathFrom(table, table[cell.Row - 1][cell.Col], targetCost, pathDirection);
    TracePathFrom(table, table[cell.Row + 1][cell.Col], targetCost, pathDirection);
    TracePathFrom(table, table[cell.Row][cell.Col + 1], targetCost, pathDirection);
    TracePathFrom(table, table[cell.Row][cell.Col - 1], targetCost, pathDirection);
}

int CountCellsInPaths(List<List<Cell>> table) =>
    table.Sum(line => line.Count(cell => cell.InPath));

enum Direction
{
    North,
    South,
    East,
    West
}

class Cell
{
    public int Row { get; }
    public int Col { get; }
    public bool Blocked { get; }
    public int North { get; set; }
    public int South { get; set; }
    public int East { get; set; }
    public int West { get; set; }
    public bool InPath { get; set; }

    public Cell(int row, int col, bool blocked, int north, int south, int east, int west)
    {
        Row = row;
        Col = col;
        Blocked = blocked;
        North = north;
        South = south;
        East = east;
        West = west;
        InPath = false;
    }
}