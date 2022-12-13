using System.Text.RegularExpressions;
using System.Text;

//https://adventofcode.com/2022/day/10
var input = await File.ReadAllLinesAsync("../input.txt");

var instructions = input.Select(x => x switch
	{
		"noop" => new Command(InstructionType.noop, 0),
		_ => new Command(InstructionType.addx, int.Parse(x.Substring(5)))
	});

var (clock, Xreg) = (1, 1);

var programHistory = new List<(int Clock, int X)>() { (clock, Xreg) };

foreach (var command in instructions)
{
	switch (command.InstructionType)
	{
		case InstructionType.noop:
			clock += 1;
			break;
		case InstructionType.addx:
			clock += 2;
			Xreg += command.V;
			break;
		default:
			break;
	}

	programHistory.Add((clock, Xreg));
}

var cycles = new int[] { 20, 60, 100, 140, 180, 220 };

var data =
	cycles.Select(x => x * GetStateAt(programHistory, x))
	.Sum();
	
var display = new StringBuilder(246);
var (width, height) = (40, 6);
for(int counter = 1; counter < width * height; counter++)
	{
		var state = GetStateAt(programHistory, counter) + 1;
		var lit = (state-1 <= (counter % width) && (counter % width) <= state + 1);
		display.Append(lit ? "#" : ".");
		if(counter % width == 0)
		{
			display.AppendLine();
		}
	}

Console.WriteLine($"Signal Strengths: {data}");
Console.WriteLine(display.ToString());

int GetStateAt(List<(int Clock, int X)> programState, int clock) => programState.Where(y => y.Clock <= clock).Last().X;

public enum InstructionType
{
	noop,
	addx
};
public record Command(InstructionType InstructionType, int V);