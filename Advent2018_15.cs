using System;
using System.Collections.Generic;
using System.Linq;

public class Advent2018_15
{

    public void Solve()
    {
        var input = @"#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########";
        var inputData = input.Split(new string[] { "\r\n" }, StringSplitOptions.None).Select(x => x.ToCharArray());
        var map = new Cell[inputData.First().Count(), inputData.Count()];

        var walls = new List<Cell>();
        var elfs = new List<Cell>();
        var goblins = new List<Cell>();

        for (var x = 0; x < inputData.First().Count() - 1; x++)
        {
            for (var y = 0; y < inputData.Count() - 1; y++)
            {
                var type = inputData.ElementAt(y).ElementAt(x);
                if (type == '#')
                    walls.Add(new Cell { Type = CellType.Wall, Coord = (x, y) });
                else if (type == 'G')
                    goblins.Add(new Cell { Type = CellType.Goblin, Coord = (x, y) });
                else if (type == 'E')
                    elfs.Add(new Cell { Type = CellType.Elf, Coord = (x, y) });
            }
        }
        var combatants = elfs.Concat(goblins).OrderBy(x => x.Coord.Y).ThenBy(x => x.Coord.Y);
        foreach (var combatant in combatants.Take(1))
        {
            var enemies = combatant.Type == CellType.Goblin ? elfs : goblins;
            {
                foreach(var enemy in enemies.Take(1))
                {
                    var distance = findDistance(combatant, enemies, combatants.Concat(walls).Except(new Cell[] { combatant, enemy } ));
                    Console.WriteLine(distance);
                }
            }
        }

    }

    private int findDistance(Cell combatant, IEnumerable<Cell> enemies, IEnumerable<Cell> obsticles)
    {
        var visited = new List<(int X, int Y)>();
        Queue<Node> q = new Queue<Node>();
        q.Enqueue(new Node() { Coord = combatant.Coord, Distance = 0 });
        while (q.Count > 0)
        {
            Node current = q.Dequeue();
            if (current == null)
                continue;
            var enemy = enemies.FirstOrDefault(x => x.Coord.X == current.Coord.X && x.Coord.Y == current.Coord.Y);
            if(enemy != null)
                return current.Distance;

            var left = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X - 1 && x.Coord.Y == current.Coord.Y);
            var right = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X + 1 && x.Coord.Y == current.Coord.Y);
            var top = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X && x.Coord.Y == current.Coord.Y - 1);
            var bottom = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X && x.Coord.Y == current.Coord.Y + 1);
            var neighbours = new (int X, int Y)[] 
            { 
                left != null ? (left.Type == CellType.Wall ? (-1, -1) : left.Coord) : (current.Coord.X - 1, current.Coord.Y), 
                right != null ? (right.Type == CellType.Wall ? (-1, -1) : right.Coord): (current.Coord.X + 1, current.Coord.Y), 
                top != null ? (top.Type == CellType.Wall ? (-1, -1) : top.Coord) : (current.Coord.X, current.Coord.Y - 1), 
                bottom != null ? (bottom.Type == CellType.Wall ? (-1, -1) : bottom.Coord) : (current.Coord.X, current.Coord.Y + 1) 
            };

            foreach(var n in neighbours.Where(x => x.X != -1 ).Except(visited))
            {
                q.Enqueue(new Node { Coord = n, Distance = current.Distance + 1});
                visited.Add(n);
            }
        }
        return 0;
    }

    private class Cell
    {
        public CellType Type { get; set; }

        public int Health { get; set; }

        public (int X, int Y) Coord { get; set; }

    }

    private class Node
    {
        public (int X, int Y) Coord { get; set; }
        public int Distance { get; set; }

    }

    public enum CellType
    {
        Goblin,
        Elf,
        Wall
    }
}