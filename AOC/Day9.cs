namespace AOC;

public class Day9 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        //var input = TestInput;
        var seqs = new List<List<int>>();
        foreach (var line in input)
        {
            seqs.Add(line.ParseIntList());
        }

        var sum = 0;
        foreach (var seq in seqs)
        {
            var result = new List<List<int>>() { seq };
            GetDiffsRec(seq, result);

            result.Reverse();
            result.First().Insert(0, 0);

            var i = 0;
            foreach (var first in result)
            {
                if (i + 1 < result.Count)
                {
                    var next = result[i + 1];
                    var l = next.First();
                    next.Insert(0, l - first.First());
                }

                i++;
            }

            sum += result.Last().First();
        }

        WriteLine(sum);
    }

    public static List<List<int>> GetDiffsRec(List<int> seq, List<List<int>> result)
    {
        var diffs = seq.Skip(1).Zip(seq).Select(pair => pair.First - pair.Second).ToList();
        result.Add(diffs);
        if (diffs.All(e => e == 0))
        {
            return result;
        }

        return GetDiffsRec(diffs, result);
    }
}