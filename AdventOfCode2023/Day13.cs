using System.Diagnostics;
using System.Text;
using System.Threading.Channels;
using MathNet.Numerics;

namespace AdventOfCode2023;

public class Day13 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;


        var fullText = string.Join("\n", input);
        var patterns = fullText.Split("\n\n");

        var sum = 0;

        foreach (var pattern in patterns) 
        {
            var lines = pattern.Split("\n");
            var original = Test(lines, null);
            var smudgeFixes = Generate(lines);
            var changed = false;
            foreach (var smudgeFix in smudgeFixes)
            {
                var res = Test(smudgeFix, original);
                if (original.Row != res.Row && res.Row > 0)
                {
                    changed = true;
                    sum += res.Row * 100;
                    break;
                }

                if (original.Col != res.Col && res.Col > 0)
                {
                    changed = true;

                    sum += res.Col;
                    break;
                }
            }

            if (!changed)
            {
                foreach (var line in pattern)
                {
                    Console.Write(line);
                }

                Console.WriteLine();

                Debugger.Break();
            }
        }

        Console.WriteLine(sum);
    }

    public IEnumerable<string[]> Generate(string[] pattern)
    {
        var res = new List<string[]>();
        for (var row = 0; row < pattern.Length; row++)
        {
            for (var col = 0; col < pattern[0].Length; col++)
            {
                var c = pattern[row][col];
                var sb = new StringBuilder(pattern[row]);
                sb[col] = c == '#' ? '.' : '#';

                var clone = new string[pattern.Length];
                Array.Copy(pattern, clone, pattern.Length);
                clone[row] = sb.ToString();
                res.Add(clone);
            }
        }

        return res;
    }

    private Res Test(string[] grid, Res? original)
    {
        var res = new Res();
        var columnWise = new List<string>();
        for (var i = 0; i < grid[0].Length; i++)
        {
            var sb = new StringBuilder();
            foreach (var line in grid)
            {
                sb.Append(line[i]);
            }

            columnWise.Add(sb.ToString());
        }

        var rowInd = MirrorCheck(grid, original?.Row ?? -1);
        if (rowInd > -1)
        {
            res.Row = rowInd;
        }

        var colInd = MirrorCheck(columnWise.ToArray(), original?.Col ?? -1);
        if (colInd > -1)
        {
            res.Col = colInd;
        }

        return res;
    }

    public int MirrorCheck(string[] input, int original = -1)
    {
        for (var i = 0; i < input.Length; i++)
        {
            var stack = new Stack<string>();
            foreach (var start in input.Take(i))
            {
                stack.Push(start);
            }

            var queue = new Queue<string>();
            foreach (var start in input.Skip(i))
            {
                queue.Enqueue(start);
            }


            var lenMin = Math.Min(stack.Count, queue.Count);
            var ok = lenMin > 0;
            for (var j = 0; j < lenMin; j++)
            {
                var s = stack.Pop();
                var q = queue.Dequeue();
                if (s != q)
                {
                    ok = false;
                    break;
                }
            }

            if (ok && original != i)
            {
                return i;
            }
        }

        return -1;
    }

    record Res
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }
}