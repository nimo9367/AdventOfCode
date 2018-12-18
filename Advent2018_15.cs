using System;
using System.Collections.Generic;
using System.Linq;

public class Advent2018_15
{

    public void Solve()
    {
        var input = @"#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######";
        var inputData = input.Split(new string[] { "\r\n" }, StringSplitOptions.None).Select(x => x.ToCharArray());
        var map = new Cell[inputData.First().Count(), inputData.Count()];

        var walls = new List<Cell>();
        var elfs = new List<Cell>();
        var goblins = new List<Cell>();

        for (var x = 0; x < inputData.First().Count(); x++)
        {
            for (var y = 0; y < inputData.Count(); y++)
            {
                var type = inputData.ElementAt(y).ElementAt(x);
                if (type == '#')
                    walls.Add(new Cell { Type = CellType.Wall, Coord = (x, y), Health = 200 });
                else if (type == 'G')
                    goblins.Add(new Cell { Type = CellType.Goblin, Coord = (x, y), Health = 200 });
                else if (type == 'E')
                    elfs.Add(new Cell { Type = CellType.Elf, Coord = (x, y), Health = 200 });
            }
        }
        var round = 0;
        while(elfs.Concat(goblins).Where(x => x.Health > 0).GroupBy(x => x.Type).Count() > 1)
        {
            var combatants = elfs.Concat(goblins).OrderBy(x => x.Coord.Y).ThenBy(x => x.Coord.X);
            foreach (var combatant in combatants.Where(x => x.Health > 0))
            {
                var enemies = combatant.Type == CellType.Goblin ? elfs : goblins;
                {
                    var distances = new List<Node>();
                    foreach(var enemy in enemies.Where(x => x.Health > 0).OrderBy(x => x.Coord.Y).ThenBy(x => x.Coord.X))
                        distances.AddRange(findDistance(combatant, enemy, combatants.Where(x => x.Health > 0).Concat(walls).Except(new Cell[] { combatant } )));

                    if(!distances.Any() || combatant.Health == 0)
                        continue;
                    
                    var shortestDistances = distances.GroupBy(x => x.Distance).OrderBy(x => x.Key).First().OrderBy(x => x.Coord.Y).ThenBy(x => x.Coord.X);
                    var shortestDistance = shortestDistances.First();
                    Node parentNode = shortestDistance;
                    var moved = false;
                    if(parentNode.Distance > 1)
                    {
                        (int X, int Y) nextCoord = (-1, -1);
                        while(parentNode.Distance > 0)
                        {
                            nextCoord = parentNode.Coord;
                            parentNode = parentNode.Parent; 
                        }
                        combatant.Coord = nextCoord;
                        moved = true;
                    }
                    if(parentNode.Distance == 1)
                    {
                        var enemiesInRange = enemies.Where(x => shortestDistances.Any(y => x.Coord.X == y.Coord.X && x.Coord.Y == y.Coord.Y));
                        var enemy = enemiesInRange.OrderBy(x => x.Health).FirstOrDefault();
                        if(enemy != null)
                        {
                            if(enemy.Health >= 3)
                                enemy.Health = enemy.Health - 3 - (moved ? 3 : 0);
                            else
                                enemy.Health = 0;
                        }
                    }
                }
            }
            round++;
            Console.WriteLine("Round: " + round);
            Print(combatants.Concat(walls), inputData.First().Count(), inputData.Count());
            foreach(var c in combatants)
                Console.WriteLine($"{c.Type}({c.Health})");
            
        }
        Print(elfs.Concat(goblins).Concat(walls), inputData.First().Count(), inputData.Count());
        var sum = elfs.Concat(goblins).Where(x => x.Health > 0).Sum(x => x.Health);
        Console.WriteLine("Sum: " + sum);
        Console.WriteLine("Round: " + round);
    }

    private void Print(IEnumerable<Cell> cells, int xMax, int yMax)
    {
        for(var y = 0; y < yMax; y++)
        {
            var line = string.Empty;
            for(var x = 0; x < xMax; x++)
            {
                var cell = cells.FirstOrDefault(c => c.Coord.X == x && c.Coord.Y == y && c.Health > 0);
                if(cell != null)
                {
                    if(cell.Type == CellType.Elf)
                        line += "E";
                    else if(cell.Type == CellType.Goblin)
                        line += "G";
                    else if(cell.Type == CellType.Wall)
                        line += "#";
                }
                else
                    line += ".";
            }
            Console.WriteLine(line);
        }
    }

    private List<Node> findDistance(Cell combatant, Cell enemy, IEnumerable<Cell> obsticles)
    {
        var visited = new List<(int X, int Y)>();
        var paths = new List<Node>();
        Queue<Node> q = new Queue<Node>();
        var start = new Node() { Coord = combatant.Coord, Distance = 0 };
        q.Enqueue(start);
        while (q.Count > 0)
        { 
            Node current = q.Dequeue();
            if (current == null)
                continue;
            var blockage = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X && x.Coord.Y == current.Coord.Y);
            if(enemy.Coord.X == current.Coord.X && enemy.Coord.Y == current.Coord.Y)
                paths.Add(current);
            else if(blockage != null)
                continue;

            var left = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X - 1 && x.Coord.Y == current.Coord.Y);
            var right = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X + 1 && x.Coord.Y == current.Coord.Y);
            var top = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X && x.Coord.Y == current.Coord.Y - 1);
            var bottom = obsticles.FirstOrDefault(x => x.Coord.X == current.Coord.X && x.Coord.Y == current.Coord.Y + 1);
            var neighbours = new (int X, int Y)[] 
            { 
                top != null ? (top.Type != enemy.Type ? (-1, -1) : top.Coord) : (current.Coord.X, current.Coord.Y - 1), 
                right != null ? (right.Type != enemy.Type ? (-1, -1) : right.Coord): (current.Coord.X + 1, current.Coord.Y), 
                left != null ? (left.Type != enemy.Type ? (-1, -1) : left.Coord) : (current.Coord.X - 1, current.Coord.Y), 
                bottom != null ? (bottom.Type != enemy.Type ? (-1, -1) : bottom.Coord) : (current.Coord.X, current.Coord.Y + 1) 
            };

            foreach(var n in neighbours.Where(x => x.X != -1).Except(visited))
            {
                q.Enqueue(new Node { Coord = n, Distance = current.Distance + 1, Parent = current });
                visited.Add(n);
            }
        }
        return paths;
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
        public Node Parent { get; internal set; }
    }

    public enum CellType
    {
        Goblin,
        Elf,
        Wall
    }
}