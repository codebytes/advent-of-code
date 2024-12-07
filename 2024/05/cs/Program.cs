//https://adventofcode.com/2024/day/5
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var sections = input.Split(new[] { "\n\r\n" }, StringSplitOptions.None);
var rules = sections[0].Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                      .Where(line => line.Contains('|'))
                      .Select(line => line.Trim().Split('|').Select(int.Parse).ToArray()).ToList();
var updates = sections[1].Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(line => line.Contains(','))
                        .Select(line => line.Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse).ToList()).ToList();

bool IsCorrectOrder(List<int> update, List<int[]> rules)
{
    var indexMap = update
        .Select((page, index) => (page, index))
        .ToDictionary(x => x.page, x => x.index);
    return rules.All(rule => !indexMap
        .ContainsKey(rule[0]) 
            || !indexMap.ContainsKey(rule[1]) 
            || indexMap[rule[0]] < indexMap[rule[1]]);
}

List<int> ReorderUpdate(List<int> update, List<int[]> rules)
{
    var graph = update.ToDictionary(page => page, page => new List<int>());
    var inDegree = update.ToDictionary(page => page, page => 0);

    foreach (var rule in rules
        .Where(rule => graph.ContainsKey(rule[0]) 
            && graph.ContainsKey(rule[1])))
    {
        var from = rule[0];
        var to = rule[1];
        graph[from].Add(to);
        inDegree[to]++;
    }

    var queue = new Queue<int>(inDegree
        .Where(kv => kv.Value == 0)
        .Select(kv => kv.Key));
    var sortedUpdate = new List<int>();

    while (queue.Count > 0)
    {
        var page = queue.Dequeue();
        sortedUpdate.Add(page);

        foreach (var neighbor in graph[page])
        {
            if (--inDegree[neighbor] == 0)
            {
                queue.Enqueue(neighbor);
            }
        }
    }

    return sortedUpdate;
}

int GetMiddlePage(List<int> update)
{
    return update[update.Count / 2];
}

// Part 1
var correctUpdates = updates
    .Where(update => IsCorrectOrder(update, rules))
    .ToList();
var middlePagesSumPart1 = correctUpdates
    .Select(GetMiddlePage)
    .Sum();
Console.WriteLine($"Part1: {middlePagesSumPart1}");

// Part 2
var incorrectUpdates = updates
    .Where(update => !IsCorrectOrder(update, rules))
    .ToList();
var reorderedUpdates = incorrectUpdates
    .Select(update => ReorderUpdate(update, rules))
    .ToList();
var middlePagesSum = reorderedUpdates
    .Select(GetMiddlePage)
    .Sum();

Console.WriteLine($"Part 2: {middlePagesSum}");
