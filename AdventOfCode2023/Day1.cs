using System.Text.RegularExpressions;

namespace AdventOfCode2023;

public class Day1 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        //var input = TestInput;

        var stringValues = new Dictionary<Regex, int>()
        {
            { new Regex("one"), 1 },
            { new Regex("two"), 2 },
            { new Regex("three"), 3 },
            { new Regex("four"), 4 },
            { new Regex("five"), 5 },
            { new Regex("six"), 6 },
            { new Regex("seven"), 7 },
            { new Regex("eight"), 8 },
            { new Regex("nine"), 9 },
            { new Regex("1"), 1 },
            { new Regex("2"), 2 },
            { new Regex("3"), 3 },
            { new Regex("4"), 4 },
            { new Regex("5"), 5 },
            { new Regex("6"), 6 },
            { new Regex("7"), 7 },
            { new Regex("8"), 8 },
            { new Regex("9"), 9 },
        };
        var sum = 0;
        foreach (var line in input)
        {
            var min = int.MaxValue;
            var minVal = 0;
            var max = -1;
            var maxVal = 0;
            foreach (var (reg, val) in stringValues)
            {
                foreach (Match match in reg.Matches(line))
                {
                    var i = match.Index;
                    if (i < min)
                    {
                        min = i;
                        minVal = val;
                    }

                    if (i > max)
                    {
                        max = i;
                        maxVal = val;
                    }
                }
            }

            sum += int.Parse(minVal + "" + maxVal);
        }


        WriteLine(sum);
    }
}