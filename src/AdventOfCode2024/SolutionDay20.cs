using System.Collections.Immutable;

namespace AdventOfCode2024.Day20;

public static class SolutionDay20
{
	private const char End = 'E';
	private const char Start = 'S';
	private const char Wall = '#';

	public static int RunPart1(ImmutableArray<string> input, int minimumSaving)
	{
		var path = SolutionDay20.GetPath(input);

		var posToPath = new Dictionary<Position, int>();
		var pathIdx = 0;
		foreach (var pos in path)
		{
			posToPath[pos] = pathIdx++;
		}

		var cheatSavings = 0;

		foreach (var startSpot in path)
		{
			var startIdx = posToPath[startSpot];
			foreach (var endSpot in SolutionDay20.GetSpotsWithin(startSpot, 2, 2))
			{
				var endIdx = posToPath.GetValueOrDefault(endSpot, 0);
				if (endIdx - startIdx - 2 >= minimumSaving)
				{
					cheatSavings++;
				}
			}
		}

		return cheatSavings;
	}

	public static int RunPart2(ImmutableArray<string> input, int minimumSaving)
	{
		var path = SolutionDay20.GetPath(input);

		var posToPath = new Dictionary<Position, int>();
		var pathIdx = 0;
		foreach (var pos in path)
		{
			posToPath[pos] = pathIdx++;
		}

		var cheatSavings = 0;

		foreach (var startSpot in path)
		{
			var startIdx = posToPath[startSpot];
			foreach (var endSpot in SolutionDay20.GetSpotsWithin(startSpot, 20, 2))
			{
				var endIdx = posToPath.GetValueOrDefault(endSpot, 0);
				var cheatLen = Math.Abs(endSpot.X - startSpot.X) + Math.Abs(endSpot.Y - startSpot.Y);
				if (endIdx - startIdx - cheatLen >= minimumSaving)
				{
					cheatSavings++;
				}
			}
		}

		return cheatSavings;
	}


	private static ImmutableArray<Position> GetSpotsWithin(Position src, int maxDist, int minDist)
	{
		var paths = new List<Position>();
		// gets all spots that are at least minDist away but at most maxDist away
		for (var xoff = -maxDist; xoff <= maxDist; xoff++)
		{
			for (var yoff = -maxDist; yoff <= maxDist; yoff++)
			{
				if (Math.Abs(xoff) + Math.Abs(yoff) > maxDist) { continue; }
				if (Math.Abs(xoff) + Math.Abs(yoff) < minDist) { continue; }
				paths.Add(new Position(xoff + src.X, yoff + src.Y));
			}
		}
		return [.. paths];
	}

	private static ImmutableArray<Position> GetPath(ImmutableArray<string> input)
	{
		Position? startPosition = null;

		for (var y = 0; y < input.Length; y++)
		{
			var x = input[y].IndexOf(SolutionDay20.Start, StringComparison.CurrentCulture);

			if (x >= 0)
			{
				startPosition = new Position(x, y);
				break;
			}
		}

		var path = new List<Position> { startPosition! };

		var maxX = input[0].Length;
		var maxY = input.Length;

		var currentPosition = startPosition!;

		while (true)
		{
			// At our current position:
			// * Find all the cheats
			// * Find the next position, and if that equals endPosition, we're done and we break

			// Look East (x + 1)
			var eastPosition = currentPosition with { X = currentPosition.X + 1 };
			var eastCharacter = input[eastPosition.Y][eastPosition.X];

			if (eastCharacter != SolutionDay20.Wall && !path.Contains(eastPosition))
			{
				path.Add(eastPosition);

				if (eastCharacter == SolutionDay20.End)
				{
					break;
				}
			}

			// Look South (y + 1)
			var southPosition = currentPosition with { Y = currentPosition.Y + 1 };
			var southCharacter = input[southPosition.Y][southPosition.X];

			if (southCharacter != SolutionDay20.Wall && !path.Contains(southPosition))
			{
				path.Add(southPosition);

				if (southCharacter == SolutionDay20.End)
				{
					break;
				}
			}

			// Look West (x - 1)
			var westPosition = currentPosition with { X = currentPosition.X - 1 };
			var westCharacter = input[westPosition.Y][westPosition.X];

			if (westCharacter != SolutionDay20.Wall && !path.Contains(westPosition))
			{
				path.Add(westPosition);

				if (westCharacter == SolutionDay20.End)
				{
					break;
				}
			}

			// Look North (y - 1)
			var northPosition = currentPosition with { Y = currentPosition.Y - 1 };
			var northCharacter = input[northPosition.Y][northPosition.X];

			if (northCharacter != SolutionDay20.Wall && !path.Contains(northPosition))
			{
				path.Add(northPosition);

				if (northCharacter == SolutionDay20.End)
				{
					break;
				}
			}

			currentPosition = path[^1];
		}

		return ([.. path]);
	}
}

public sealed record Position(int X, int Y);