using System;
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
        var map = new Cell[inputData.First().Count(),inputData.Count()];
        for(var x = 0; x < inputData.First().Count() - 1; x++)
        {
            for(var y = 0; y < inputData.Count() - 1; y++)
            {
                var type = inputData.ElementAt(y).ElementAt(x);
                if(type =='#')
                    map[x, y] = new Cell { Type = CellType.Wall };
                else if(type =='G')
                    map[x, y] = new Cell    { Type = CellType.Goblin };
                else if(type =='E')
                    map[x, y] = new Cell { Type = CellType.Elf };
            }
        }

        for(var y = 0; y < map.GetUpperBound(1); y++)
        {
            for(var x = 0; x < map.GetUpperBound(0); x++)
            {
                var cell = map[x, y];
                if(cell == null || cell.Type == CellType.Wall)
                    return;
                else 
                {
                    var closest = findCloses((x, y), cell, map); 
                }
            }
        }
        
    }

    private (int x, int y) findCloses((int x, int y) p, Cell cell, Cell[,] map)
    {
        throw new NotImplementedException();
    }

    private class Cell
    {
        public CellType Type { get; set; }

        public int Health { get; set; }

    }
    public enum CellType
    {
        Goblin,
        Elf,
        Wall
    }
}