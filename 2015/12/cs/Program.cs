//https://adventofcode.com/2015/day/12
using System.Text.Json;

var input = await File.ReadAllLinesAsync("../input.txt");

var total = 0;
foreach (var line in input)
{
    var sum = SumNumbers(line);
    Console.WriteLine($"{sum}");
    total += sum;
}

Console.WriteLine($"Total: {total}");

total = 0;
foreach (var line in input)
{
    var sum = SumNumbers(line, "red");
    Console.WriteLine($"{sum}");
    total += sum;
}

Console.WriteLine($"Total ignoring red: {total}");

int SumNumbers(string input, string? ignoreProperty = null)
{
    using JsonDocument document = JsonDocument.Parse(input);
    return SumElement(document.RootElement, ignoreProperty);
}

int SumElement(JsonElement element, string? ignoreValue)
{
    int sum = 0;

    switch (element.ValueKind)
    {
        case JsonValueKind.Object:
            foreach (JsonProperty property in element.EnumerateObject())
            {
                if (ignoreValue != null && property.Value.ValueKind == JsonValueKind.String && property.Value.GetString() == ignoreValue)
                {
                    return 0;
                }
                sum += SumElement(property.Value, ignoreValue);
            }
            break;

        case JsonValueKind.Array:
            foreach (JsonElement item in element.EnumerateArray())
            {
                sum += SumElement(item, ignoreValue);
            }
            break;

        case JsonValueKind.Number:
            sum += element.GetInt32();
            break;
    }

    return sum;
}