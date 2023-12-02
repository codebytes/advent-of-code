//https://adventofcode.com/2023/day/1
var input = await File.ReadAllLinesAsync("../input.txt");

var part1Digits = input
                .Select(x => (x.First(char.IsDigit), x.Last(char.IsDigit)));
var part2Digits = input
                .Select(x => x.Replace("one", "o1e"))
                .Select(x => x.Replace("two", "t2o"))
                .Select(x => x.Replace("three", "t3e"))
                .Select(x => x.Replace("four", "f4r"))
                .Select(x => x.Replace("five", "f5e"))
                .Select(x => x.Replace("six", "s6x"))
                .Select(x => x.Replace("seven", "s7n"))
                .Select(x => x.Replace("eight", "e8t"))
                .Select(x => x.Replace("nine", "n9e"))
                .Select(x => (x.First(char.IsDigit), x.Last(char.IsDigit)));

var part1Sum = part1Digits
                .Select(x => $"{x.Item1}{x.Item2}")
                .Select(int.Parse)
                .Sum();
Console.WriteLine($"Part1: {part1Sum}"); // 142

var part2Sum = part2Digits
                .Select(x => $"{x.Item1}{x.Item2}")
                .Select(int.Parse)
                .Sum();
Console.WriteLine($"Part2: {part2Sum}"); // 142

