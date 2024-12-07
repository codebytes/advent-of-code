//https://adventofcode.com/2015/day/15
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var ingredients = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
    .Select(line => line.Split(new[] { ": ", ", " }, StringSplitOptions.None))
    .Select(parts => new
    {
        Name = parts[0],
        Capacity = int.Parse(parts[1].Split(' ')[1]),
        Durability = int.Parse(parts[2].Split(' ')[1]),
        Flavor = int.Parse(parts[3].Split(' ')[1]),
        Texture = int.Parse(parts[4].Split(' ')[1]),
        Calories = int.Parse(parts[5].Split(' ')[1])
    }).ToArray();

var combinations  = Enumerable.Range(0, 101)
    .Combinations(ingredients.Length, 100);

var maxScore = combinations.Max(amounts => CalculateScore(ingredients, amounts));

Console.WriteLine($"Max score: {maxScore}");

maxScore = combinations
    .Where(amounts => CalculateCalories(ingredients, amounts) == 500)
    .Max(amounts => CalculateScore(ingredients, amounts));

Console.WriteLine($"Max score with 500 calories: {maxScore}");

int CalculateScore(dynamic[] ingredients, int[] amounts) =>
    new[] { "Capacity", "Durability", "Flavor", "Texture" }
        .Select(property => Math.Max(0, ingredients.Select((ingredient, index) =>
            (int)ingredient.GetType().GetProperty(property).GetValue(ingredient) * amounts[index]).Sum()))
        .Aggregate(1, (acc, score) => acc * score);

int CalculateCalories(dynamic[] ingredients, int[] amounts) =>
    ingredients.Select((ingredient, index) =>
        (int)ingredient.GetType().GetProperty("Calories").GetValue(ingredient) * amounts[index]).Sum();

public static class Extensions
{
    public static IEnumerable<int[]> Combinations(this IEnumerable<int> source, int count, int total)
    {
        return Enumerable.Repeat(0, count).Aggregate(
            (IEnumerable<int[]>)[[]],
            (combinations, _) => combinations.SelectMany(c =>
                Enumerable.Range(0, total - c.Sum() + 1)
                    .Select(n => c.Append(n).ToArray())
            )
        ).Where(c => c.Sum() == total);
    }
}