//https://adventofcode.com/2022/day/2
var input = await File.ReadAllLinesAsync("input.txt");

//part1
var rounds = input
                .Select(x => x.Split(" "))
				.Select(x=>new Round(x[0],x[1]));
Console.WriteLine($"Score: {rounds.Sum(x=>x.Score)}");


//part2
var rounds2 = input
                .Select(x => x.Split(" "))
				.Select(x=>new Round2(x[0],x[1]));

Console.WriteLine($"Score: {rounds2.Sum(x=>x.Score)}");

public enum Shape 
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
};

public enum Outcome
{
    Win = 6,
    Lose = 0,
    Draw = 3
};

public class Round
{
	public Round(string opponent, string shape)
	{
		this.Opponent = ParseShape(opponent);
		this.Shape = ParseShape(shape);
		
		this.Outcome = this switch
		{
			{ Shape: Shape.Rock, Opponent: Shape.Rock } => Outcome.Draw,
			{ Shape: Shape.Paper, Opponent: Shape.Paper } => Outcome.Draw,
			{ Shape: Shape.Scissors, Opponent: Shape.Scissors } => Outcome.Draw,
			{ Shape: Shape.Rock, Opponent: Shape.Scissors } => Outcome.Win,
			{ Shape: Shape.Paper, Opponent: Shape.Rock } => Outcome.Win,
			{ Shape: Shape.Scissors, Opponent: Shape.Paper} => Outcome.Win,
			_ => Outcome.Lose
		};
	}
	public Shape Opponent {get;init;}
	public Shape Shape { get; init; }
	public Outcome Outcome { get; init; }
	public int Score => (int)this.Shape + (int)this.Outcome;

	public Shape ParseShape(string input) =>
		input switch
		{
			"A" => Shape.Rock,
			"X" => Shape.Rock,
			"B" => Shape.Paper,
			"Y" => Shape.Paper,
			"C" => Shape.Scissors,
			"Z" => Shape.Scissors,
			_ => Shape.Scissors
		};
};


public class Round2
{
	public Round2(string opponent, string outcome)
	{
		this.Opponent = ParseShape(opponent);
        this.Outcome = ParseOutcome(outcome);
		this.Shape = this switch
		{
			{ Outcome: Outcome.Draw, Opponent: Shape.Rock } => Shape.Rock,
			{ Outcome: Outcome.Win, Opponent: Shape.Scissors } => Shape.Rock,
			{ Outcome: Outcome.Lose, Opponent: Shape.Paper } => Shape.Rock,
			{ Outcome: Outcome.Win, Opponent: Shape.Rock } => Shape.Paper,
			{ Outcome: Outcome.Lose, Opponent: Shape.Scissors } => Shape.Paper,
			{ Outcome: Outcome.Draw, Opponent: Shape.Paper } => Shape.Paper,
			_ => Shape.Scissors
		};
	}
	public Shape Opponent {get;init;}
	public Shape Shape { get; init; }
	public Outcome Outcome { get; init; }
	public int Score => (int)this.Shape + (int)this.Outcome;

	public Shape ParseShape(string input) =>
		input switch
		{
			"A" => Shape.Rock,
			"B" => Shape.Paper,
			"C" => Shape.Scissors,
			_ => Shape.Scissors
		};
	public Outcome ParseOutcome(string input) =>
		input switch
		{
			"X" => Outcome.Lose,
			"Y" => Outcome.Draw,
			"Z" => Outcome.Win,
			_ => Outcome.Win
		};
};
