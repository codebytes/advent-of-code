using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//https://adventofcode.com/2015/day/6
var input = await File.ReadAllLinesAsync("../input.txt");
var inputCount = input.Count();

var lights = new bool[1000, 1000];
var lights2 = new int[1000, 1000];
foreach (var line in input)
{
    var parts = line.Split(" ");
    var start = parts[^3].Split(",").Select(int.Parse).ToArray();
    var end = parts[^1].Split(",").Select(int.Parse).ToArray();
    for (int i = start[0]; i <= end[0]; i++)
    {
        for (int j = start[1]; j <= end[1]; j++)
        {
            lights[i, j] = parts[1] switch
            {
                "on" => true,
                "off" => false,
                _ => !lights[i, j]
            };
            lights2[i, j] = parts[1] switch
            {
                "on" => lights2[i, j] + 1,
                "off" => lights2[i, j] == 0 ? 0 : lights2[i, j] - 1,
                _ => lights2[i, j] + 2
            };
        }
    }
}

var count = lights.Cast<bool>().Count(x => x);
Console.WriteLine($"Part 1: {count}");

var count2 = lights2.Cast<int>().Sum();
Console.WriteLine($"Part 2: {count2}");
