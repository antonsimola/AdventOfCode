using AdventOfCodeLib;

namespace AdventOfCode2025;

public class Day7 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        var beams = new bool[input[0].Length];
        var splitCount = 0;
        foreach (var line in input)
        {
            var chars = line.ToCharArray();
            for (var i = 0; i < chars.Length; i++)
            {
                var ch = chars[i];

                if (ch == 'S')
                {
                    beams[i] = true;
                }
                else if (ch == '^' && beams[i])
                {
                    beams[i - 1] = true;
                    beams[i] = false;
                    beams[i+1] = true;
                    splitCount += 1;
                }
            }
        }

        Console.WriteLine(splitCount);
    }
}
 