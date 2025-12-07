using AdventOfCodeLib;

namespace AdventOfCode2025;

public class Day2 : BaseDay
{
    public override void Run()
    {
        var input = Input[0];
        // var input = string.Join("", TestInput);
        var idRanges =  input.Split(",").Select(l =>
        {
            var ids = l.Split("-");
            return new {Start = long.Parse(ids[0]), End = long.Parse(ids[1])};
        }).ToList();

        var count = 0;
        var sum = 0;
        var list = new List<long >();
        foreach (var range in idRanges)
        {
            for (long  i = range.Start; i <= range.End; i++)
            {
                var str = i.ToString();
                for (int j = 1; j <= (str.Length / 2); j++)
                {
                    var subs = ToSubArray(str, j);
                    if (subs.Count > 0 && new HashSet<string>(subs).Count == 1)
                    {
                        list.Add(i);
                        break;
                    }
                }
            }
        }
        Console.WriteLine(list.Sum());
    }

    public List<string> ToSubArray(string str, int size)
    {
        if (str.Length % size != 0) return new List<string>();
        var list = new List<string>();
        var i = 0;
        while (true)
        {
            var start = i * size;
            if (start >= str.Length) return list;
            list.Add(str[start .. (start+size)]);
            i++;
        }
    }
}