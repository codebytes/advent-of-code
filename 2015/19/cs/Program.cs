//https://adventofcode.com/2015/day/19
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var sections = input.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
if (sections.Length < 2)
{
	throw new InvalidOperationException("Input must contain replacement rules and a target molecule.");
}

var replacementLines = sections[0].Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
var replacements = replacementLines
	.Select(line => line.Split(" => ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
	.Select(parts => parts.Length == 2
		? (From: parts[0], To: parts[1])
		: throw new InvalidOperationException($"Invalid replacement line: {string.Join(' ', parts)}"))
	.ToArray();

var molecule = sections[^1].Trim();


var part1 = ApplySteps(replacements, molecule, 1).Count;
Console.WriteLine("Part 1: " + part1);

var part2 = ReduceViaSearch(replacements, molecule);
Console.WriteLine("Part 2: " + part2);


static IReadOnlyCollection<string> ApplySteps((string From, string To)[] replacements, string start, int steps) =>
	Enumerable.Range(0, Math.Max(0, steps))
		.Aggregate(
			(HashSet<string>)new(StringComparer.Ordinal) { start },
			(current, _) => current
				.SelectMany(molecule => EnumerateSingleStep(replacements, molecule))
				.ToHashSet(StringComparer.Ordinal));

static IEnumerable<string> EnumerateSingleStep((string From, string To)[] replacements, string molecule) =>
	replacements.SelectMany(rule =>
		Enumerable
			.Range(0, Math.Max(0, molecule.Length - rule.From.Length + 1))
			.Where(index => string.CompareOrdinal(molecule, index, rule.From, 0, rule.From.Length) == 0)
			.Select(index => string.Concat(molecule.AsSpan(0, index), rule.To, molecule.AsSpan(index + rule.From.Length))));

static int ReduceViaSearch((string From, string To)[] replacements, string target)
{
	var reverse = replacements
		.Select(rule => (From: rule.To, To: rule.From))
		.ToArray();

	var queue = new PriorityQueue<(string Molecule, int Steps), int>();
	queue.Enqueue((target, 0), target.Length);
	var seen = new HashSet<string>(StringComparer.Ordinal) { target };

	while (queue.TryDequeue(out var state, out _))
	{
		if (state.Molecule == "e")
		{
			return state.Steps;
		}

		foreach (var next in EnumerateSingleStep(reverse, state.Molecule))
		{
			if (seen.Add(next))
			{
				queue.Enqueue((next, state.Steps + 1), next.Length);
			}
		}
	}

	throw new InvalidOperationException("Unable to reach 'e' from target molecule.");
}
