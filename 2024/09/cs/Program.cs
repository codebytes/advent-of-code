// https://adventofcode.com/2024/day/9
var input = await File.ReadAllTextAsync("../input.txt");

List<long> CompactDisk(string diskMap, bool usePart1Strategy = false)
{
    var blocks = ParseDiskMap(diskMap, out var files);
    if (usePart1Strategy)
        MoveBlocksOneAtATime(blocks);
    else
        MoveFilesToFreeSpaces(blocks, files);
    return blocks;
}

List<long> ParseDiskMap(string diskMap, out List<(long id, int start, int length)> files)
{
    var blocks = new List<long>();
    long fileId = 0;
    files = new List<(long id, int start, int length)>();

    for (int i = 0, index = 0; i < diskMap.Length; i++)
    {
        int length = diskMap[i] - '0';
        if (length == 0) continue;

        if (i % 2 == 0)
        {
            files.Add((fileId++, index, length));
            blocks.AddRange(Enumerable.Repeat(files[^1].id, length));
        }
        else
        {
            blocks.AddRange(Enumerable.Repeat(-1L, length));
        }
        index = blocks.Count;
    }
    return blocks;
}

void MoveFilesToFreeSpaces(List<long> blocks, List<(long id, int start, int length)> files)
{
    var freeSpaces = GetFreeSpaces(blocks);
    foreach (var file in files.OrderByDescending(f => f.id))
    {
        var (found, freeStartIdx) = FindSuitableSpace(freeSpaces, file.start, file.length);
        if (found)
        {
            MoveFile(blocks, file.start, file.length, freeStartIdx);
            UpdateFreeSpaces(freeSpaces, file.start, file.length, freeStartIdx);
        }
    }
}

(bool found, int start) FindSuitableSpace(List<(int start, int length)> freeSpaces, int fileStart, int fileLength)
{
    foreach (var space in freeSpaces)
    {
        if (space.start + space.length <= fileStart && space.length >= fileLength)
            return (true, space.start);
    }
    return (false, -1);
}

List<(int start, int length)> GetFreeSpaces(List<long> blocks)
{
    var freeSpaces = new List<(int start, int length)>();
    int freeStart = -1;

    for (int i = 0; i < blocks.Count; i++)
    {
        if (blocks[i] == -1)
        {
            if (freeStart == -1) freeStart = i;
        }
        else
        {
            if (freeStart != -1)
            {
                freeSpaces.Add((freeStart, i - freeStart));
                freeStart = -1;
            }
        }
    }
    if (freeStart != -1)
        freeSpaces.Add((freeStart, blocks.Count - freeStart));

    freeSpaces.Sort((a, b) => a.start.CompareTo(b.start));
    return freeSpaces;
}

void MoveFile(List<long> blocks, int fileStart, int fileLength, int freeStartIdx)
{
    for (int i = 0; i < fileLength; i++)
    {
        blocks[freeStartIdx + i] = blocks[fileStart + i];
        blocks[fileStart + i] = -1;
    }
}

void UpdateFreeSpaces(List<(int start, int length)> freeSpaces, int fileStart, int fileLength, int freeStartIdx)
{
    var suitableSpace = freeSpaces.First(space => space.start == freeStartIdx);
    if (suitableSpace.length == fileLength)
        freeSpaces.Remove(suitableSpace);
    else
        freeSpaces[freeSpaces.IndexOf(suitableSpace)] = (suitableSpace.start + fileLength, suitableSpace.length - fileLength);

    freeSpaces.Add((fileStart, fileLength));
    freeSpaces.Sort((a, b) => a.start.CompareTo(b.start));
}

void MoveBlocksOneAtATime(List<long> blocks)
{
    int freeIndex = blocks.IndexOf(-1);
    int lastFileIndex = blocks.Count - 1;

    while (freeIndex != -1 && lastFileIndex > freeIndex)
    {
        while (lastFileIndex > freeIndex && blocks[lastFileIndex] == -1)
            lastFileIndex--;
        if (lastFileIndex <= freeIndex) break;

        blocks[freeIndex] = blocks[lastFileIndex];
        blocks[lastFileIndex--] = -1;
        freeIndex = blocks.IndexOf(-1, freeIndex + 1);
    }
}

long CalculateChecksum(List<long> blocks)
{
    return blocks.Select((fileId, index) => fileId != -1 ? index * fileId : 0L).Sum();
}

// Part 1
var blocksPart1 = CompactDisk(input, true);
var checksumPart1 = CalculateChecksum(blocksPart1);
Console.WriteLine($"Checksum Part 1: {checksumPart1}");

// Part 2
var blocksPart2 = CompactDisk(input, false);
var checksumPart2 = CalculateChecksum(blocksPart2);
Console.WriteLine($"Checksum Part 2: {checksumPart2}");
