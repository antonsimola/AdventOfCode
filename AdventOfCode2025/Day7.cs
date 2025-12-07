using System.Numerics;
using AdventOfCodeLib;

namespace AdventOfCode2025;

public class Day7 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        var beams = new BigInteger[input[0].Length];
        var splitCount = 0;
        foreach (var line in input)
        {
            var chars = line.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                var ch = chars[i];

                if (ch == 'S')
                {
                    beams[i]++;
                }
                else if (ch == '^')
                {
                    var prev = beams[i];
                    beams[i - 1] += prev;
                    beams[i] = 0;
                    beams[i+1] += prev;
                }
            }
        }

        Console.WriteLine(beams.Aggregate((acc,cur) => acc + cur));
    }
}
 