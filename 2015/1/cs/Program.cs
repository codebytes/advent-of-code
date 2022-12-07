//https://adventofcode.com/2015/day/1
var input = await File.ReadAllLinesAsync("../input.txt");
var directions = input.First();

int floor = 0;
bool inBasement = false;
for (int i = 0; i < directions.Length; i++)
{
    floor += directions[i] switch
    {
        '(' => 1,
        ')' => -1,
        _ => 0
    };
    if(floor < 0)
    {
        if(!inBasement)
        {
            Console.WriteLine($"First time in basement at step: {i+1}");
            inBasement = true;
        }
    }
}

Console.WriteLine(floor);