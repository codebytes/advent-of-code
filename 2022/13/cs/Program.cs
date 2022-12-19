//https://adventofcode.com/2022/day/13
var input = await File.ReadAllLinesAsync("../input.txt");

var packets = new List<Packet>();

for (int i = 0; i < input.Length - 1; i++)
{
	var lPacket = new Packet(input[i++]);
	var rPacket = new Packet(input[i++]);

	packets.Add(lPacket);
	packets.Add(rPacket);
}

var correct = packets
    .Chunk(2)
	.Select((x,i)=>(item: (lPacket: x.First(), rPacket: x.Last()), index:i))
	.Where(x=>x.item.lPacket.Compare(x.item.lPacket, x.item.rPacket) < 0)
	.Sum(x => x.index + 1);

Console.WriteLine($"Index Sum: {correct}");

packets.Add(new Packet("[[2]]"));
packets.Add(new Packet("[[6]]"));

packets.Sort((x,y)=>x.Compare(x,y));
var twoIndex = packets.FindIndex(x => x.Value == -1 && x.EmbeddedPackets.Count() == 1 && x.EmbeddedPackets.First().Value == 2) + 1;
var sixIndex = packets.FindIndex(x => x.Value == -1 && x.EmbeddedPackets.Count() == 1 && x.EmbeddedPackets.First().Value == 6) + 1;
Console.WriteLine($"Decoder Key: {twoIndex * sixIndex}");

public class Packet : IComparer<Packet>
{
	public Packet()
	{
		Value = -1;
		EmbeddedPackets = new List<Packet>();
	}

	public Packet(string data)
	{
		var parseString = data;
		if (parseString[0] == '[')
		{
			parseString = parseString[1..];
		}
		if (parseString[^1] == ']')
		{
			parseString = parseString[..^1];
		}

		int value = 0;
		if(int.TryParse(parseString, out value))
		{
			this.Value = value;
		}
		else if(!string.IsNullOrEmpty(parseString))
		{
			var values = new List<string>();
			string currentStr = "";
			int nesting = 0;
			
			foreach(var ch in parseString)
			{
				if(ch == '[')
				{
					nesting++;
				}
				else if(ch == ']')
				{
					nesting--;
				}
				if(nesting == 0 && ch == ',') 
				{
					values.Add(currentStr);
					currentStr = "";
				} 
				else 
				{
					currentStr += ch;
				}
			}
			
			values.Add(currentStr);
			
			foreach(var str in values)
			{
				this.EmbeddedPackets.Add(new Packet(str));	
			}
		}
	}
	
	public int Value { get; set; } = -1;
	public List<Packet> EmbeddedPackets { get; set; } = new List<Packet>();

	public int Compare(Packet? x, Packet? y)
	{
		bool xHasPackets = x?.EmbeddedPackets.Any() ?? false || x?.Value == -1;
		bool yHasPackets = y?.EmbeddedPackets.Any() ?? false || y?.Value == -1;
		if(xHasPackets && yHasPackets)
		{
			int xSize = x?.EmbeddedPackets.Count() ?? 0;
			int ySize = y?.EmbeddedPackets.Count() ?? 0;
			
			int minSize = Math.Min(xSize, ySize);
			for(int i = 0; i<minSize; i++)
			{
				var result = this.Compare(x?.EmbeddedPackets[i], y?.EmbeddedPackets[i]);
				if(result != 0)
				{
					return result;
				}
			}
			
            return (xSize, ySize) switch {
                var (a, b) when a < b => -1,
                var (a, b) when a > b => 1,
                _ => 0
            };
		}
		else if (!xHasPackets && !yHasPackets)
		{
               return (x?.Value, y?.Value) switch {
                var (a, b) when a < b => -1,
                var (a, b) when a > b => 1,
                _ => 0
            };
		} 
		else if(xHasPackets)
		{
			Packet newYPacket = new Packet();
			if(y is not null)
            {
                newYPacket.EmbeddedPackets.Add(y);
            }
			return this.Compare(x, newYPacket);
		}
		else
		{
			Packet newXPacket= new Packet();
			if(x is not null)
            {
                newXPacket.EmbeddedPackets.Add(x);
            }
			return this.Compare(newXPacket, y);
		}
	}
}
