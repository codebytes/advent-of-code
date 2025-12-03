//https://adventofcode.com/2015/day/22
// var input = await File.ReadAllTextAsync("../sample.txt");
var input = await File.ReadAllTextAsync("../input.txt");

var stats = input
	.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
	.Select(line => line.Split(':', StringSplitOptions.TrimEntries))
	.ToDictionary(parts => parts[0], parts => int.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture));

var playerHp = 50;
var playerMana = 500;
var bossHp = stats["Hit Points"];
var bossDamage = stats["Damage"];

Console.WriteLine(FindLeastManaToWin(playerHp, playerMana, bossHp, bossDamage, hardMode: false));
Console.WriteLine(FindLeastManaToWin(playerHp, playerMana, bossHp, bossDamage, hardMode: true));

static int FindLeastManaToWin(int playerHp, int playerMana, int bossHp, int bossDamage, bool hardMode)
{
	var best = int.MaxValue;
	var stack = new Stack<FightState>();
	stack.Push(new(playerHp, playerMana, bossHp, bossDamage, 0, 0, 0, 0, true));

	while (stack.TryPop(out var state))
	{
		if (state.ManaSpent >= best)
		{
			continue;
		}

		if (hardMode && state.PlayerTurn)
		{
			state = state with { PlayerHp = state.PlayerHp - 1 };
			if (state.PlayerHp <= 0)
			{
				continue;
			}
		}

		state = state.ApplyEffects();

		if (state.BossHp <= 0)
		{
			best = System.Math.Min(best, state.ManaSpent);
			continue;
		}

		if (state.PlayerTurn)
		{
			foreach (var next in EnumeratePlayerMoves(state))
			{
				if (next.ManaSpent < best)
				{
					stack.Push(next);
				}
			}
		}
		else
		{
			var next = state.ResolveBossAttack();
			if (next.PlayerHp > 0)
			{
				stack.Push(next);
			}
		}
	}

	return best;
}

static IEnumerable<FightState> EnumeratePlayerMoves(FightState state)
{
	foreach (var spell in Spellbook.Spells)
	{
		if (state.PlayerMana < spell.Cost || !spell.CanCast(state))
		{
			continue;
		}

		yield return spell.Resolve(state);
	}
}

file static class Spellbook
{
	public static readonly Spell[] Spells =
	[
		new(
			"Magic Missile",
			53,
			_ => true,
			state => state with
			{
				PlayerMana = state.PlayerMana - 53,
				ManaSpent = state.ManaSpent + 53,
				BossHp = state.BossHp - 4,
				PlayerTurn = false,
			}),
		new(
			"Drain",
			73,
			_ => true,
			state => state with
			{
				PlayerMana = state.PlayerMana - 73,
				ManaSpent = state.ManaSpent + 73,
				BossHp = state.BossHp - 2,
				PlayerHp = state.PlayerHp + 2,
				PlayerTurn = false,
			}),
		new(
			"Shield",
			113,
			state => state.ShieldTimer == 0,
			state => state with
			{
				PlayerMana = state.PlayerMana - 113,
				ManaSpent = state.ManaSpent + 113,
				ShieldTimer = 6,
				PlayerTurn = false,
			}),
		new(
			"Poison",
			173,
			state => state.PoisonTimer == 0,
			state => state with
			{
				PlayerMana = state.PlayerMana - 173,
				ManaSpent = state.ManaSpent + 173,
				PoisonTimer = 6,
				PlayerTurn = false,
			}),
		new(
			"Recharge",
			229,
			state => state.RechargeTimer == 0,
			state => state with
			{
				PlayerMana = state.PlayerMana - 229,
				ManaSpent = state.ManaSpent + 229,
				RechargeTimer = 5,
				PlayerTurn = false,
			}),
	];
}

readonly record struct Spell(
	string Name,
	int Cost,
	Func<FightState, bool> CanCast,
	Func<FightState, FightState> Resolve);

readonly record struct FightState(
	int PlayerHp,
	int PlayerMana,
	int BossHp,
	int BossDamage,
	int ShieldTimer,
	int PoisonTimer,
	int RechargeTimer,
	int ManaSpent,
	bool PlayerTurn)
{
	public FightState ApplyEffects()
	{
		var state = this;

		if (state.PoisonTimer > 0)
		{
			state = state with { BossHp = state.BossHp - 3, PoisonTimer = state.PoisonTimer - 1 };
		}

		if (state.RechargeTimer > 0)
		{
			state = state with { PlayerMana = state.PlayerMana + 101, RechargeTimer = state.RechargeTimer - 1 };
		}

		if (state.ShieldTimer > 0)
		{
			state = state with { ShieldTimer = state.ShieldTimer - 1 };
		}

		return state;
	}

	public FightState ResolveBossAttack()
	{
		var armor = ShieldTimer > 0 ? 7 : 0;
		var damage = System.Math.Max(1, BossDamage - armor);
		return this with { PlayerHp = PlayerHp - damage, PlayerTurn = true };
	}
}

