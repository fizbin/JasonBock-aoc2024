using System.Collections.Immutable;
using Map = System.Collections.Immutable.ImmutableDictionary<AdventOfCode2024.Day16.Position, AdventOfCode2024.Day16.MapItemType>;

namespace AdventOfCode2024.Day16;

public static class SolutionDay16
{
	public const char End = 'E';
	public const char Start = 'S';
	public const char Wall = '#';

	public static long RunPart1(ImmutableArray<string> input)
	{
		var map = SolutionDay16.ParseInput(input);

		var minimalCost = long.MaxValue;

		var startLocation = map.Single(_ => _.Value == MapItemType.Start).Key;
		ImmutableList<Path> startPaths = [
			new(map, 0, 0, startLocation, Direction.East, false, null),
			new(map, 0, 0, startLocation, Direction.West, false, null),
			new(map, 0, 0, startLocation, Direction.South, false, null),
			new(map, 0, 0, startLocation, Direction.North, false, null),
		];

		var pathsToEvaluate = new List<Path>(startPaths);

		var bestCosts = new Dictionary<Direction, Dictionary<Position, long>>();
		foreach (var dir in Enum.GetValues<Direction>())
		{
			bestCosts[dir] = [];
		}

		while (pathsToEvaluate.Count > 0)
		{
			var newPaths = new List<Path>();

			foreach (var pathToEvaluate in pathsToEvaluate)
			{
				if (pathToEvaluate.CurrentCost > bestCosts[pathToEvaluate.CurrentDirection].GetValueOrDefault(pathToEvaluate.CurrentPosition, long.MaxValue))
				{
					continue;
				}
				bestCosts[pathToEvaluate.CurrentDirection][pathToEvaluate.CurrentPosition] = pathToEvaluate.CurrentCost;
				var nextPaths = pathToEvaluate.GetNextPaths();
				var minimalFinishedNextPath = nextPaths.Where(_ => _.IsFinished).MinBy(_ => _.CurrentCost);

				if (minimalFinishedNextPath?.CurrentCost < minimalCost)
				{
					minimalCost = minimalFinishedNextPath.CurrentCost;
				}

				newPaths.AddRange(nextPaths.Where(_ => !_.IsFinished && _.CurrentCost < minimalCost));
			}

			pathsToEvaluate = newPaths;
		}

		return minimalCost;
	}

	public static long RunPart2(ImmutableArray<string> input)
	{
		var map = SolutionDay16.ParseInput(input);

		var minimalCost = long.MaxValue;
		List<Path> minPaths = [];

		var startLocation = map.Single(_ => _.Value == MapItemType.Start).Key;
		ImmutableList<Path> startPaths = [
			new(map, 0, 0, startLocation, Direction.East, false, null),
			new(map, 0, 0, startLocation, Direction.West, false, null),
			new(map, 0, 0, startLocation, Direction.South, false, null),
			new(map, 0, 0, startLocation, Direction.North, false, null),
		];

		var pathsToEvaluate = new List<Path>(startPaths);

		var bestCosts = new Dictionary<Direction, Dictionary<Position, long>>();
		foreach (var dir in Enum.GetValues<Direction>())
		{
			bestCosts[dir] = [];
		}

		while (pathsToEvaluate.Count > 0)
		{
			var newPaths = new List<Path>();

			foreach (var pathToEvaluate in pathsToEvaluate)
			{
				if (pathToEvaluate.CurrentCost > bestCosts[pathToEvaluate.CurrentDirection].GetValueOrDefault(pathToEvaluate.CurrentPosition, long.MaxValue))
				{
					continue;
				}
				bestCosts[pathToEvaluate.CurrentDirection][pathToEvaluate.CurrentPosition] = pathToEvaluate.CurrentCost;
				var nextPaths = pathToEvaluate.GetNextPaths();
				var minimalFinishedNextPath = nextPaths.Where(_ => _.IsFinished).MinBy(_ => _.CurrentCost);

				if (minimalFinishedNextPath?.CurrentCost < minimalCost)
				{
					minimalCost = minimalFinishedNextPath.CurrentCost;
					minPaths = nextPaths.Where(_ => _.IsFinished && _.CurrentCost == minimalCost).ToList();
				}
				else if (minimalFinishedNextPath?.CurrentCost == minimalCost)
				{
					minPaths.AddRange(nextPaths.Where(_ => _.IsFinished && _.CurrentCost == minimalCost));
				}

				newPaths.AddRange(nextPaths.Where(_ => !_.IsFinished && _.CurrentCost < minimalCost));
			}

			pathsToEvaluate = newPaths;
		}

		var minPathTiles = new HashSet<Position>();
		foreach (var minP in minPaths)
		{
			var cp = minP;
			while (cp is not null)
			{
				minPathTiles.Add(cp.CurrentPosition);
				cp = cp.parent;
			}
		}
		return minPathTiles.Count;
	}

	private static Map ParseInput(ImmutableArray<string> input)
	{
		var mapItems = new Dictionary<Position, MapItemType>();

		for (var y = 0; y < input.Length; y++)
		{
			var data = input[y];

			for (var x = 0; x < data.Length; x++)
			{
				var mapType = data[x];

				if (mapType == Start || mapType == End || mapType == Wall)
				{
					mapItems.Add(new Position(x, y), mapType switch
					{
						Start => MapItemType.Start,
						End => MapItemType.End,
						Wall => MapItemType.Wall,
						_ => throw new NotSupportedException()
					});
				}
			}
		}
		return mapItems.ToImmutableDictionary();
	}
}

public sealed record Path(Map Map, int TraversedPositionCount, int NumberOfTurns,
	Position CurrentPosition, Direction CurrentDirection, bool IsFinished, Path? parent)
{
	public ImmutableArray<Path> GetNextPaths()
	{
		var newPaths = new List<Path>();
		foreach (var look in Enum.GetValues<Direction>())
		{
			if (this.CurrentDirection != look.Opposite())
			{
				var nextPosition = this.CurrentPosition.Look(look);
				if (this.Map.GetValueOrDefault(nextPosition, MapItemType.End) == MapItemType.End)
				{
					newPaths.Add(this with
					{
						TraversedPositionCount = this.TraversedPositionCount + 1,
						NumberOfTurns = this.CurrentDirection != look ? this.NumberOfTurns + 1 : this.NumberOfTurns,
						CurrentPosition = nextPosition,
						CurrentDirection = look,
						IsFinished = this.Map.ContainsKey(nextPosition),
						parent = this,
					});
				}
			}
		}

		return [.. newPaths];
	}

	public long CurrentCost => this.TraversedPositionCount + (1_000L * this.NumberOfTurns);
}

public enum MapItemType { Start, End, Wall }
public enum Direction { West, North, East, South }
static class DirectionExtension
{
	public static Direction Opposite(this Direction d) => d switch
	{
		Direction.North => Direction.South,
		Direction.South => Direction.North,
		Direction.West => Direction.East,
		_ => Direction.West,
	};
}
public sealed record Position(int X, int Y)
{
	public Position Look(Direction direction) => direction switch
	{
		Direction.North => this with { Y = this.Y - 1 },
		Direction.South => this with { Y = this.Y + 1 },
		Direction.West => this with { X = this.X - 1 },
		_ => this with { X = this.X + 1 },
	};
}
public sealed record MapItem(MapItemType Type, Position Position);
public sealed record Reindeer(Direction CurrentDirection, Position Position);