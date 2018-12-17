using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Advent2018_13
{
    private char[,] map;
    private List<Car> cars;

    public void Solve() 
    {
        var input = @"/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/";
        input = File.ReadAllText(@"Advent2018_13.input");
        var inputData = input.Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToCharArray());
        map = new char[inputData.First().Count(), inputData.Count()];
        for(var row = 0; row < inputData.Count(); row++)
        {
            for(var col = 0; col < inputData.First().Count(); col++)
            map[col, row] = inputData.ElementAt(row).ElementAt(col);
        }
        //Print(map);
        var carCoords = FindCars(map).ToArray();
        cars = new List<Car>();
        foreach(var carCoord in carCoords)
        {
            var car =  new Car() { Direction = map[carCoord.x, carCoord.y], Coord = carCoord, LastTurn = Turn.Initial };
            cars.Add(car);
            if(car.Direction == '<' || car.Direction == '>')
            {
                car.Road = '-';
                map[carCoord.x, carCoord.y] = '-';
            }
            else
            {
                car.Road = '|';
                map[carCoord.x, carCoord.y] = '|';
            }
        }

        var count = 0;
        while(count < 50000) 
        {
            foreach(var car in cars.OrderBy(x => x.Coord.y).ThenBy(x => x.Coord.x))
                SetNewPosition(map, car);

            //Print(map);
            var survivors = cars.Where(x => !x.Crashed).ToArray();
            
            if(survivors.Count() == 1)
            {
                var winner = survivors.First();
                Print(map);
                Console.WriteLine($"{winner.Coord.x}, {winner.Coord.y}");
                break;
            }
            count++;
        }
    }

    private void SetNewPosition(char[,] map, Car car)
    {
        if(car.Crashed)
            return;
        map[car.Coord.x, car.Coord.y] = car.Road;
        if (car.Direction == '>')
        {
            var newPos = map[car.Coord.x + 1, car.Coord.y];
            CheckPos((car.Coord.x + 1, car.Coord.y), car.Coord);
            if (newPos == '+')
            {
                switch (car.LastTurn)
                {
                    case Turn.Initial:
                        car.Direction = '^';
                        car.LastTurn = Turn.Left;
                        break;
                    case Turn.Left:
                        car.Direction = '>';
                        car.LastTurn = Turn.Straight;
                        break;
                    case Turn.Straight:
                        car.Direction = 'v';
                        car.LastTurn = Turn.Right;
                        break;
                    case Turn.Right:
                        car.Direction = '^';
                        car.LastTurn = Turn.Left;
                        break;
                }
            }
            else if(newPos == '\\')
                    car.Direction = 'v';
            else if(newPos == '/')
                    car.Direction = '^';
            car.Coord = (car.Coord.x + 1, car.Coord.y);
        }
        else if (car.Direction == 'v')
        {
            var newPos = map[car.Coord.x, car.Coord.y + 1];
            CheckPos((car.Coord.x, car.Coord.y + 1), car.Coord);
            if (newPos == '+')
            {
                switch (car.LastTurn)
                {
                    case Turn.Initial:
                        car.Direction = '>';
                        car.LastTurn = Turn.Left;
                        break;
                    case Turn.Left:
                        car.Direction = 'v';
                        car.LastTurn = Turn.Straight;
                        break;
                    case Turn.Straight:
                        car.Direction = '<';
                        car.LastTurn = Turn.Right;
                        break;
                    case Turn.Right:
                        car.Direction = '>';
                        car.LastTurn = Turn.Left;
                        break;
                }
            }
            else if(newPos == '\\')
                    car.Direction = '>';
            else if(newPos == '/')
                    car.Direction = '<';
            car.Coord = (car.Coord.x, car.Coord.y + 1);
        }
        else if (car.Direction == '<')
        {
            var newPos = map[car.Coord.x - 1, car.Coord.y];
            CheckPos((car.Coord.x - 1, car.Coord.y), car.Coord);
            if (newPos == '+')
            {
                switch (car.LastTurn)
                {
                    case Turn.Initial:
                        car.Direction = 'v';
                        car.LastTurn = Turn.Left;
                        break;
                    case Turn.Left:
                        car.Direction = '<';
                        car.LastTurn = Turn.Straight;
                        break;
                    case Turn.Straight:
                        car.Direction = '^';
                        car.LastTurn = Turn.Right;
                        break;
                    case Turn.Right:
                        car.Direction = 'v';
                        car.LastTurn = Turn.Left;
                        break;
                }
            }
            else if(newPos == '\\')
                    car.Direction = '^';
            else if(newPos == '/')
                    car.Direction = 'v';
            car.Coord = (car.Coord.x - 1, car.Coord.y);
        }
        else if (car.Direction == '^')
        {
            var newPos = map[car.Coord.x, car.Coord.y - 1];
            CheckPos((car.Coord.x, car.Coord.y - 1), car.Coord);
            if (newPos == '+')
            {
                switch (car.LastTurn)
                {
                    case Turn.Initial:
                        car.Direction = '<';
                        car.LastTurn = Turn.Left;
                        break;
                    case Turn.Left:
                        car.Direction = '^';
                        car.LastTurn = Turn.Straight;
                        break;
                    case Turn.Straight:
                        car.Direction = '>';
                        car.LastTurn = Turn.Right;
                        break;
                    case Turn.Right:
                        car.Direction = '<';
                        car.LastTurn = Turn.Left;
                        break;
                }
            }
            else if(newPos == '\\')
                    car.Direction = '<';
            else if(newPos == '/')
                    car.Direction = '>';
            car.Coord = (car.Coord.x, car.Coord.y - 1);
        }
        car.Road = map[car.Coord.x, car.Coord.y];
        if(!car.Crashed)
            map[car.Coord.x, car.Coord.y] = car.Direction;
    }

    private void CheckPos((int x, int y) p, (int x, int y) pp)
    {
        var part = map[p.x, p.y];
        if(new char[] {'<','>','^','v'}.Any(x => x == part))
        {
            var crashed = cars.Where(x => x.Coord.x == p.x && x.Coord.y == p.y && !x.Crashed);
            var crashee = cars.Where(x => x.Coord.x == pp.x && x.Coord.y == pp.y);
            foreach(var c in crashed.Concat(crashee)) 
            {
                map[c.Coord.x,c.Coord.y] = c.Road;
                c.Crashed = true;
            }
        }
    }

    private IEnumerable<(int x, int y)> FindCars(char[,] map)
    {
        for(var y = 0; y <= map.GetUpperBound(1); y++)
        {
            for(var x = 0; x <= map.GetUpperBound(0); x++)
                if(new char[] { '>', 'v', '<', '^'}.Any(c => c == map[x, y]))
                    yield return (x, y);
        }
    }

    private void Print(char[,] map)
    {
        for(var y = 0; y <= map.GetUpperBound(1); y++)
        {
            var line = string.Empty;
            for(var x = 0; x <= map.GetUpperBound(0); x++)
                line += map[x, y];
            Console.WriteLine(line);
        }
    }

    private class Car
    {
        public Turn LastTurn { get; internal set; }
        public (int x, int y) Coord { get; internal set; }
        public char Direction { get; internal set; }
        public char Road { get; internal set; }
        public bool Crashed { get; internal set; }
    }

    private enum Turn
    {
        Initial,
        Left,
        Right,
        Straight
    }
}