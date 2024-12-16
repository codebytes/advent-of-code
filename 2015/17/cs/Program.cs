//https://adventofcode.com/2015/day/17
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");
var containers = input.Split('\n').Select(int.Parse).ToList();
int target = 150;

var allCombinations = GetCombinations(containers);
var validCombinations = allCombinations
    .Where(c => c.Sum() == target)
    .ToList();

Console.WriteLine($"Part 1: {validCombinations.Count}");

int minContainers = validCombinations.Min(c => c.Count);
var minContainerCombinations = validCombinations.Where(c => c.Count == minContainers).ToList();

Console.WriteLine($"Part 2: {minContainerCombinations.Count}");

IEnumerable<List<int>> GetCombinations(List<int> list)
{
    int count = list.Count;
    int combinations = 1 << count;
    for (int i = 1; i < combinations; i++)
    {
        var combination = new List<int>();
        for (int j = 0; j < count; j++)
        {
            if ((i & (1 << j)) != 0)
            {
                combination.Add(list[j]);
            }
        }
        yield return combination;
    }
}

