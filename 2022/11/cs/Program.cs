//https://adventofcode.com/2022/day/11
var input = await File.ReadAllLinesAsync("../input.txt");

var monkeys = input
 .Split((prev, next) => next == "")
 .Select(x => x
        .Where(y => y != "")
        .ToArray())
 .Select((x, index) => new Monkey(index, x))
 .ToList();

var monkeysPart2 = new List<Monkey>(monkeys.Select(x=>(Monkey)x.Clone()));

var part1Reducer = (Int128 x) => x/3;

Console.WriteLine($"Part1 Monkey Business {MonkeyBusiness(20, monkeys, part1Reducer)}");

//was dealing with overflow and/or slow operation with BigInteger
int reducer = monkeys.Select(x => x.divisor).Aggregate((int)1, (x, y) => x * y);
var part2Reducer = (Int128 x) => x % reducer;

Console.WriteLine($"Part2 Monkey Business {MonkeyBusiness(10000, monkeysPart2, part2Reducer)}");

Int128 MonkeyBusiness(int loops, List<Monkey> monkeys, Func<Int128, Int128> worryReducer)
{
    var display = new List<int> { 1 , 20, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };
    for (int i = 0; i < loops; i++)
    {
        foreach (var monkey in monkeys)
        {
            monkey.InspectionCount += monkey.Items.Count;
            foreach (var item in monkey.Items)
            {
                var updatedWorry = PerformOperation(item, monkey);
                var reducedWorry = worryReducer(updatedWorry);
                var recieverMonkey = reducedWorry % (int)monkey.divisor == 0
                ? monkey.trueMonkey
                : monkey.falseMonkey;
                monkeys[recieverMonkey].Items.Add(reducedWorry);
            }
            monkey.Items.Clear();
        }
        if (display.Contains(i + 1))
        {
            Console.WriteLine($"== After round {i + 1} ==");
            monkeys.ForEach(x =>
            Console.WriteLine($"Monkey {x.Number} inspected items {x.InspectionCount} times"));
        }
    }

    return monkeys.OrderByDescending(x => x.InspectionCount).Select(x => x.InspectionCount).Take(2).Aggregate((Int128)1, (x, y) => x * y);
}

Int128 PerformOperation(Int128 item, Monkey m)
{
    Int128 value = m.Operation.value == -1 
		? item
		: m.Operation.value;
	return m.Operation.op switch {
		Op.Add => item + value,
		Op.Mult => item * value,
		_ => item
	};
}

public class Monkey : ICloneable
{
    public int Number { get; set; }
    public List<Int128> Items { get; set; } =  new List<Int128>();
    public Operation Operation { get; set; } = new Operation(Op.Add, 0);
    public int divisor { get; set; }
    public int trueMonkey { get; set; }
    public int falseMonkey { get; set; }
    public int InspectionCount { get; set; }
	public Monkey()
	{
	}
   
    public Monkey(int Number, string[] inputs)
    {

        this.Number = Number;
        Items = new List<Int128>(inputs[1].Substring(18).Split(", ").Select(x => Int128.Parse(x)));
        var op = Op.Add;
        switch (inputs[2][23])
        {
            case '+':
                op = Op.Add;
                break;
            case '*':
                op = Op.Mult;
                break;

        }
        var valueString = inputs[2].Substring(25);
        var value = valueString == "old" ? -1 : int.Parse(valueString);
        Operation = new Operation(op, value);
        divisor = int.Parse(inputs[3].Substring(21));
        trueMonkey = int.Parse(inputs[4].Substring(29));
        falseMonkey = int.Parse(inputs[5].Substring(30));
    }

    public object Clone()
    {
        return new Monkey
        {
            Number = this.Number,
			Items = new List<Int128>(this.Items),
    		Operation = this.Operation,
    		divisor = this.divisor,
    		trueMonkey = this.trueMonkey,
    		falseMonkey = this.falseMonkey,
    		InspectionCount = this.InspectionCount
        };
    }
}

public enum Op
{
    Add,
    Mult
};

public record Operation(Op op, int value);

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Func<T, T, bool> splitOn)
    {
        using (var e = source.GetEnumerator())
        {
            for (bool more = e.MoveNext(); more;)
            {
                var last = e.Current;
                var group = new List<T> { last };
                while ((more = e.MoveNext()) && !splitOn(last, e.Current))
                    group.Add(last = e.Current);
                yield return group;
            }
        }
    }
}

