//https://adventofcode.com/2022/day/6
var input = await File.ReadAllLinesAsync("../input.txt");

var buffer = new Queue<char>(4);
int i = 0;

var marker = input
    .Select(x =>
    {
        i = 0;
        buffer.Clear();
        //part1
        // while (i < x.Length && buffer.Count < 4)
        //part2
        while (i < x.Length && buffer.Count < 14)
        {
            while(buffer.Contains(x[i]))
            {
                buffer.Dequeue();
            }
            buffer.Enqueue(x[i++]);
        }
        return i;
    })
    .ToList();

marker.ForEach(x => Console.Write(x));