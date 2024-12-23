//https://adventofcode.com/2024/day/22
int N = 2000;
long DecModulus = 16777215; // dec (1 << 24) - 1

var input = (await File.ReadAllTextAsync("../input.txt"))
    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
    .Select(long.Parse)
    .ToArray();

long part1 = 0;
var seqNumbers = new Dictionary<(int, int, int, int), long>();

foreach (var num in input)
{
    var nums = GenerateNumbers(num).ToList();
    part1 += nums.Last();

    var diffs = CalculateDifferences(nums);

    var seen = new HashSet<(int, int, int, int)>();
    for (int i = 0; i < nums.Count - 4; i++)
    {
        var pat = (diffs[i], diffs[i + 1], diffs[i + 2], diffs[i + 3]);
        if (!seen.Contains(pat))
        {
            if (!seqNumbers.ContainsKey(pat))
            {
                seqNumbers[pat] = 0;
            }
            seqNumbers[pat] += nums[i + 4] % 10;
            seen.Add(pat);
        }
    }
}

Console.WriteLine(part1);
Console.WriteLine(seqNumbers.Values.Max());

IEnumerable<long> GenerateNumbers(long start)
{
    long current = start;
    yield return current;
    for (int i = 0; i < N; i++)
    {
        current = PseudoRandomGenerator(current);
        yield return current;
    }
}

List<int> CalculateDifferences(List<long> nums)
{
    return nums.Zip(nums.Skip(1), (a, b) => (int)(b % 10 - a % 10)).ToList();
}

long PseudoRandomGenerator(long x)
{
    x ^= (x << 6) & DecModulus;
    x ^= (x >> 5) & DecModulus;
    return x ^ (x << 11) & DecModulus;
}
