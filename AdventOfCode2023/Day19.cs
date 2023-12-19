using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public class Day19 : BaseDay
{
    record PropRange(string Prop, int Min = 1, int Max = 4000)
    {
        public int Min { get; set; } = Min;
        public int Max { get; set; } = Max;
    }

    record RuleList
    {
        public IList<Rule> Rules { get; set; } = new List<Rule>();
        public string Else { get; set; } = "";
        public RuleListType Type { get; set; }
    };

    enum RuleListType { Normal, Accept, Reject };

    enum Operator { Gt, Lt, Gte, Lte }

    record Rule(string Prop, Operator Operator, int Value, string Next)
    {
        public string Prop { get; set; } = Prop;
        public Operator Operator { get; set; } = Operator;
        public int Value { get; set; } = Value;
        public string Next { get; set; } = Next;

        public Rule() : this(null, Operator.Gt, 0, null)
        {
        }

        public string ToVisString() => $"{Prop}_{Operator}_{Value}";

        public override string ToString()
        {
            return ToVisString();
        }
    };

    public override void Run()
    {
        var input = Input;
        // var input = TestInput;
        var directions = input.TakeWhile(s => s != "").ToList();
        var parts = input.SkipWhile(s => s != "").Where(s => s != "").ToList();

        var part1 = false;

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
            rList.RemoveAt(rList.Count - 1);
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


        if (part1)
        {
            Part1(items, rulesMap);
        }
        else
        {
            rulesMap["A"] = new RuleList() { Type = RuleListType.Accept };
            rulesMap["R"] = new RuleList() { Type = RuleListType.Reject };
            Part2(rulesMap);
        }
    }

    record Node(Rule? Rule, Node Left, Node Right)
    {
        public Rule? Rule { get; set; } = Rule;
        public Node Left { get; set; } = Left;
        public Node Right { get; set; } = Right;
        public RuleListType Type { get; set; }
    };

    private void Part2(Dictionary<string, RuleList> rulesMap)
    {
        var parentNode = new Node(null, null, null);
        MakeNode(rulesMap, parentNode, rulesMap["in"], 0);
        Console.WriteLine(GetDotRepresentation(parentNode));

        IList<PropRange> initRanges =
        [
            new PropRange("x"),
            new PropRange("m"),
            new PropRange("a"),
            new PropRange("s")
        ];

        Console.WriteLine(Traverse(parentNode, initRanges, []));
    }


    private long Traverse(Node node, IList<PropRange> ranges, IList<Node> path)
    {
        if (node == null) return 0;

        path = [..path, node];
        if (node.Type == RuleListType.Accept)
        {
            return CountCombinations(ranges, path);
        }

        if (node.Type == RuleListType.Reject)
        {
            return 0;
        }

        ranges = UpdateRanges(node.Rule!, ranges);

        return Traverse(node.Left, ranges, path) + Traverse(node.Right, ranges, path);
    }

    private long CountCombinations(IList<PropRange> ranges, IList<Node> path)
    {
        // foreach (var node in path)
        // {
        //     Console.WriteLine(node.Rule?.ToVisString());
        // }
        //
        // foreach (var range in ranges)
        // {
        //     Console.WriteLine(range);
        // }

        return ranges.Select(r => (long)r.Max - (long)r.Min + 1).Aggregate((a, b) => a * b);
    }

    private IList<PropRange> UpdateRanges(Rule rule, IList<PropRange> ranges)
    {
        if (rule == null) return [..ranges];
        var rangeForProp = ranges.First(r => r.Prop == rule.Prop);

        var newrule = rangeForProp with { }; //clone

        if (rule.Operator == Operator.Gt)
        {
            newrule.Min = rule.Value + 1;
        }

        if (rule.Operator == Operator.Gte)
        {
            newrule.Min = rule.Value;
        }

        if (rule.Operator == Operator.Lt)
        {
            newrule.Max = rule.Value - 1;
        }

        if (rule.Operator == Operator.Lte)
        {
            newrule.Max = rule.Value;
        }

        return [newrule, ..ranges.Where(r => r != rangeForProp)];
    }

    private string GetDotRepresentation(Node parentNode)
    {
        var sb = new StringBuilder();

        sb.AppendLine("digraph BST {");
        GetDotRepresentation(parentNode, sb);
        sb.AppendLine("}");

        return sb.ToString();
    }

    private void GetDotRepresentation(Node root, StringBuilder sb)
    {
        if (root == null) return;


        if (root.Left != null && root.Left.Type != RuleListType.Normal)
        {
            sb.AppendLine($"{root.Rule?.ToVisString()} -> {root.Left.Type}");
        }
        else if (root.Left != null)
        {
            sb.AppendLine($"{root.Rule?.ToVisString()} -> {root.Left.Rule?.ToVisString()}");
        }

        if (root.Right != null && root.Right.Type != RuleListType.Normal)
        {
            sb.AppendLine($"{root.Rule?.ToVisString()} -> {root.Right.Type}");
        }
        else if (root.Right != null)
        {
            sb.AppendLine($"{root.Rule?.ToVisString()} -> {root.Right.Rule?.ToVisString()}");
        }


        GetDotRepresentation(root.Left, sb);
        GetDotRepresentation(root.Right, sb);
    }

    //TODO FIND EXISTING NODE????
    private void MakeNode(IDictionary<string, RuleList> ruleMap, Node parentNode, RuleList list, int ruleInListInd)
    {
        if (list.Type == RuleListType.Accept)
        {
            parentNode.Left = new Node(null, null, null) { Type = RuleListType.Accept };
            return;
        }

        if (list.Type == RuleListType.Reject)
        {
            parentNode.Right = new Node(null, null, null) { Type = RuleListType.Reject };
            return;
        }

        var rule = list.Rules[ruleInListInd];
        var left = new Node(rule, null, null);
        var counterRule = rule with
        {
            Operator = rule.Operator switch
            {
                Operator.Gt => Operator.Lte,
                Operator.Lt => Operator.Gte,
                Operator.Gte => Operator.Lt,
                Operator.Lte => Operator.Gt,
                _ => throw new ArgumentOutOfRangeException()
            },
        };
        var right = new Node(counterRule, null, null);

        MakeNode(ruleMap, left, ruleMap[rule.Next], 0);

        if (ruleInListInd + 1 == list.Rules.Count)
        {
            MakeNode(ruleMap, right, ruleMap[list.Else], 0);
        }
        else
        {
            MakeNode(ruleMap, right, list, ruleInListInd + 1);
        }


        parentNode.Left = left;
        parentNode.Right = right;
    }

    private static void Part1(List<Dictionary<string, int>> items, Dictionary<string, RuleList> rulesMap)
    {
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