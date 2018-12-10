using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Advent2018_7
{
    private List<Relation> _inputData = new List<Relation>();

    public void Solve()
    {
        const string regex = "Step ([A-Z]) must be finished before step ([A-Z]) can begin.";
#region input
        var input = @"Step G must be finished before step X can begin.
Step X must be finished before step B can begin.
Step A must be finished before step I can begin.
Step D must be finished before step H can begin.
Step O must be finished before step T can begin.
Step H must be finished before step C can begin.
Step S must be finished before step E can begin.
Step U must be finished before step M can begin.
Step M must be finished before step Z can begin.
Step R must be finished before step N can begin.
Step C must be finished before step Q can begin.
Step T must be finished before step P can begin.
Step I must be finished before step W can begin.
Step W must be finished before step N can begin.
Step P must be finished before step J can begin.
Step N must be finished before step F can begin.
Step Y must be finished before step J can begin.
Step J must be finished before step L can begin.
Step L must be finished before step E can begin.
Step E must be finished before step B can begin.
Step Q must be finished before step B can begin.
Step F must be finished before step K can begin.
Step V must be finished before step K can begin.
Step Z must be finished before step B can begin.
Step B must be finished before step K can begin.
Step G must be finished before step U can begin.
Step E must be finished before step V can begin.
Step A must be finished before step Z can begin.
Step C must be finished before step V can begin.
Step R must be finished before step B can begin.
Step Q must be finished before step Z can begin.
Step R must be finished before step K can begin.
Step T must be finished before step B can begin.
Step L must be finished before step B can begin.
Step M must be finished before step K can begin.
Step T must be finished before step Z can begin.
Step W must be finished before step B can begin.
Step I must be finished before step E can begin.
Step A must be finished before step M can begin.
Step V must be finished before step Z can begin.
Step Y must be finished before step B can begin.
Step Q must be finished before step F can begin.
Step W must be finished before step Y can begin.
Step U must be finished before step K can begin.
Step D must be finished before step F can begin.
Step P must be finished before step F can begin.
Step N must be finished before step L can begin.
Step H must be finished before step T can begin.
Step H must be finished before step L can begin.
Step C must be finished before step T can begin.
Step H must be finished before step I can begin.
Step Z must be finished before step K can begin.
Step L must be finished before step Z can begin.
Step Y must be finished before step K can begin.
Step I must be finished before step V can begin.
Step P must be finished before step K can begin.
Step P must be finished before step N can begin.
Step G must be finished before step D can begin.
Step I must be finished before step J can begin.
Step H must be finished before step K can begin.
Step L must be finished before step Q can begin.
Step D must be finished before step M can begin.
Step O must be finished before step V can begin.
Step R must be finished before step L can begin.
Step D must be finished before step W can begin.
Step M must be finished before step J can begin.
Step O must be finished before step R can begin.
Step N must be finished before step Z can begin.
Step Y must be finished before step V can begin.
Step W must be finished before step L can begin.
Step U must be finished before step Y can begin.
Step S must be finished before step V can begin.
Step M must be finished before step P can begin.
Step X must be finished before step A can begin.
Step A must be finished before step E can begin.
Step A must be finished before step L can begin.
Step A must be finished before step R can begin.
Step V must be finished before step B can begin.
Step P must be finished before step B can begin.
Step E must be finished before step F can begin.
Step T must be finished before step V can begin.
Step S must be finished before step R can begin.
Step T must be finished before step F can begin.
Step P must be finished before step Y can begin.
Step A must be finished before step C can begin.
Step J must be finished before step F can begin.
Step H must be finished before step B can begin.
Step C must be finished before step E can begin.
Step P must be finished before step E can begin.
Step D must be finished before step I can begin.
Step X must be finished before step F can begin.
Step T must be finished before step Q can begin.
Step J must be finished before step B can begin.
Step C must be finished before step B can begin.
Step P must be finished before step Q can begin.
Step H must be finished before step R can begin.
Step F must be finished before step B can begin.
Step T must be finished before step J can begin.
Step A must be finished before step W can begin.
Step N must be finished before step K can begin.
Step T must be finished before step E can begin.";
#endregion
        input = @"Step C must be finished before step A can begin.
  Step C must be finished before step F can begin.
  Step A must be finished before step B can begin.
  Step A must be finished before step D can begin.
  Step B must be finished before step E can begin.
  Step D must be finished before step E can begin.
  Step F must be finished before step E can begin.";

        foreach(Match match in Regex.Matches(input, regex, RegexOptions.Multiline)) 
        {
                _inputData.Add(new Relation { Letter = match.Groups[1].Value, FinishBefore = match.Groups[2].Value });
        }
        Console.WriteLine("Done with input.");

        var start = _inputData.Where(x => !_inputData.Any(y => y.FinishBefore == x.Letter)).OrderBy(x => x.FinishBefore).First();
        _inputData.Remove(start);
        
        var letters = new string[] {start.Letter, start.FinishBefore}.ToList();
        _inputData = _inputData.OrderBy(x => x.FinishBefore).ThenBy(x => x.Letter).ToList();

        foreach(var inp in _inputData)
        {
                var index = letters.FindIndex(x => x.ToCharArray()[0] > inp.Letter.ToCharArray()[0]);
                if(!letters.Any(x => x == inp.FinishBefore))
                {
                    if(index > 0)
                        letters.Insert(index + 1, inp.FinishBefore);
                    else
                        letters.Add(inp.FinishBefore);
                }
        }
        Console.WriteLine(string.Join("", letters));
    }

}