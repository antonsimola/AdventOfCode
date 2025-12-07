using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCodeLib;
using MathNet.Numerics;

namespace AdventOfCode2023;

public class Day12 : BaseDay
{
    private Dictionary<string, ICollection<IList<char>>> Cache = new();

    //TODO brute force, takes years on full input. Memoization? Split into unknown parts and known parts
    public override void Run()
    {
        //https://old.reddit.com/r/adventofcode/comments/18ghux0/2023_day_12_no_idea_how_to_start_with_this_puzzle/kd0npmi/
        var input = Input;
        // var input = AllTestInputs[1];
        var grandSum = 0;
        var progress = 0;
        var cache = new Dictionary<string, IList<string>>();

        var reg = new Regex("([?]*)");

        for (var i = 1; i < 15; i++)
        {
            Console.WriteLine(i);
            Generate(new string('?', i), cache);
        }

        var globalmax = 0;
        foreach (var line in input)
        {
            var split = line.Split(" ");
            // var records = split[0];
            var records = string.Join("?", Enumerable.Range(0, 5).Select(i => split[0]));
            var checks = split[1].ParseIntList(",");
            // checks = Enumerable.Range(0, 5).Select(i => checks).Aggregate((a, b) => a.Concat(b).ToList());

            // var max = 0;
            // foreach (Match match in reg.Matches(records))
            // {
            //         max = Math.Max(   match.Groups[1].Length, max);
            // }
            //
            // Console.WriteLine(max);
            // globalmax = Math.Max(globalmax, max);

            // on i-th char, group the brokens by count
            var dict = new Dictionary<int, Dictionary<int, int>>();
            
            Action<int, Transition> counter = (i, transition) =>
            {
                if (transition.From == "intact" && transition.To == "broken")
                {
                }
              
                else if (transition.From == "broken" && transition.To == "intact")
                {
                }

                else if (transition.From == "intact" && transition.To == "intact")
                {
                }
                else if (transition.From == "broken" && transition.To == "broken")
                {
                }
            };

            new StateMachinery(["intact", "broken"], ['?', '.', '#'], [
                new Transition("intact", '.', "intact", counter),
                new Transition("intact", '#', "broken", counter),
                new Transition("intact", '?', "broken", counter),
                new Transition("intact", '?', "intact", counter),
                new Transition("broken", '.', "intact", counter),
                new Transition("broken", '#', "broken", counter),
                new Transition("broken", '?', "intact", counter),
                new Transition("broken", '?', "broken", counter),
            ], "intact", []);


            // var checksQ = new Queue<int>(checks);
            //
            // var ways = records.Split(".");
            //
            // var sum = 0;
            //  
            // foreach (var w in ways)
            // {
            //      Generate(w, cache);
            //      
            // }
            // grandSum += sum;
        }

        Console.WriteLine("--------");
        Console.WriteLine(globalmax);
        Console.WriteLine(grandSum);
    }


    public int RecCheck(string s, Queue<int> checksQ, Dictionary<string, IList<string>> cache)
    {
        if (s.Length == 0 && checksQ.Count == 0)
        {
            Console.WriteLine("Counting 1");
            return 1;
        }

        if (s.Length == 0 && checksQ.Count != 0) return 0;

        var sum = 0;
        if (s[0] == '.')
        {
            sum += RecCheck(s[1..], checksQ, cache);
        }
        else if (s[0] == '?')
        {
            sum += RecCheck("#" + s[1..], checksQ, cache);
            sum += RecCheck("." + s[1..], checksQ, cache);
        }

        else if (s[0] == '#')
        {
            var i = 0;
            while (i < s.Length && s[i] == '#')
            {
                i++;
            }

            if (i == s.Length || (s[i] == '.' && i == checksQ.Peek()))
            {
                var newQ = new Queue<int>(checksQ);
                var check = newQ.Dequeue();
                sum += RecCheck(s[i..], newQ, cache);
            }
            else
            {
                sum += RecCheck(s[i..], checksQ, cache);
            }
        }

        return sum;
    }


    public IList<string> Generate(string s, Dictionary<string, IList<string>> memo)
    {
        // generate from bottom to top, only ? , ??, ???, ???, then merge those into strings?
        // divide and conquer?
        if (memo.TryGetValue(s, out var cached))
        {
            return cached;
        }

        if (s == "?") return memo.GetOrAdd("?", e => ["#", "."]);
        if (s == "#") return memo.GetOrAdd("#", e => ["#"]);
        if (s == ".") return memo.GetOrAdd(".", e => ["."]);

        var head = s[0];
        var tail = s.Substring(1);

        var res = Generate(tail, memo);

        var appended = new List<string>();
        if (head == '?')
        {
            appended.AddRange(res.Select(str => "." + str));
            appended.AddRange(res.Select(str => "#" + str));
        }
        else
        {
            appended.AddRange(res.Select(str => head + str));
        }


        memo[s] = appended;
        return appended;
    }


    public IList<string> Generate(string s)
    {
        var res = Generate(s, new Dictionary<string, IList<string>>());

        return res;
    }
}