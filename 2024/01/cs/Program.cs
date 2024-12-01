//https://adventofcode.com/2024/day/1
// var input = await File.ReadAllLinesAsync("../sample.txt");
var input = await File.ReadAllLinesAsync("../input.txt");

var list1 = new List<int>();
var list2 = new List<int>();
foreach (var line in input)
{
    var (number1, number2) = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(int.Parse)
                                 .ToArray() switch
    {
        int[] arr when arr.Length == 2 => (arr[0], arr[1]),
        _ => throw new FormatException("Each line must contain exactly 2 numbers separated by whitespace.")
    };

    list1.Add(number1);
    list2.Add(number2);
}

list1 = list1.OrderBy(x => x).ToList();
list2 = list2.OrderBy(x => x).ToList();

var result = list1.Zip(list2, (a, b) => Math.Abs(a - b)).Sum();
Console.WriteLine($"Part 1: {result}");

var result2 = list1.Sum(item => list2.Count(x => x == item) * item);
Console.WriteLine($"Part 2: {result2}");
