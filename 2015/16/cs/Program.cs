//https://adventofcode.com/2015/day/16
var input = await File.ReadAllTextAsync("../input.txt");

var tickerTape = new Dictionary<string, int>
{
    { "children", 3 },
    { "cats", 7 },
    { "samoyeds", 2 },
    { "pomeranians", 3 },
    { "akitas", 0 },
    { "vizslas", 0 },
    { "goldfish", 5 },
    { "trees", 3 },
    { "cars", 2 },
    { "perfumes", 1 }
};

var aunts = input.Split('\n')
    .Select(line => line.Trim())
    .Where(line => !string.IsNullOrEmpty(line))
    .Select(line => line.Split(new[] { ": ", ", " }, StringSplitOptions.None))
    .Where(parts => parts.Length > 2) // Ensure there are enough parts to parse
    .Select(parts => new
    {
        Number = int.Parse(parts[0].Split(' ')[1]),
        Attributes = parts.Skip(1).Select((value, index) => new { value, index })
            .GroupBy(x => x.index / 2)
            .ToDictionary(g => g.First().value, g => int.Parse(g.Last().value))
    })
    .ToList();

bool MatchesAttributesPart1(Dictionary<string, int> attributes)
{
    return attributes.All(attr => tickerTape
        .ContainsKey(attr.Key) && tickerTape[attr.Key] == attr.Value);
}

bool MatchesAttributesPart2(Dictionary<string, int> attributes)
{
    foreach (var attr in attributes)
    {
        if (tickerTape.ContainsKey(attr.Key))
        {
            var value = tickerTape[attr.Key];
            if (attr.Key switch
            {
                "cats" => value >= attr.Value,
                "trees" => value >= attr.Value,
                "pomeranians" => value <= attr.Value,
                "goldfish" => value <= attr.Value,
                _ => value != attr.Value
            })
            {
                return false;
            }
        }
    }
    return true;
}

var matchingAuntPart1 = aunts.FirstOrDefault(aunt => MatchesAttributesPart1(aunt.Attributes));
var matchingAuntPart2 = aunts.FirstOrDefault(aunt => MatchesAttributesPart2(aunt.Attributes));

Console.WriteLine($"Part 1: Aunt Sue #{matchingAuntPart1?.Number} got you the gift.");
Console.WriteLine($"Part 2: Aunt Sue #{matchingAuntPart2?.Number} got you the gift.");
