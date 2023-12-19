using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public class Day19 : BaseDay
{
    
    record RuleList()
    {
        public IList<Rule> Rules { get; set; } = new List<Rule>();
        public string Else { get; set; } = "";
    };

    enum Operator
    {
        Gt,
        Lt,
    }

    record Rule(string Prop, Operator Operator, int Value, string Next);

    public override void Run()
    {
        var input = Input;
        // var input = TestInput;
        var directions = input.TakeWhile(s => s != "").ToList();
        var parts = input.SkipWhile(s => s != "").Where(s => s != "").ToList();

        Console.WriteLine();


        var regex = new Regex(@"([a-z]+)([<>])(\d+):([a-zA-Z]+)");
        var partRegex = new Regex(@"\{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");

        var rulesMap = new Dictionary<string, RuleList>();
        var items = new List<Dictionary<string, int>>();
        foreach (var d in directions)
        {
            var split = d.Split("{");
            var name = split[0];
            var rules = split[1].Replace("}", "");
            var ruleList = new RuleList();
            var rList = rules.Split(",").ToList();
            var els = rList[rList.Count - 1];
            ruleList.Else = els;
            rList.RemoveAt(rList.Count -1);
            foreach (var r in rList)
            {
                var matches = regex.Matches(r);
                var prop = matches[0].Groups[1].Value;
                var op = matches[0].Groups[2].Value;
                var value = matches[0].Groups[3].Value;
                var next = matches[0].Groups[4].Value;
                ruleList.Rules.Add(new Rule(prop, op == ">" ? Operator.Gt : Operator.Lt, int.Parse(value), next));
            }
            rulesMap.Add(name, ruleList);
        }

        foreach (var p in parts)
        {
            var dir = new Dictionary<string, int>();
            var m = partRegex.Matches(p)[0];
            dir["x"] = int.Parse(m.Groups[1].Value);
            dir["m"] = int.Parse(m.Groups[2].Value);
            dir["a"] = int.Parse(m.Groups[3].Value);
            dir["s"] = int.Parse(m.Groups[4].Value);
            items.Add(dir);
        }


        var sum = 0;
        foreach (var item in items)
        {
            var curRule = "in";

            while (true)
            {
                if (curRule == "A")
                {
                     sum += item.Values.Sum();
                    break;
                }
                if (curRule == "R")
                {
                    break;
                }

                var rules = rulesMap[curRule];
                var found = false;
                foreach (var r in rules.Rules)
                {
                    var v = item[r.Prop];
                    if (r.Operator == Operator.Gt && v > r.Value
                        || r.Operator == Operator.Lt && v < r.Value
                        )
                    {
                        found = true;
                        curRule = r.Next;
                        break;
                    }


                }

                if (!found)
                {
                    curRule = rules.Else;
                }
            }
        }
        
        Console.WriteLine(sum);
        
    }
}