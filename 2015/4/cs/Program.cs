using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

//https://adventofcode.com/2015/day/4
var input = await File.ReadAllTextAsync("../input.txt");

var md5 = System.Security.Cryptography.MD5.Create();

Console.WriteLine($"secret key: {input}");
bool foundFive = false;
foreach(var number in Enumerable.Range(1,10000000))
{
    var inputVal = $"{input}{number}";
    var inputBytes = System.Text.Encoding.ASCII.GetBytes(inputVal);
    var hashBytes = md5.ComputeHash(inputBytes);
    var hashString = BitConverter.ToString(hashBytes).Replace("-", "");
    if(!foundFive && hashString.StartsWith("00000"))
    {
        foundFive = true;
        Console.WriteLine($"number: {number}, hashString: {hashString}");
        continue;
    }
    if(hashString.StartsWith("000000"))
    {
        Console.WriteLine($"number: {number}, hashString: {hashString}");
        break;
    }
}