//https://adventofcode.com/2024/day/17

var input = await File.ReadAllTextAsync("../input.txt");
// var input = await File.ReadAllTextAsync("../sample.txt");

var (_, registers, opCodesAndOperands) = ParseInput(input);
var result = string.Join(",", Execute(opCodesAndOperands, registers[0], registers[1], registers[2]));
Console.WriteLine($"Part 1: {result}");

Console.WriteLine($"Part 2: {Part2(input)}");

(string, long[], long[]) ParseInput(string rawInput)
{
    var parts = rawInput.Split("\n\n");
    var registers = parts[0].Split("\n").Select(line => long.Parse(line.Split(": ")[1])).ToArray();
    var opCodesAndOperands = parts[1].Split(": ")[1].Split(',').Select(long.Parse).ToArray();
    return (string.Join("", opCodesAndOperands), registers, opCodesAndOperands);
}

IEnumerable<long> Execute(long[] opCodesAndOperands, long a, long b, long c)
{
    var registers = new long[] { a, b, c };
    long ip = 0;
    var output = new List<long>();

    while (ip < opCodesAndOperands.Length)
    {
        var operatorCode = opCodesAndOperands[ip];
        var operand = opCodesAndOperands[ip + 1];

        switch (operatorCode)
        {
            case 0:
                registers[0] = registers[0] / (1L << (int) combo(registers, operand));
                break;
            case 1:
                registers[1] = registers[1] ^ operand;
                break;
            case 2:
                registers[1] = combo(registers, operand) % 8;
                break;
            case 3:
                if (registers[0] != 0)
                {
                    ip = operand - 2;
                }
                break;
            case 4:
                registers[1] = registers[1] ^ registers[2];
                break;
            case 5:
                output.Add(combo(registers, operand) % 8);
                break;
            case 6:
                registers[1] = registers[0] / (1 << (int) combo(registers, operand));
                break;
            case 7:
                registers[2] = registers[0] / (1 << (int) combo(registers, operand));
                break;
        }
        ip += 2;
    }

    return output;
}

string Part2(string rawInput)
{
    var (quineput, registers, opCodesAndOperands) = ParseInput(rawInput);
    var aInits = new List<long> { 0 };

    for (int i = 1; i <= quineput.Length; i++)
    {
        var currentQuineputWindow = string.Join(",", quineput.Substring(quineput.Length - i).Select(c => c.ToString()));
        var nextAInits = new List<long>();

        foreach (var init in aInits.ToList())
        {
            var aInit = init;
            for (int count = 0; count < 9; count++)
            {
                var output = string.Join(",", Execute(opCodesAndOperands, aInit, registers[1], registers[2]));

                if (currentQuineputWindow == output)
                {
                    if (i < quineput.Length)
                    {
                        nextAInits.Add(aInit << 3);
                    }
                    else
                    {
                        nextAInits.Add(aInit);
                    }
                }
                aInit++;
            }
        }

        aInits = nextAInits;
    }

    var answerStr = aInits[0].ToString();
    return answerStr.Substring(0, answerStr.Length);
}

long combo(long[] registers, long value) => value switch {
    >= 0 and <= 3 => value,
    var reg => registers[reg - 4],
};

enum OpCode
{
    adv = 0,
    bxl = 1,
    bst = 2,
    jnz = 3,
    bxc = 4,
    output = 5,
    bdv = 6,
    cdv = 7,
}
