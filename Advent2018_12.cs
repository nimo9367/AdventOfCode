using System;
using System.Collections.Generic;
using System.Linq;
public class Advent2018_12
{
    public void Solve()
    {
        var input = @"...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #";
        input = @"#.### => .
###.# => #
.##.. => .
..### => .
..##. => .
##... => #
###.. => #
.#... => #
##..# => #
#.... => .
.#.#. => .
####. => .
#.#.. => .
#.#.# => .
#..## => #
.#### => #
...## => .
#..#. => #
.#.## => #
..#.# => #
##.#. => #
#.##. => #
##### => .
..#.. => #
....# => .
##.## => .
.###. => #
..... => .
...#. => #
.##.# => .
#...# => .
.#..# => #";
        var inputData = input.Split('\n').Select(x => new Input { Pattern = x.Substring(0, 5), HasPlant = x.Substring(9, 1) == "#" }).ToArray();

        var gen = 0;
        var pots = "#.#..#..###.###.#..###.#####...########.#...#####...##.#....#.####.#.#..#..#.#..###...#..#.#....##.";//"#..#.#..##......###...###";
        var sumps = new List<int>();
        var prevSum = 0;
        while (gen < 2000)
        {
            var newSum = SumUp(gen, pots);
            Console.WriteLine("Sum gen {0}: {1}", gen, SumUp(gen, pots) - prevSum);
            prevSum = newSum;

            pots = "..." + pots + "...";
            var newPots = string.Empty;
            for (var i = 0; i < pots.Length - 4; i++)
            {
                var potPattern = string.Join("", pots.Skip(i).Take(5));
                var matchingPattern = inputData.FirstOrDefault(x => potPattern == x.Pattern);
                if (matchingPattern != null)
                    newPots += matchingPattern.HasPlant ? "#" : ".";
                else
                    newPots += ".";
            }
            pots = newPots;
            gen++;
        }

        int sum = SumUp(gen, pots);
        Console.WriteLine("Pattern: " + pots.TrimStart('.'));
        Console.WriteLine("Sum: " + sum);
    }

    private static int SumUp(int gen, string pots)
    {
        var sum = 0;
        var generation = gen * -1;
        foreach (var pot in pots.ToCharArray())
        {
            if (pot == '#')
                sum += generation;
            generation++;
        }

        return sum;
    }

    internal class Input
    {
        public string Pattern { get; internal set; }
        public bool HasPlant { get; internal set; }
    }
}