//https://adventofcode.com/2015/day/7
var input = await File.ReadAllLinesAsync("../input.txt");

var instructions = input
    .Select(i => i.Split(' '))
    .ToDictionary(i => i.Last());
var result1 = processSignal("a", instructions);
Console.WriteLine(result1);

instructions = input
    .Select(i => i.Split(' '))
    .ToDictionary(i => i.Last());
instructions["b"] = ["3176", "->", "b"];
var result2 = processSignal("a", instructions);
Console.WriteLine(result2);

ushort processSignal(string input, Dictionary<string, string[]> instructions)
{
    ushort Eval(string x) => char.IsLetter(x[0]) 
        ? processSignal(x, instructions) 
        : ushort.Parse(x);
    ushort AssignOp(string[] x) => Eval(x[0]);
    ushort AndOp(string[] x) => (ushort)(Eval(x[0]) & Eval(x[2]));
    ushort OrOp(string[] x) => (ushort)(Eval(x[0]) | Eval(x[2]));
    ushort LshiftOp(string[] x) => (ushort)(Eval(x[0]) << Eval(x[2]));
    ushort RshiftOp(string[] x) => (ushort)(Eval(x[0]) >> Eval(x[2]));
    ushort NotOp(string[] x) => (ushort)~Eval(x[1]);

    var ins = instructions[input];
    ushort value;
    value = ins[1] switch
    {
        "->" => AssignOp(ins),
        "AND" => AndOp(ins),
        "OR" => OrOp(ins),
        "LSHIFT" => LshiftOp(ins),
        "RSHIFT" => RshiftOp(ins),
        _ when ins[0] == "NOT" => NotOp(ins),
        _ => throw new InvalidDataException("Unrecognised command")
    };

    instructions[input] = [value.ToString(), "->", input];
    return value;
}