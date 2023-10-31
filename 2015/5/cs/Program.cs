using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//https://adventofcode.com/2015/day/5
var input = await File.ReadAllLinesAsync("../input.txt");
var inputCount = input.Count();

var count = input.Where(x=>NaughtyNicePart1(x)).Count();
Console.WriteLine($"Input Count: {inputCount}, Part1 Nice: {count}");

count = input.Where(x=>NaughtyNicePart2(x)).Count();
Console.WriteLine($"Input Count: {inputCount}, Part2 Nice: {count}");

//return true if any of these conditions are met
//It contains at least three vowels (aeiou only), like aei, xazegov, or aeiouaeiouaeiou.
//It contains at least one letter that appears twice in a row, like xx, abcdde (dd), or aabbccdd (aa, bb, cc, or dd).
//It does not contain the strings ab, cd, pq, or xy, even if they are part of one of the other requirements.
bool NaughtyNicePart1(string input)
{
    int vowelCount = 0;
    bool hasDoubleLetter = false;
    char lastChar = '\0';

    foreach (char c in input)
    {
        if ("aeiou".Contains(c)) vowelCount++;
        if (c == lastChar) hasDoubleLetter = true;
        lastChar = c;
    }

    if (input.Contains("ab") || input.Contains("cd") || input.Contains("pq") || input.Contains("xy")) return false;

    return vowelCount >= 3 && hasDoubleLetter;
}

//return true if any of these conditions are met
//It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
//It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
bool NaughtyNicePart2(string input)
{
    bool hasRepeatedPair = false;
    bool hasRepeatedLetterWithOneInBetween = false;

    for (int i = 0; i < input.Length - 2; i++)
    {
        string pair = input.Substring(i, 2);
        if (input.IndexOf(pair, i + 2) != -1) hasRepeatedPair = true;
        if (input[i] == input[i + 2]) hasRepeatedLetterWithOneInBetween = true;
    }

    return hasRepeatedPair && hasRepeatedLetterWithOneInBetween;
}