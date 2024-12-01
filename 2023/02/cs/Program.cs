//https://adventofcode.com/2023/day/2
// var input = await File.ReadAllLinesAsync("../sample.txt");
var input = await File.ReadAllLinesAsync("../input.txt");

var games = new List<Game>();
foreach (var line in input)
{
    var parts = line.Split(':');
    var gameId = int.Parse(parts[0].Split(' ')[1]);
    var game = new Game(gameId);
    var setParts = parts[1].Split(';');
    foreach (var part in setParts)
    {
        var colors = part.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var colorPart in colors)
        {
            var split = colorPart.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var count = int.Parse(split[0]);
            var color = split[1];
            game.Add(color, count);
        }
    }
    games.Add(game);
}

var maxCubes = new Dictionary<string, int>
{
    { "red", 12 },
    { "green", 13 },
    { "blue", 14 }
};

var possibleGameIds = games
    .Where(game => game.ColorStats.All(color => color.Value <= maxCubes.GetValueOrDefault(color.Key, 0)))
    .Select(game => game.Id)
    .ToList();

var sumOfPossibleGameIds = possibleGameIds.Sum();
Console.WriteLine($"Sum of possible game IDs: {sumOfPossibleGameIds}");

var totalPower = games.Sum(game =>
{
    var maxRed = game.ColorStats.GetValueOrDefault("red");
    var maxGreen = game.ColorStats.GetValueOrDefault("green");
    var maxBlue = game.ColorStats.GetValueOrDefault("blue");
    return maxRed * maxGreen * maxBlue;
});

Console.WriteLine($"Sum of the power of the minimum sets: {totalPower}");


record Game(int Id)
{
    public Dictionary<string, int> ColorStats { get; private set; } = new();
    public void Add(string color, int count) => ColorStats[color] = Math.Max(ColorStats.GetValueOrDefault(color), count);
}

