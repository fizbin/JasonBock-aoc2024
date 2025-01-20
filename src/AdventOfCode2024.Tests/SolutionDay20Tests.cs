using NUnit.Framework;

namespace AdventOfCode2024.Day20.Tests;

public static class SolutionDay20Tests
{
	[TestCase(80, 0)]
	[TestCase(64, 1)]
	[TestCase(40, 2)]
	[TestCase(38, 3)]
	[TestCase(36, 4)]
	[TestCase(20, 5)]
	[TestCase(12, 8)]
	[TestCase(10, 10)]
	[TestCase(8, 14)]
	[TestCase(6, 16)]
	[TestCase(4, 30)]
	[TestCase(3, 30)]
	[TestCase(2, 44)]
	public static void Part1(int minimumSavings, int expectedCheatCount)
	{
		var input =
			"""
			###############
			#...#...#.....#
			#.#.#.#.#.###.#
			#S#...#.#.#...#
			#######.#.#.###
			#######.#.#...#
			#######.#.###.#
			###..E#...#...#
			###.#######.###
			#...###...#...#
			#.#####.#.###.#
			#.#...#.#.#...#
			#.#.#.#.#.#.###
			#...#...#...###
			###############
			""";


		Assert.That(
			SolutionDay20.RunPart1([.. input.Split(Environment.NewLine)], minimumSavings),
			Is.EqualTo(expectedCheatCount));
	}

	[TestCase(80, 0)]
	[TestCase(76, 3)]
	[TestCase(74, 7)]
	[TestCase(72, 29)]
	[TestCase(70, 41)]
	public static void Part2(int minimumSavings, int expectedCheatCount)
	{
		var input =
			"""
			###############
			#...#...#.....#
			#.#.#.#.#.###.#
			#S#...#.#.#...#
			#######.#.#.###
			#######.#.#...#
			#######.#.###.#
			###..E#...#...#
			###.#######.###
			#...###...#...#
			#.#####.#.###.#
			#.#...#.#.#...#
			#.#.#.#.#.#.###
			#...#...#...###
			###############
			""";


		Assert.That(
			SolutionDay20.RunPart2([.. input.Split(Environment.NewLine)], minimumSavings),
			Is.EqualTo(expectedCheatCount));
	}
}