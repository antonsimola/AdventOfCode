namespace AdventOfCode2023;

public class Day8 : BaseDay
{
    record Node(Node Left, Node Right, string l, string r, string Cur)
    {
        public Node Left { get; set; } = Left;
        public Node Right { get; set; } = Right;
        public string Cur { get; set; } = Cur;
    }


    public override void Run()
    {
        var input = Input;
        //var input = TestInput;

        var instructions = input[0];


        var nodes = new List<Node>();

        foreach (var l in input.Skip(2))
        {
            var s = l.Split(" = ");
            var nexts = s[1].Split(", ");
            var left = nexts[0].Substring(1, 3);
            var right = nexts[1].Substring(0, 3);
            var cur = s[0];
            var n = new Node(null, null, left, right, cur);
            nodes.Add(n);
        }

        foreach (var node in nodes)
        {
            node.Left = nodes.FirstOrDefault(n => n.Cur == node.l);
            node.Right = nodes.FirstOrDefault(n => n.Cur == node.r);
        }


        var currentNodes = nodes
            .Select(kv => kv.Cur.EndsWith("A") ? kv : null)
            .Where(c => c != null)
            .ToList();


        var mins = new List<long>();

        foreach (var curNode in currentNodes)
        {
            var count = 0;
            var cNode = curNode;
            while (true)
            {
                foreach (var i in instructions)
                {
                    count++;
                    var everyZ = true;

                    var n = i == 'L' ? cNode.Left : cNode.Right;
                    if (!n.Cur.EndsWith("Z"))
                    {
                        everyZ = false;
                    }

                    if (everyZ)
                    {
                        goto o;
                    }

                    cNode = n;
                }
            }

            o:
            mins.Add(count);
        }

        WriteLine(LeastCommonMultiple(mins));
    }

    static long LeastCommonMultiple(List<long> args)
    {
        return args.Aggregate(LeastCommonMultiple);
    }


    static long GreatestCommonDivisor(long a, long b)
    {
        if (a == 0)
            return b;
        return GreatestCommonDivisor(b % a, a);
    }

    static long LeastCommonMultiple(long a, long b)
    {
        return (a / GreatestCommonDivisor(a, b)) * b;
    }
}