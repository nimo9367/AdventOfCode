using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class Advent2018_14
{
    private List<int> _scoreboard;
    private List<int> _scoreboard2;
    private int _counter;
    private int _input;
    private string _inputString;
    private IEnumerable<int> _inputs;
    private int _inpCount;

    public void Solve() 
    {
        _input = 74501;

        var curr1 = 3;
        var curr2 = 7;
        var index1 = 0;
        var index2 = 1;

        _scoreboard = new List<int> {curr1, curr2};
        _scoreboard2 = new List<int> {curr1, curr2};
        _counter = 0;

        while(_counter <= _input + 10)
        {
            var result = RecepieMangler(index1, index2);
            index1 = result.Item1;
            index2 = result.Item2;
            _counter++;
        }
        Console.WriteLine($"Result: {string.Join("", _scoreboard.Skip(_input).Take(10))}");
        
        
        _inputs = "074501".ToCharArray().Select(x => int.Parse(x.ToString())).ToArray();
        _inpCount = _inputs.Count();
        curr1 = 3;
        curr2 = 7;
        index1 = 0;
        index2 = 1;

        _scoreboard = new List<int> {curr1, curr2};
        _counter = 0;

        var sw = new Stopwatch();
        sw.Start();
        var lastFive = new List<int>();
        while(true)
        {
            var result = RecepieMangler2(index1, index2, lastFive);

            if(result.Item1 == -1)
            {
                Console.WriteLine($"Res: { result.Item2} ");
                break;
            }

            index1 = result.Item1;
            index2 = result.Item2;

            
            if(_counter % 10000 == 0) 
            {
                Console.WriteLine($"Number: { _counter } in { sw.ElapsedMilliseconds / 1000 } seconds");
                sw.Reset();
                sw.Start();
            }
            
            _counter++;
        }
    }


    private (int, int) RecepieMangler2(int index1, int index2, List<int> lastFive)
    {
        var curr1 = _scoreboard2[index1];
        var curr2 = _scoreboard2[index2];
        var newScore = curr1 + curr2;
        if(newScore > 9)
        {
            var scores = new int[] { 1, newScore - 10}; 
            for(int i = 0; i < scores.Count(); i++)
            {
                _scoreboard2.Add(scores[i]);
                AddMax5(lastFive, scores[i]);
                if(IsSame(lastFive, _inputs))
                    return (-1, _scoreboard2.Count - _inpCount);
            }
        }
        else 
        {
            _scoreboard2.Add(newScore);
            AddMax5(lastFive, newScore);
        }
        
        if(IsSame(lastFive, _inputs))
            return (-1, _scoreboard2.Count - _inpCount);
        
        index1 = (curr1 + index1 + 1) % _scoreboard2.Count;
        index2 = (curr2 + index2 + 1) % _scoreboard2.Count;
        return (index1, index2);
    }

    private void AddMax5(List<int> lastFive, int item)
    {
        lastFive.Add(item);
        if(lastFive.Count() > 6)
            lastFive.RemoveAt(0);
    }

    private bool IsSame(IEnumerable<int> first, IEnumerable<int> second)
    {
        if(first.Count() == 0)
            return false;
        for(int i = 0; i < first.Count(); i++)
            if(first.ElementAt(i) != second.ElementAt(i))
                return false;
        return true;
    }

    private (int, int) RecepieMangler(int index1, int index2)
    {
        var curr1 = _scoreboard.ElementAt(index1);
        var curr2 = _scoreboard.ElementAt(index2);
        var newScore = curr1 + curr2;
        if(newScore > 9)
            _scoreboard.AddRange(newScore.ToString().ToCharArray().Select(x => int.Parse(x.ToString())));
        else
            _scoreboard.Add(newScore);
        
        index1 = (curr1 + index1 + 1) % _scoreboard.Count();
        index2 = (curr2 + index2 + 1) % _scoreboard.Count();
        return (index1, index2);
    } 
}