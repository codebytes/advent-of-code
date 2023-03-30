using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//https://adventofcode.com/2015/day/3
var input = await File.ReadAllBytesAsync("../input.txt");

var part1Visits = new List<Coords>();
var part2Visits = new List<Coords>();
Coords part1Santa = new Coords(0, 0);
Coords part2Santa = new Coords(0, 0);
Coords part2RoboSanta = new Coords(0, 0);

part1Visits.Add(part1Santa);
part2Visits.Add(part2Santa);
part2Visits.Add(part2RoboSanta);
int count = 0;
foreach (char ch in input)
{
    part1Santa = NewCoordOnInput(part1Santa, ch);
	if(count++ % 2 == 0)
    {
        part2Santa = NewCoordOnInput(part2Santa, ch);
    	part2Visits.Add(part2Santa);
    }
	else
	{
		part2RoboSanta = NewCoordOnInput(part2RoboSanta, ch);
    	part2Visits.Add(part2RoboSanta);
	}
    part1Visits.Add(part1Santa);
}

//input count
Console.WriteLine("Part1: " + part1Visits.Count);

//unique count
Console.WriteLine("Part1 distinct: " + part1Visits.Distinct().Count());


//input count
Console.WriteLine("Part2: " + part2Visits.Count);

//unique count
Console.WriteLine("Part2 distinct: " + part2Visits.Distinct().Count());

static Coords NewCoordOnInput(Coords coord, char ch)
{
    return ch switch
    {
        '>' => new Coords(coord.x + 1, coord.y),
        '<' => new Coords(coord.x - 1, coord.y),
        '^' => new Coords(coord.x, coord.y + 1),
        'v' => new Coords(coord.x, coord.y - 1),
        _ => coord
    };
}

public record Coords(int x, int y);
