using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Advent2018_6
{
    private readonly List<Coord> _inputData;
    private readonly Coord[] _orgInputData;

    public Advent2018_6()
    {
        #region input
        var input = @"124, 262
182, 343
79, 341
44, 244
212, 64
42, 240
225, 195
192, 325
192, 318
42, 235
276, 196
181, 262
199, 151
166, 214
49, 81
202, 239
130, 167
166, 87
197, 53
341, 346
235, 241
99, 278
163, 184
85, 152
349, 334
175, 308
147, 51
251, 93
163, 123
151, 219
162, 107
71, 58
249, 293
223, 119
46, 176
214, 140
80, 156
265, 153
92, 359
103, 186
242, 104
272, 202
292, 93
304, 55
115, 357
43, 182
184, 282
352, 228
267, 147
248, 271";
        #endregion

//         input = @"1, 1
// 1, 6
// 8, 3
// 3, 4
// 5, 5
// 8, 9";
        _inputData = input.Split("\n").Select(x => new Coord { X = int.Parse(x.Split(',')[0].Trim()), Y = int.Parse(x.Split(',')[1].Trim()) }).ToList();
        _orgInputData = _inputData.ToArray();
        // Normalize
        var minX = _inputData.Min(x => x.X);
        var minY = _inputData.Min(x => x.Y);
        var i = 1;
        foreach(var inp in _inputData) {
            inp.X -= minX;
            inp.Y -= minY;
            inp.Id = i;
            i++;
        }
        
    }

    public void Solve()
    {
        var maxX = _inputData.Max(x => x.X);
        var maxY = _inputData.Max(x => x.Y);
        var world = new int[maxX + 1, maxY + 1];
        var cellsTotal = (maxY + 1) * (maxX + 1);
        var gen = _inputData.Count();
    
        Console.WriteLine("Cells total: " + cellsTotal);

        while(_inputData.Any(c => !c.HasNoFrontier)){
            
            foreach(var coord in _inputData.Where(c => !c.HasNoFrontier).ToArray())
                ClaimNearest(coord, gen, world);
            gen++;
            // Print(maxX, maxY);
            Console.WriteLine("Cells processed: " + _inputData.Count());
        }
        // Print(maxX, maxY);
        // var edgeCells = _inputData.Where(x => x.X == maxX || x.X == 0 || x.Y == maxY|| x.Y == 0).Select(x => x.Id).Distinct().ToArray();
        // var innerCells = _inputData.Where(x => !edgeCells.Any(y => y == x.Id)).GroupBy(x => x.Id).Select(x => "[" + x.First().Id + ", " + x.Count() + "]").Distinct();
        // Console.WriteLine("Inner cells: " + string.Join(',', innerCells));

        // var bestquad = BestQuad(maxX / 2, maxY / 2);
        // var cooolCell =  _inputData.First(x => x.X == bestquad.Item1 && x.Y == bestquad.Item2); 
        // Console.WriteLine(string.Format("Cool cell:  {0}, {1}", cooolCell.X, cooolCell.Y));
        // Console.WriteLine(_inputData.Count(x => x.Id == cooolCell.Id));

    //    var distances = new List<(int[], int)>();
    //     for(var y = 0; y < maxY; y++)
    //     {
    //         for(var x = 0; x < maxX; x++)
    //         {
    //             if(_orgInputData.Any(c => c.X == x && c.Y == y))
    //                 continue;
    //             var totalDistance = _orgInputData.Select(c => Math.Abs(c.X - x) + Math.Abs(c.Y - y)).Sum();
    //             distances.Add((new int[] {x, y}, totalDistance));
    //             if ( x * y % 1000 == 0)
    //                 Console.WriteLine("Processed: " + x*y);
    //         } 
    //     }
    //     var d = distances.OrderBy(x => x.Item2);
        var okCells = new List<Coord>();
        foreach(var cell in _inputData)
        {
            var sum = 0;
            foreach(var org in _orgInputData) {
                sum += Math.Abs(org.X - cell.X) + Math.Abs(org.Y - cell.Y);
                if(sum > 9999)
                    break;
            }
                
            if(sum < 10000)
                okCells.Add(cell);
        }
        Console.WriteLine("Num cells: " + okCells.Count());
    }

    private (int, int) BestQuad(int x, int y)
    {
        var halfX = x / 2;
        var halfY = y / 2;
        var sum1 = SumOfDistance(x + halfX, y + halfY);
        var sum2 = SumOfDistance(x + halfX, y - halfY);
        var sum3 = SumOfDistance(x - halfX, y + halfY);
        var sum4 = SumOfDistance(x - halfX, y - halfY);

        var min = new int[]{ sum1, sum2, sum3, sum4 }.Min(); 
        if(sum1 == min)
            return (x + halfX, y + halfY);
        if(sum1 == min)
            return (x + halfX, y - halfY);
        if(sum1 == min)
            return (x - halfX, y + halfY);
        else
            return (x - halfX, y - halfY);
    }

    private int SumOfDistance(int x, int y)
    {
        var distances = _inputData.Select(c => (c, Math.Abs(c.X - x) + Math.Abs(c.Y - y)));
        return distances.Sum(c => c.Item2);
    }

    private void ClaimNearest(Coord coord, int gen, int[,] world)
    {  
        var cellClaimed = false;
        if(coord.X > 0)
            cellClaimed = TryClaim(coord.X - 1, coord.Y, coord.Id, gen, world);
        if(coord.X + 1 < world.GetLength(0))
            cellClaimed = TryClaim(coord.X + 1, coord.Y, coord.Id,gen, world);
            
        if(coord.Y > 0)
            cellClaimed = TryClaim(coord.X, coord.Y - 1, coord.Id, gen, world);
        if(coord.Y + 1 < world.GetLength(1))
            cellClaimed = TryClaim(coord.X, coord.Y + 1, coord.Id, gen, world);
        coord.HasNoFrontier = !cellClaimed;
    }

    private bool TryClaim(int x, int y, int id, int gen, int[,] world)
    {
        var cell = _inputData.FirstOrDefault(c => c.X == x && c.Y == y);
        if(cell == null) {
            _inputData.Add(new Coord { X = x, Y= y, Id = id, Gen = gen});
            return true;
        }
        else if(cell.Id != id && cell.Gen == gen)
        {
            _inputData.Remove(cell);
            _inputData.Add(new Coord { X = x, Y= y, Id = -1, Gen = gen});
            return false;
        }
        return false;
    }

    private void Print(int maxX, int maxY)
    {
        using(var file = new StreamWriter(@"C:\Temp\output.txt"))
        {
            for (var y = 0; y <= maxY; y++)
            {
                var line = string.Empty;
                for (var x = 0; x <= maxX; x++)
                {
                    var cell = _inputData.FirstOrDefault(c => c.X == x && c.Y == y);
                    var val = cell == null ? 0 : cell.Id;
                    if(val == -1)
                        line += ".";
                    else if(val == 0)
                        line += "-";
                    else
                        line += val.ToString();
                }
                file.WriteLine(line);
            }
            file.WriteLine("------------------");
        }
    }

    private class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Id { get; set; }
        public int Gen { get; set; }
        public bool HasNoFrontier { get; set; }
    }
}