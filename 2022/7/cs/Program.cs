using System.Text.RegularExpressions;

//https://adventofcode.com/2022/day/7
var input = await File.ReadAllLinesAsync("../input.txt");

Regex cd = new Regex(@"\$ cd (?<path>.+)", RegexOptions.Compiled);
Regex dir = new Regex(@"dir (?<dir>.+)", RegexOptions.Compiled);
Regex file = new Regex(@"(?<size>\d+) (?<filename>.+)", RegexOptions.Compiled);

var rootNode = new Node{Name = "/", NodeType=NodeType.Directory};
var nodes = new List<Node>(){rootNode};
var currentNode = nodes.Find(x=>x.Name == "/");
foreach (var line in input)
{
	if(cd.IsMatch(line))
		{
			var match = cd.Match(line);
			var path = match.Groups["path"].Value;
			if(path == "..")
			{
				if(currentNode.ParentNode != null)
				{
					currentNode = currentNode.ParentNode;
				}
			}
			else if(path == "/")
		{
			currentNode = nodes.Find(x => x.Name == "/");
		}
		else 
		{
			var node = currentNode.ChildNodes.FirstOrDefault(x=>x.Name == path);
			if(node is null)
			{
				node = new Node{Name = path, NodeType = NodeType.Directory, ParentNode = currentNode};
				currentNode.ChildNodes.Add(node);
				nodes.Add(node);
			}
			else
			{
				currentNode = node;
			}
		}
	}
	else if(dir.IsMatch(line))
	{
		var match = dir.Match(line);
		var dirName = match.Groups["dir"].Value;
		var node = currentNode.ChildNodes.FirstOrDefault(x => x.Name == dirName);
		if (node is null)
		{
			node = new Node { Name = dirName, NodeType = NodeType.Directory, ParentNode = currentNode };
			currentNode.ChildNodes.Add(node);
			nodes.Add(node);
		}
	}
	else if (file.IsMatch(line))
	{
		var match = file.Match(line);
		var filename = match.Groups["filename"].Value;
		var size = int.Parse(match.Groups["size"].Value);
		var node = currentNode.ChildNodes.FirstOrDefault(x => x.Name == filename);
		if (node is null)
		{
			node = new Node { Name = filename, NodeType = NodeType.File, Size = size, ParentNode = currentNode };
			currentNode.ChildNodes.Add(node);
			nodes.Add(node);
		}
	}
}

//part 1
var dirs = nodes
	.Select(x=>new{x.Name, Type = x.NodeType, Size=x.GetTotalSize()})
	.Where(x=>x.Type == NodeType.Directory && x.Size <= 100000);

Console.WriteLine($"Sum of dirs {dirs.Sum(x=>x.Size)}");

//part 2
var totalSize = rootNode.GetTotalSize();
var remainingSize = 70000000 - totalSize;

var directoryToDelete = nodes
	.Select(x=>new{x.Name, Type = x.NodeType, Size=x.GetTotalSize()})
	.Where(x=>x.Type == NodeType.Directory && (x.Size + remainingSize) > 30000000)
	.OrderBy(x=>x.Size)
	.First();

Console.WriteLine($"Dir to Delete {directoryToDelete.Name} with size {directoryToDelete.Size}");

public enum NodeType
{
	File,
	Directory
};

public record Node
{
	public NodeType NodeType { get; set; }
	public int Size { get; set; }
	public Node ParentNode { get; set; } = null;
	public IList<Node> ChildNodes { get; } = new List<Node>();
	public string Name { get; set; }
	public int GetTotalSize()
	{
		return this.NodeType switch
		{
			NodeType.File => this.Size,
			NodeType.Directory => this.ChildNodes.Sum(x=>x.GetTotalSize())
		};
	}
}
