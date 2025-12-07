using AdventOfCodeLib;

namespace AdventOfCode2025;

public class Day1 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        var current = 50;
        var count = 0;
        foreach (var line in input)
        {
            var lr = line[0];
            var num = int.Parse(line[1..]);
            while (num > 0)
            {
                if (lr == 'L')
                {
                    current--;
                }
                else
                {
                    current++;
                }

                if (current > 99)
                {
                    current = 0;
                }

                if (current < 0)
                {
                    current = 99;
                }

                if (current == 0) count++;
                num--;
            }

            // count += Math.Abs(next / 100);
            // count += next % 100;
            // current = FixTo0_99(current);
        }

        Console.WriteLine(count);
    }

    int FixTo0_99(int num)
    {
        while (num > 99 || num < 0)
        {
            if (num < 0)
            {
                num += 100;
            }
            else if (num > 99)
            {
                num -= 100;
            }
            else
            {
                break;
            }
        }

        return num;
    }
}