using System;
using System.Collections.Generic;
using System.Linq;
public class Advent2018_11
{
    private static int _seed = 5093;

    public void Solve()
    {
        var grid = new Cell[300, 300];
        for(int y = 0; y < 300; y++)
        {
            for(int x = 0; x < 300; x++)
                grid[x, y] = new Cell() { X = x, Y = y };
        }
        
         
        for(int y = 0; y < 300; y++)
        {
            for(int x = 0; x < 300; x++)
            {
                var sum = 0;
                for(int size = 1; size <= 300; size++) 
                {
                    if(y + size > 300 || x + size > 300)
                        break;
                    if(size == 1)
                    {
                        sum = grid[x, y].PowerLevel;
                        grid[x, y].GridSum = (sum, size);
                    }
                    else 
                    {
                        for(int xx = 0; xx < size; xx++)
                            sum += grid[x + xx, y + size - 1].PowerLevel;
                        for(int yy = 0; yy < size - 1; yy++)
                            sum += grid[x + size - 1, y + yy].PowerLevel;
                        if(grid[x, y].GridSum.Item1 < sum)
                            grid[x, y].GridSum = (sum, size);
                    }
                }
            }
        }
        var maxCell = grid.Cast<Cell>().ToArray().OrderByDescending(x => x.GridSum.Item1).First();
        Console.WriteLine(string.Format("{0},{1},{2}", maxCell.X, maxCell.Y, maxCell.GridSum.Item2));
    }

    public class Cell
    {
        private int? _powerLevel;

        public int RackId { get { return X + 10; } }
        public int X { get; set; }
        public int Y { get; set; }

        
        public int PowerLevel 
        { 
            get 
            {
                if(_powerLevel.HasValue)
                    return _powerLevel.Value;
                var res = (((RackId * Y) + _seed) * RackId);
                var s = res < 100  ? 0 : res / 100 % 10;
                _powerLevel = s - 5;
                return _powerLevel.Value;
            }  
        } 

        public (int, int) GridSum { get; set; }
    }
}