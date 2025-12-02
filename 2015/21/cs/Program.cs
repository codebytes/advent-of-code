//https://adventofcode.com/2015/day/21
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var stats = ParseBossStats(input);
const int playerHitPoints = 100;

var weapons = new[]
{
	new Item(8, 4, 0),
	new Item(10, 5, 0),
	new Item(25, 6, 0),
	new Item(40, 7, 0),
	new Item(74, 8, 0)
};

var armors = new[]
{
	Item.None,
	new Item(13, 0, 1),
	new Item(31, 0, 2),
	new Item(53, 0, 3),
	new Item(75, 0, 4),
	new Item(102, 0, 5)
};

var rings = new[]
{
	new Item(25, 1, 0),
	new Item(50, 2, 0),
	new Item(100, 3, 0),
	new Item(20, 0, 1),
	new Item(40, 0, 2),
	new Item(80, 0, 3)
};

var ringCombos = BuildRingCombos(rings).ToArray();

var outcomes = weapons
	.SelectMany(weapon => armors, (weapon, armor) => weapon + armor)
	.SelectMany(gear => ringCombos, (gear, ring) => gear + ring)
	.Select(gear => new
	{
		gear.Cost,
		Wins = PlayerWins(playerHitPoints, gear.Damage, gear.Armor, stats)
	});

var minWinningCost = outcomes.Where(o => o.Wins).Min(o => o.Cost);
var maxLosingCost = outcomes.Where(o => !o.Wins).Max(o => o.Cost);

System.Console.WriteLine($"Part 1: {minWinningCost}");
System.Console.WriteLine($"Part 2: {maxLosingCost}");

static (int HitPoints, int Damage, int Armor) ParseBossStats(string raw)
{
	var stats = raw
		.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries)
		.Select(line => line.Split(':', System.StringSplitOptions.TrimEntries))
		.Where(parts => parts.Length == 2)
		.ToDictionary(parts => parts[0], parts => int.Parse(parts[1]), System.StringComparer.Ordinal);

	return (stats["Hit Points"], stats["Damage"], stats["Armor"]);
}

static bool PlayerWins(int playerHitPoints, int playerDamage, int playerArmor, (int HitPoints, int Damage, int Armor) boss)
{
	var damageToBoss = System.Math.Max(1, playerDamage - boss.Armor);
	var damageToPlayer = System.Math.Max(1, boss.Damage - playerArmor);

	var turnsToKillBoss = (boss.HitPoints + damageToBoss - 1) / damageToBoss;
	var turnsToKillPlayer = (playerHitPoints + damageToPlayer - 1) / damageToPlayer;

	return turnsToKillBoss <= turnsToKillPlayer;
}

static IEnumerable<Item> BuildRingCombos(Item[] available) =>
	Enumerable.Repeat(Item.None, 1)
		.Concat(available)
		.Concat(
			available.SelectMany((first, i) => available
				.Skip(i + 1)
				.Select(second => first + second)));

readonly record struct Item(int Cost, int Damage, int Armor)
{
	public static Item None => new(0, 0, 0);

	public static Item operator +(Item left, Item right) =>
		new(left.Cost + right.Cost, left.Damage + right.Damage, left.Armor + right.Armor);
}
