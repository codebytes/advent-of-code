//https://adventofcode.com/2015/day/14
//var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var reindeers = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
    .Select(line => {
        var parts = line.Split(' ');
        return new Reindeer(
            parts[0],
            int.Parse(parts[3]),
            int.Parse(parts[6]),
            int.Parse(parts[13])
        );
    }).ToList();

int raceDuration = 2503;
var distances = new Dictionary<string, int>();
var points = reindeers.ToDictionary(reindeer => reindeer.Name, _ => 0);

for (int second = 1; second <= raceDuration; second++) {
    foreach (var reindeer in reindeers) {
        int cycleTime = reindeer.FlyTime + reindeer.RestTime;
        int distance = (second / cycleTime) * reindeer.Speed * reindeer.FlyTime +
                       Math.Min(second % cycleTime, reindeer.FlyTime) * reindeer.Speed;
        distances[reindeer.Name] = distance;
    }

    int maxDistance = distances.Values.Max();
    foreach (var reindeer in reindeers.Where(r => distances[r.Name] == maxDistance)) {
        points[reindeer.Name]++;
    }
}

var maxDistanceReindeer = distances.OrderByDescending(kvp => kvp.Value).First();
var maxPointsReindeer = points.OrderByDescending(kvp => kvp.Value).First();

Console.WriteLine($"{maxDistanceReindeer.Key} traveled the most distance: {maxDistanceReindeer.Value} km");
Console.WriteLine($"{maxPointsReindeer.Key} has the highest points: {maxPointsReindeer.Value}");

record Reindeer(string Name, int Speed, int FlyTime, int RestTime);
