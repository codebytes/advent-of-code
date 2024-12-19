//https://adventofcode.com/2024/day/15

var input = await File.ReadAllTextAsync("../input.txt");
//var input = await File.ReadAllTextAsync("../sample.txt");

var (map, width, height, botRow, botCol, guide) = ProcessInput(input, false);
(botRow, botCol) = Walk(map, botRow, botCol, guide, WalkPart1);
Console.WriteLine("The answer for part 1 is " + CountBoxesGps(map, 'O'));

// Reset the state for part 2
(map, width, height, botRow, botCol, guide) = ProcessInput(input, true);
(botRow, botCol) = Walk(map, botRow, botCol, guide, WalkToPart2);
Console.WriteLine("The answer for part 2 is " + CountBoxesGps(map, '['));

(char[][] map, int width, int height, int botRow, int botCol, string guide) ProcessInput(string inputString, bool isPart2)
{
    var sections = inputString.Split(new[] { "\n\n" }, StringSplitOptions.None);

    var processedMap = sections[0]
        .Trim()
        .Split('\n')
        .Select(line => isPart2
            ? ExpandLine(line.Trim())
            : line.Trim().ToCharArray())
        .ToArray();
    var mapHeight = processedMap.Length;
    var mapWidth = processedMap[0].Length;
    var mapGuide = string.Concat(
        sections[1]
            .Trim()
            .Split('\n')
            .Select(segment => segment.Trim()));

    var (botRow, botCol) = FindBotPosition(processedMap);

    return (processedMap, mapWidth, mapHeight, botRow, botCol, mapGuide);
}

char[] ExpandLine(string line) =>
    line.SelectMany(c => c switch
    {
        '#' => "##",
        '.' => "..",
        'O' => "[]",
        '@' => "@.",
        _ => throw new InvalidOperationException()
    }).ToArray();

(int, int) FindBotPosition(char[][] map)
{
    var position = map
        .SelectMany((row, rowIndex) => row.Select((cell, colIndex) => (cell, rowIndex, colIndex)))
        .FirstOrDefault(x => x.cell == '@');

    return (position.rowIndex, position.colIndex);
}

(int, int) Walk(char[][] map, int botRow, int botCol, string guide, Func<char[][], int, int, (int, int), (int, int)> walkTo)
{
    foreach (var direction in guide)
    {
        (botRow, botCol) = walkTo(map, botRow, botCol, direction switch
        {
            '^' => (-1, 0),
            'v' => (1, 0),
            '>' => (0, 1),
            '<' => (0, -1),
            _ => throw new InvalidOperationException()
        });
    }
    return (botRow, botCol);
}

(int, int) WalkPart1(char[][] map, int botRow, int botCol, (int deltaRow, int deltaCol) delta)
{
    var (deltaRow, deltaCol) = delta;
    int futureRow = botRow, futureCol = botCol;

    while (true)
    {
        futureRow += deltaRow;
        futureCol += deltaCol;
        if (map[futureRow][futureCol] == '#')
        {
            return (botRow, botCol);
        }
        if (map[futureRow][futureCol] == '.')
        {
            break;
        }
    }

    while (true)
    {
        int previousRow = futureRow - deltaRow;
        int previousCol = futureCol - deltaCol;
        map[futureRow][futureCol] = map[previousRow][previousCol];
        futureRow = previousRow;
        futureCol = previousCol;
        if (futureRow == botRow && futureCol == botCol) break;
    }

    map[botRow][botCol] = '.';
    botRow += deltaRow;
    botCol += deltaCol;

    return (botRow, botCol);
}

(int, int) WalkToPart2(char[][] map, int botRow, int botCol, (int deltaRow, int deltaCol) delta)
{
    var (deltaRow, deltaCol) = delta;
    return deltaRow switch
    {
        0 => WalkHorizontal(map, botRow, botCol, deltaCol),
        _ => WalkVertical(map, botRow, botCol, deltaRow)
    };
}

(int, int) WalkVertical(char[][] map, int botRow, int botCol, int deltaRow)
{
    int futureRow = botRow + deltaRow;
    char futureSpot = map[futureRow][botCol];

    if (futureSpot == '#') return (botRow, botCol);

    if (futureSpot == '.')
    {
        map[botRow][botCol] = '.';
        botRow = futureRow;
        map[botRow][botCol] = '@';
        return (botRow, botCol);
    }

    if (!MayMoveBoxVertical(map, futureRow, botCol, deltaRow))
    {
        return (botRow, botCol);
    }
    MoveBoxVertical(map, futureRow, botCol, deltaRow);
    map[botRow][botCol] = '.';
    botRow = futureRow;
    map[botRow][botCol] = '@';

    return (botRow, botCol);
}

(int, int) WalkHorizontal(char[][] map, int botRow, int botCol, int deltaCol)
{
    int futureCol = botCol;

    while (true)
    {
        futureCol += deltaCol;
        if (map[botRow][futureCol] == '#')
        {
            return (botRow, botCol);
        }
        if (map[botRow][futureCol] == '.')
        {
            break;
        }
    }

    while (true)
    {
        int previousCol = futureCol - deltaCol;
        map[botRow][futureCol] = map[botRow][previousCol];
        futureCol = previousCol;
        if (futureCol == botCol)
        {
            break;
        }
    }

    map[botRow][botCol] = '.';
    botCol += deltaCol;

    return (botRow, botCol);
}

bool MayMoveBoxVertical(char[][] map, int boxRow, int boxCol, int deltaRow)
{
    if (map[boxRow][boxCol] == ']')
    {
        boxCol -= 1;
    }

    int futureRow = boxRow + deltaRow;
    char futureSpotLeft = map[futureRow][boxCol];
    char futureSpotRight = map[futureRow][boxCol + 1];

    if (futureSpotLeft == '#' || futureSpotRight == '#')
    {
        return false;
    }

    if (futureSpotLeft == '[' && !MayMoveBoxVertical(map, futureRow, boxCol, deltaRow))
    {
        return false;
    }
    if (futureSpotLeft == ']' && !MayMoveBoxVertical(map, futureRow, boxCol, deltaRow))
    {
        return false;
    }
    if (futureSpotRight == '[' && !MayMoveBoxVertical(map, futureRow, boxCol + 1, deltaRow))
    {
        return false;
    }
    return true;
}

void MoveBoxVertical(char[][] map, int boxRow, int boxCol, int deltaRow)
{
    if (map[boxRow][boxCol] == ']')
    {
        boxCol -= 1;
    }

    int futureRow = boxRow + deltaRow;
    char futureSpotLeft = map[futureRow][boxCol];
    char futureSpotRight = map[futureRow][boxCol + 1];

    if (futureSpotLeft == '[')
    {
        MoveBoxVertical(map, futureRow, boxCol, deltaRow);
    }
    if (futureSpotLeft == ']')
    {
        MoveBoxVertical(map, futureRow, boxCol, deltaRow);
    }
    if (futureSpotRight == '[')
    {
        MoveBoxVertical(map, futureRow, boxCol + 1, deltaRow);
    }

    map[boxRow][boxCol] = '.';
    map[boxRow][boxCol + 1] = '.';
    map[futureRow][boxCol] = '[';
    map[futureRow][boxCol + 1] = ']';
}

int CountBoxesGps(char[][] map, char boxChar)
{
    return map.SelectMany((row, rowIndex) =>
        row.Select(
            (cell, colIndex) =>
                cell == boxChar ? 100 * rowIndex + colIndex : 0))
        .Sum();
}