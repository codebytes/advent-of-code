//https://adventofcode.com/2015/day/11
var input = await File.ReadAllLinesAsync("../input.txt");

char[] invalidChars = { 'i', 'o', 'l' };
var password = input[0];

do
{
    password = GetNextPassword(password);
} while (!IsValidPassword(password));
Console.WriteLine(password);

do
{
    password = GetNextPassword(password);
} while (!IsValidPassword(password));
Console.WriteLine(password);


string GetNextPassword(string password)
{
    var chars = password.ToCharArray();
    int index = chars.Length - 1;

    while (index >= 0)
    {
        if (chars[index] == 'z')
        {
            chars[index] = 'a';
            index--;
        }
        else
        {
            chars[index]++;
            if (invalidChars.Contains(chars[index]))
            {
                chars[index]++;
            }
            break;
        }
    }

    for (int i = index + 1; i < chars.Length; i++)
    {
        chars[i] = 'a';
    }

    return new string(chars);
}

bool IsValidPassword(string password)
{
    if (password.IndexOfAny(invalidChars) != -1)
    {
        return false;
    }

    int pairs = CountPasswordPairs(password);
    bool hasStraight = ContainsStraightSequence(password);

    return hasStraight && pairs >= 2;
}

int CountPasswordPairs(string password)
{
    int pairs = 0;
    char? lastPairChar = null;

    for (int i = 0; i < password.Length - 1; i++)
    {
        var pair = password.AsSpan(i, 2);
        if (pair[0] == pair[1] && pair[0] != lastPairChar)
        {
            pairs++;
            lastPairChar = pair[0];
            i++; // Skip next character to avoid overlapping pairs
        }
    }

    return pairs;
}

bool ContainsStraightSequence(string password)
{
    for (int i = 0; i < password.Length - 2; i++)
    {
        var slice = password.AsSpan(i, 3);
        if (slice[1] == slice[0] + 1 && slice[2] == slice[1] + 1)
        {
            return true;
        }
    }
    return false;
}