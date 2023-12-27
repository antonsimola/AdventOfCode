using System.Numerics;
using Combinatorics.Collections;


namespace AdventOfCode2023;

public class Day24 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;
        var lines = new List<(PB, PB, PB)>();
        foreach (var line in input)
        {
            var sp = line.Split("@");
            var vectorStart = sp[0].Split(",").Select(t => t.Trim()).Select(BigInteger.Parse).ToList();
            var vectorDir = sp[1].Split(",").Select(t => t.Trim()).Select(BigInteger.Parse).ToList();
            PB curPos = (vectorStart[0], vectorStart[1]);
            PB nextPos = (vectorDir[0] + vectorStart[0], vectorDir[1] + vectorStart[1]);

            lines.Add((curPos, nextPos, (vectorDir[0], vectorDir[1])));
        }

        var minX = 200000000000000;
        var minY = 200000000000000;
        var maxX = 400000000000000;
        var maxY = 400000000000000;
        // var minX = 7;
        // var minY = 7;
        // var maxX = 27;
        // var maxY = 27;

        var sum = 0;
        foreach (var pairs in new Combinations<(PB, PB, PB)>(lines, 2, GenerateOption.WithoutRepetition))
        {
            if (pairs.Count > 2) continue;
            var line1 = pairs[0];
            var line2 = pairs[1];

            var p = Helpers.LineIntercect(line1.Item1, line1.Item2, line2.Item1, line2.Item2);
            
            if (p != null)
            {
                var pV = p.Value;

                PB pde = pV;
                var betweenLine1 = pde - line1.Item1;
                var betweenLine2 = pde - line2.Item1;

                 
                var sameDir1 = betweenLine1.Row > 0 == line1.Item3.Row > 0  && betweenLine1.Col > 0 == line1.Item3.Col > 0  ;
                var sameDir2 = betweenLine2.Row > 0 == line2.Item3.Row > 0  && betweenLine2.Col > 0 == line2.Item3.Col > 0  ;
                
                


             

                if (!sameDir1 || !sameDir2)
                {
                    Console.WriteLine("Past " + line1.Item1 + " " + line2.Item1);
                    continue;
                }

                if (
                    minX <= pV.X &&
                    pV.X <= maxX &&
                    minY <= pV.Y &&
                    pV.Y <= maxY
                  
                )
                {
                    Console.WriteLine("OK " + line1.Item1 + " " + line2.Item1);
                    sum += 1;
                    
                }
                else
                {
                    Console.WriteLine("OUTSIDE " + line1.Item1 + " " + line2.Item1);
                }
            }
            else
            {
                Console.WriteLine("PARALLEL " + line1.Item1 + " " + line2.Item1);
            }
        }

        Console.WriteLine(sum);
    }
}