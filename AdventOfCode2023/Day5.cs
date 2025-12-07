using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Day5 : BaseDay
{
    record Range(long SourceStart, long DestStart, long Length);

    record Map(string From, string To, List<Range> Ranges);

    public override void Run()
    {
        // var input = Input;
        var input = TestInput;

        var seeds = input[0].Split(": ")[1].Split(" ").Select(long.Parse).ToList();


        var ranges = new List<ValueTuple<long, long>>();

        for (int i = 0; i < seeds.Count; i += 2)
        {
            ranges.Add((seeds[i], seeds[i + 1]));
        }

        var max = ranges.Max(r => r.Item1 + r.Item2);
        var min = ranges.Min(r => r.Item1);
        WriteLine(min);
        WriteLine(max);
        WriteLine(max - min);

        Map mapToCollect = null;
        List<Map> maps = new List<Map>();

        foreach (var line in input.Skip(1))
        {
            if (line.Length == 0)
            {
                continue;
            }

            if (char.IsLetter(line[0]))
            {
                var parts = line.Split(" ")[0].Split("-");
                var from = parts[0];
                var to = parts[2];
                mapToCollect = new Map(from, to, new List<Range>());
                maps.Add(mapToCollect);
            }
            else if (mapToCollect != null)
            {
                var range = line.Split(" ").Select(long.Parse).ToList();
                mapToCollect.Ranges.Add(new Range(range[1], range[0], range[2]));
            }
        }

        var globalMin = ranges.AsParallel().Select((seedrange, i) =>
        {
            var min = long.MaxValue;
            var size = seedrange.Item2;
            var counter = 0;
            for (var seed = seedrange.Item1; seed < seedrange.Item1 + seedrange.Item2; seed++)
            {
                var cur = seed;
                foreach (var map in maps)
                {
                    var found = false;
                    foreach (var range in map.Ranges)
                    {
                        if (found)
                        {
                            break;
                        }

                        var rangeStart = range.SourceStart;
                        var rangeEnd = range.SourceStart + range.Length;

                        if (rangeStart <= cur && cur <= rangeEnd)
                        {
                            found = true;
                            cur = range.DestStart + cur - rangeStart;
                        }
                    }
                }

                min = Math.Min(cur, min);
                if (counter % 100_0000 == 0)
                {
                    WriteLine($"Progress {i} {+((double)counter / (double)size) * 100} % ");
                }

                counter++;
            }

            return min;
        }).Min();


        WriteLine(globalMin);
    }
}