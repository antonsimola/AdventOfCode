using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Day2 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        //var input = TestInput;
        var sum = 0;
        foreach (var line in input)
        {
            var a = line.Split(":");
            var id = int.Parse(a[0].Split(" ")[1]);
            var shows = a[1].Trim().Split(";");
            var possible = true;
            var minRed = 0;
            var minBlue = 0;
            var minGreen = 0;
            foreach (var show in shows)
            {
                foreach (var cubeCount in show.Split(","))
                {
                    var colorCount = cubeCount.Trim().Split(" ");
                    var count = int.Parse(colorCount[0].Trim());
                    var color = colorCount[1].Trim();

                    if (color == "blue" && count > minBlue)
                    {
                        minBlue = count;
                    }
                    else if (color == "red" && count > minRed)
                    {
                        minRed = count;
                    }
                    else if (color == "green" && count > minGreen)
                    {
                        minGreen = count;
                    }
                }
            }

            if (possible)
            {
                sum += minBlue * minRed * minGreen;
            }
        }

        WriteLine(sum);
    }
}