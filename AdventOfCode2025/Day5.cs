using System.Numerics;
using AdventOfCodeLib;

namespace AdventOfCode2025;

public class Day5 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;
        var freshRanges = new List<Range>();
        var ingToCheck = new List<BigInteger>();
        var parseIngToCheck = false;
        foreach (var line in input)
        {
            if (line == "")
            {
                parseIngToCheck = true;
                continue;
            }

            if (parseIngToCheck)
            {
                ingToCheck.Add(BigInteger.Parse(line));
            }
            else
            {
                var split = line.Split("-");
                freshRanges.Add(new Range(BigInteger.Parse(split[0]),
                    BigInteger.Parse(split[1])));
            }
        }

        var expandedRanges = new List<Range>();
        freshRanges = freshRanges.OrderBy(r => r.Start).ToList();
        var currentInterval = freshRanges[0];
        foreach (var range in freshRanges)
        {
            if (currentInterval.End >= range.Start)
            {
                //overlaps
                currentInterval.End = BigInteger.Max(currentInterval.End, range.End);
            }
            else
            {
                expandedRanges.Add(currentInterval);
                currentInterval = range;
            }
        }
        expandedRanges.Add(currentInterval);


        Console.WriteLine(expandedRanges.Select(r => r.Count).Aggregate((r1,r2) => r1 + r2));
    }

    class Range
    {
        public BigInteger Start { get; set; }
        public BigInteger End { get; set; }
        public BigInteger Count => (End - Start) + 1; 
        public Range(BigInteger Start, BigInteger End)
        {
            this.Start = Start;
            this.End = End;
        }

        public bool Overlaps(Range other)
        {
            if (other.Start >= Start && End >= other.Start)
            {
                return true;
            }
            if (other.End <= End && other.End >= Start)
            {
                return true;
            }

            return false;
        }

        public Range MergeOverlapping(Range other)
        {
            return new Range(BigInteger.Min(Start, other.Start), BigInteger.Max(End, other.End));
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
        }
    }
}