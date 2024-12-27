//https://adventofcode.com/2024/day/24
var input = await File.ReadAllTextAsync("../input.txt");

var wires = new Dictionary<string, int>();
var operations = new List<(string op1, string op, string op2, string res)>();

string highestZ = "z00";
var inputData = await File.ReadAllLinesAsync("../input.txt");

foreach (var line in inputData)
{
    switch (line)
    {
        case var l when l.Contains(":"):
            var parts1 = l.Split(": ");
            wires[parts1[0]] = int.Parse(parts1[1]);
            break;
        case var l when l.Contains("->"):
            var parts2 = l.Split(" ");
            operations.Add((parts2[0], parts2[1], parts2[2], parts2[4]));
            if (parts2[4].StartsWith("z") && int.Parse(parts2[4][1..]) > int.Parse(highestZ[1..]))
            {
                highestZ = parts2[4];
            }
            break;
    }
}

var wrong = new HashSet<string>();

IdentifyWrongOperations();
ProcessOperations();

var resultBits = wires.Where(kvp => kvp.Key.StartsWith("z")).OrderByDescending(kvp => kvp.Key).Select(kvp => kvp.Value.ToString()).ToArray();
Console.WriteLine(Convert.ToUInt64(string.Join("", resultBits), 2));
Console.WriteLine(string.Join(",", wrong.OrderBy(x => x)));

void IdentifyWrongOperations()
{
    foreach (var (op1, op, op2, res) in operations)
    {
        if (res.StartsWith("z") && op != "XOR" && res != highestZ)
        {
            wrong.Add(res);
        }
        if (op == "XOR" && !new HashSet<char> { 'x', 'y', 'z' }.Contains(res[0]) && !new HashSet<char> { 'x', 'y', 'z' }.Contains(op1[0]) && !new HashSet<char> { 'x', 'y', 'z' }.Contains(op2[0]))
        {
            wrong.Add(res);
        }
        if (op == "AND" && !new HashSet<string> { "x00", "y00" }.Contains(op1) && !new HashSet<string> { "x00", "y00" }.Contains(op2))
        {
            foreach (var (subop1, subop, subop2, subres) in operations)
            {
                if ((res == subop1 || res == subop2) && subop != "OR")
                {
                    wrong.Add(res);
                }
            }
        }
        if (op == "XOR")
        {
            foreach (var (subop1, subop, subop2, subres) in operations)
            {
                if ((res == subop1 || res == subop2) && subop == "OR")
                {
                    wrong.Add(res);
                }
            }
        }
    }
}

void ProcessOperations()
{
    while (operations.Count > 0)
    {
        var (op1, op, op2, res) = operations[0];
        operations.RemoveAt(0);
        if (wires.ContainsKey(op1) && wires.ContainsKey(op2))
        {
            wires[res] = op switch
            {
                "AND" => wires[op1] & wires[op2],
                "OR" => wires[op1] | wires[op2],
                "XOR" => wires[op1] ^ wires[op2],
                _ => throw new InvalidOperationException("Unexpected operation")
            };
        }
        else
        {
            operations.Add((op1, op, op2, res));
        }
    }
}
