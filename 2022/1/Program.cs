var input = await File.ReadAllLinesAsync("input.txt");

var groups = input
    .Split((prev,next) => next == "")
    .Select(x=>x
        .Where(y=>y!="")
        .Select(y=>int.Parse(y)))
    .Select(x=>x.Sum());

//part1
Console.WriteLine($"Max: {groups.Max()}");

//part2
var top3 = groups.OrderByDescending(x=>x).Take(3).Sum();
Console.WriteLine($"Top3: {top3}");


public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Func<T, T, bool> splitOn)
    {
        using (var e = source.GetEnumerator())
        {
            for (bool more = e.MoveNext(); more; )
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